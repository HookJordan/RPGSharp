using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using System.Drawing;
using SdlDotNet.Graphics.Primitives;

namespace Engine.Core.Scenes
{
    public class OpenWorld : GameScene
    {
        public World.Camera Camera { get; set; }
        public World.Map.WorldMap Map { get; set; }

        // list of triggers (all optional so that they are preloaded?)
        // convert to list of trigger id, match to file name ? 
        // file name will have trigger data 
        public List<World.Triggers.Trigger> GameTriggers { get; set; }

        // player avatar 
        public Sprite.Avatar Avatar { get; set; }
        public List<GamePanel.PanelButton> PanelButtons = new List<GamePanel.PanelButton>(); 

        // npc {id, npc} 
        public Dictionary<int, Sprite.NPC> NPC { get { return Map.NPC; } }
        int buddyId = 0; 

        // cursor surface
        public string displayCursor { get; set; }
        public Dictionary<string, Surface> Cursors { get; set; }

        // for movement in editor
        private Key currentKey = Key.Unknown; 

        // tick counters for movement and events 
        public int TickCount = 0;

        private List<Particles.ParticleSystem> Emitters = new List<Particles.ParticleSystem>(); 

        public OpenWorld()
        {
            // load triggers or list of triggers when updates 
            this.GameTriggers = new List<World.Triggers.Trigger>();
            LoadTriggerScripts();

            // setup camera 
            //this.Camera = new World.Camera(-3, 16); 
            //this.Avatar = new Sprite.Avatar(8, 24);

            // setup game map
            this.Map = World.Map.MapFactory.FromFile(@"Data\maps\0.map");

            this.Camera = new World.Camera(Map.SpawnX, Map.SpawnY);
            this.Avatar = new Sprite.Avatar(Map.SpawnX, Map.SpawnY);

            #region SetupPanels 
            int panelX = Settings.Width - ((Settings.PanelWidth - 1) * Settings.tileWidth);
            int panelWidth = Settings.Width - panelX;
            int panelY = 0;
            int panelHeight = Settings.Height;

            //PanelButtons.Add(new GamePanel.PanelButton("Skills", panelX + Settings.tileWidth, 96 + 40, 32, 32));
            PanelButtons.Add(new GamePanel.StatPanel(panelX + Settings.tileWidth, 96 + 40, 32, 32));
            //PanelButtons.Add(new GamePanel.PanelButton("Inventory", panelX + Settings.tileWidth * 3, 136, 32, 64, true));
            PanelButtons.Add(new GamePanel.InventoryPanel(panelX + Settings.tileWidth * 3, 136, 32, 64, true));

            // test soem inventory stuff 
            Sprite.Items.InventoryItem coins = new Sprite.Items.InventoryItem(1, "Coins", "Currency", Sprite.Items.ItemType.Currency, true);
            coins.Add(25);
            ((GamePanel.InventoryPanel)PanelButtons[1]).Slots[0] = coins;

            PanelButtons.Add(new GamePanel.PanelButton("Equipped", panelX + Settings.tileWidth * 6, 136, 32, 64));
            PanelButtons.Add(new GamePanel.PanelButton("Spells", panelX + Settings.tileWidth * 9, 136, 32, 32));
            #endregion

            Settings.Editor = new GUI.MapEditor(this);

            Random r = new Random(1000);
            //NPC = new Dictionary<int, Sprite.NPC>();
            NPC.Add(1111, new Sprite.NPC(10, 14, 1111) { CanMove = true });

            //for(int i = 0; i < 25; i++)
            //{
            //    int id = r.Next(1000, 9999); 

            //    while(NPC.ContainsKey(id))
            //    {
            //        id = r.Next(1000, 9999);
            //    }

            //    if (buddyId == 0)
            //        buddyId = id; 

            //    Sprite.NPC n = new Sprite.NPC(r.Next(Camera.X + 10, Map.Width), r.Next(Camera.Height + 10, Map.Height), id);

            //    NPC.Add(n.Id, n);
            //}

            //this.Cursor = new Surface((Bitmap)Bitmap.FromFile(@"Data\cursors\default_cursor.png")); 

            this.Cursors = new Dictionary<string, Surface>();
            this.displayCursor = "default"; 
            this.Cursors.Add("default", new Surface((Bitmap)Bitmap.FromFile(@"Data\cursors\default_cursor.png")));
            this.Cursors.Add("select", new Surface((Bitmap)Bitmap.FromFile(@"Data\cursors\select_cursor.png")));
            this.Cursors.Add("target", new Surface((Bitmap)Bitmap.FromFile(@"Data\cursors\target_cursor.png")));
        }

        public void Draw(Surface screen)
        {
            // do game events before draw 
            DoEvents(); 

            int realX = 0, realY = 0;
            var blocked = Content.gameFont.Render("B", Color.Purple); 

            for(int x = -1; x < Camera.Width + 2; x++)
            {
                for(int y = -1; y < Camera.Height + 2; y++)
                {
                    // calculate real locations
                    realX = x + Camera.X;
                    realY = y + Camera.Y;

                    // loop through tiles here 
                    var tile = Map.GetTile(realX, realY);
                    if (tile != null)
                    {
                        Point tileLocation = new Point((x * Settings.tileWidth) + (Avatar.DrawX * -1), (y * Settings.tileHeight) + (Avatar.DrawY * -1));
                        for (int z = 0; z < tile.Layers.Length; z++)
                        {
                            // draw tile + layers 
                            if (tile.GetLayer(z) > 0)
                            {
                                // draw using cached tiles
                                screen.Blit(Map.GetSurface(tile.GetLayer(z)), tileLocation);

                                // screen.Blit(Map.Surfaces[tile.GetLayer(z)], tileLocation);
                            }
                        }

                        // draw items on ground ? 
                        for(int item = 0; item < tile.Items.Count; item++)
                        {
                            
                        }

                        if (Settings.Editor.Visible && tile.Blocked)// && Settings.Editor.radBlocks.Checked)
                        {
                            screen.Blit(blocked, new Point(x * Settings.tileWidth + 12, y * Settings.tileHeight + 10));
                        }

                        if(Settings.Editor.Visible && realX == Map.SpawnX && realY == Map.SpawnY)
                        {
                            var spawn_text = Content.gameFont.Render("Spawn", Color.Red);
                            screen.Blit(spawn_text, new Point(x * Settings.tileWidth + 12, y * Settings.tileHeight + 10));
                            spawn_text.Dispose();
                        }

                        if(Settings.Editor.Visible && tile.TriggerId != -1)// && Settings.Editor.TriggerEditor)
                        {
                            var trigger_id = Content.gameFont.Render(tile.TriggerId.ToString(), Color.Blue);
                            screen.Blit(trigger_id, new Point(x * Settings.tileWidth + 12, y * Settings.tileHeight + 10));
                            trigger_id.Dispose();
                        }

                        if(Settings.displayAvatarPath && Avatar.Points != null)
                        {
                            foreach(var p in Avatar.Points)
                            {
                                if(p.X == realX && p.Y == realY)
                                {
                                    //screen.Fill(new Rectangle(tileLocation, new Size(Settings.tileWidth, Settings.tileHeight)), Color.FromArgb(128, 0, 0, 255));
                                    screen.Blit(Content.PathTile, tileLocation);
                                    break;
                                }
                            }
                        }
                    }

                    if(Settings.displayGrid)
                    {
                        //canvis.Draw(new Line(new Point(0, y * TileSize), new Point(canvis.Width, y * TileSize)), Color.Red);
                        screen.Draw(new Line(new Point(0, y * Settings.tileHeight), new Point(screen.Width, y * Settings.tileHeight)), Color.Red); 
                    }
                }

                if(Settings.displayGrid)
                {
                    screen.Draw(new Line(new Point(x * Settings.tileWidth, 0), new Point(x * Settings.tileWidth, screen.Height)), Color.Red);
                }
            }

            // npc
            #region NPC_Draw
            int avatarX;
            int avatarY; 

            foreach(Sprite.NPC npc in this.NPC.Values)
            {
                // only draw visible content 
                if(Camera.Within(npc.X, npc.Y))
                {
                    avatarX = ((npc.X - Camera.X) * Settings.tileWidth) + npc.DrawX - Avatar.DrawX;
                    avatarY = ((npc.Y - Camera.Y - 1) * Settings.tileHeight) + npc.DrawY - Avatar.DrawY;
                    screen.Blit(npc.GetFrame(), new Point(avatarX, avatarY));

                    var npcname = Content.gameFont.Render(npc.Id.ToString() + ":" + npc.Wait.ToString(), Color.White);
                    screen.Blit(npcname, new Point(avatarX,avatarY - 12));
                    npcname.Dispose();

                    var targetLocation = Content.gameFont.Render("Target: " + npc.TargetX + "," + npc.TargetY, Color.White);
                    screen.Blit(targetLocation, new Point(avatarX, avatarY + 48));
                    targetLocation.Dispose();

                    if(Camera.Within(npc.TargetX, npc.TargetY))
                    {
                        screen.Blit(this.Cursors["select"], new Point(
                            ((npc.TargetX - Camera.X) * Settings.tileWidth) - Avatar.DrawX,
                            ((npc.TargetY - Camera.Y) * Settings.tileHeight) - Avatar.DrawY
                            )); 
                    }

                    //screen.Blit(this.Cursors["target"], new Point(((Camera.X - npc.TargetX) * Settings.tileWidth) - Avatar.DrawX, ((Camera.Y - npc.TargetY) * Settings.tileHeight) - Avatar.DrawY));
                }
            }

            #endregion

            // avatar 
            #region AvatarDraw

            // character 
            int screenX = ((Avatar.X - Camera.X) * Settings.tileWidth);// + Avatar.DrawX;
            int screenY = ((Avatar.Y - Camera.Y - 1) * Settings.tileHeight);// + Avatar.DrawY; 
            screen.Blit(Avatar.GetFrame(), new Point(screenX, screenY));

            // name
            var name = Content.gameFont.Render(Avatar.CharacterName, Color.White);
            screen.Blit(name, new Point(screenX, screenY - 12));
            name.Dispose();

            if(Settings.displayAvatarLocation)
            {
                if(Avatar.Points != null)
                {
                    name = Content.gameFont.Render("Tiles left: " + (Avatar.Points.Count() - 1), Color.White);
                    screen.Blit(name, new Point(screenX, screenY + 50));
                    name.Dispose();
                }
            }
            #endregion

            #region FX
            // environmental / time of day effects
            // stuff like daylight and fot 
            if (Map.DarkMap)
            {
                screen.Blit(Content.NightlyOverlay, new Point(0, 0));
            }

            // particles 
            int count = Emitters.Count;
            for(int i = 0; i < count && i > -1; i++)
            {

                if(!Emitters[i].Update())
                {
                    Emitters.RemoveAt(i);
                    i--;
                    count = Emitters.Count;
                }
                else
                {
                    Emitters[i].Draw(screen);
                }
            }
            #endregion

            #region PlayerPanel

            // top right corner stats? 
            //int statsX = Camera.Width / 2 * Settings.tileWidth;
            //int statsY = Settings.tileHeight;

            //screen.Draw(new Box(new Point(statsX, statsY), new Size(((Camera.Width / 2) - 1) * Settings.tileWidth, Settings.tileHeight * 4)), Color.Red);

            int panelX = 28 * Settings.tileWidth;//(Camera.Width + 1) * Settings.tileWidth;
            int panelWidth = 12 * Settings.tileWidth; // screen.Width - panelX;
            int panelY = 0;
            int panelHeight = screen.Height;

            screen.Fill(new Rectangle(panelX, panelY, panelWidth, panelHeight), Color.Black);


            //for(int i = 0; i < panelHeight + 1; i += Content.PanelTop.Height)
            //{
            //    screen.Blit(Content.PanelTop, new Point(panelX, i - 32));
            //}
            // Draw gui backgrounds 
            

            // change background of panel if you want ?
            //screen.Fill(new Rectangle(panelX, panelY, panelWidth, panelHeight), Color.SandyBrown);

            // Display Players name 
            var playerName = Content.statsFont.Render(Avatar.CharacterName, Color.White);
            screen.Blit(playerName, new Point(panelX + 32, 16));
            playerName.Dispose();

            // Health Bar --------------------------------------- CLEAN UP PLEASE 
            screen.Fill(new Rectangle(panelX + 32, 64, panelWidth - (2 * Settings.tileWidth), Settings.tileHeight / 2), Color.Red);
            double hpWidthIndex = ((panelWidth - (2 * Settings.tileWidth)) / 100.0);
            double health = Avatar.GetHPPercentage();
            double hWidth = hpWidthIndex * health;
            screen.Fill(new Rectangle(panelX + 32, 64, (int)hWidth, Settings.tileHeight / 2), Color.Green);

            var health_text = Content.debugFont.Render("Health: " + Avatar.GetHP() + "/" + Avatar.GetTotalHP(), Color.White);
            screen.Blit(health_text, new Point(panelX + 32, 48));
            health_text.Dispose();

            // staminal bar ---------------------------------- clean up please 
            var stamina_text = Content.debugFont.Render("Stamina: " + (int)Avatar.GetStaminaPercentage() + "%", Color.White);
            screen.Blit(stamina_text, new Point(panelX + 32, 80));
            stamina_text.Dispose();

            // actual bar 
            screen.Fill(new Rectangle(panelX + 32, 96, panelWidth - (2 * Settings.tileWidth), Settings.tileHeight / 2), Color.Red);

            // foreground 
            hWidth = hpWidthIndex * Avatar.GetStaminaPercentage();
            screen.Fill(new Rectangle(panelX + 32, 96, (int)hWidth, Settings.tileHeight / 2), Color.Blue);

            // tab options 
            // draw each button on the screen 
            foreach(var pButton in PanelButtons)
            {
                pButton.DrawButton(screen); 

                // draw panel if needed 
                if(pButton.Selected)
                {
                    var PanelSurface = new Surface(panelWidth - (2 * Settings.tileWidth), 12 * Settings.tileHeight);

                    pButton.DrawPanel(PanelSurface);
                    PanelSurface.Update(); 

                    screen.Blit(PanelSurface, new Point(panelX + 32, 5 * Settings.tileHeight)); 
                }
            }

            // draw selected button panel under buttons 

            //var tab_text = Content.debugFont.Render("Stats", Color.White);
            //screen.Blit(tab_text, new Point(panelX + 32, 96 + 40));
            //tab_text.Dispose();

            //tab_text = Content.debugFont.Render("Inventory", Color.White);
            //screen.Blit(tab_text, new Point(panelX + 32 * 3, 96 + 40));
            //tab_text.Dispose();

            //tab_text = Content.debugFont.Render("Equiped", Color.White);
            //screen.Blit(tab_text, new Point(panelX + 32 * 6, 96 + 40));
            //tab_text.Dispose();

            //tab_text = Content.debugFont.Render("Spells", Color.White);
            //screen.Blit(tab_text, new Point(panelX + 32 * 9, 96 + 40));
            //tab_text.Dispose();
            #endregion

            #region Debug
            if (Settings.displayCameraLocation)
            {
                var cLoc = Content.debugFont.Render("Camera Location: " + Camera.X + ", " + Camera.Y, Color.Yellow);
                screen.Blit(cLoc, new Point(10, 22));
                cLoc.Dispose();
            }

            if(Settings.displayAvatarLocation)
            {
                var aLoc = Content.debugFont.Render("Avatar Location: " + Avatar.X + ", " + Avatar.Y, Color.Yellow);
                screen.Blit(aLoc, new Point(10, 34));
                aLoc.Dispose();
                aLoc = Content.debugFont.Render("Avatar draw: " + Avatar.DrawX + ", " + Avatar.DrawY, Color.Yellow);
                screen.Blit(aLoc, new Point(10, 46));
                aLoc.Dispose();
            }
            if(Settings.displayMouseLocation)
            {
                int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
                int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;

                var mLoc = Content.debugFont.Render("Mouse Location: " + realMouseX + ", " + realMouseY, Color.Yellow);
                screen.Blit(mLoc, new Point(10, 58));
                mLoc.Dispose(); 
            }

            if(Settings.highlightMouseLocation)
            {
                if (Mouse.MousePosition.X < screen.Width - panelWidth)
                {
                    screen.Blit(this.Cursors[displayCursor], new Point(((Mouse.MousePosition.X / Settings.tileWidth) * Settings.tileWidth) - Avatar.DrawX, ((Mouse.MousePosition.Y / Settings.tileHeight) * Settings.tileHeight) - Avatar.DrawY));
                }//screen.Draw(new Box(new Point((Mouse.MousePosition.X / Settings.tileWidth) * Settings.tileWidth, (Mouse.MousePosition.Y / Settings.tileHeight) * Settings.tileHeight), new Size(Settings.tileWidth, Settings.tileHeight)), Color.Purple);
            }

            // if(Settings.displayAvatarLocation)
            {
                int tarX = (Avatar.TargetX - Camera.X) * Settings.tileWidth;
                int tarY = (Avatar.TargetY - Camera.Y) * Settings.tileHeight;

                // screen.Draw(new Box(new Point(tarX, tarY), new Size(Settings.tileWidth, Settings.tileHeight)), Color.FromArgb(128, Color.Red));
                screen.Blit(this.Cursors["target"], new Point(tarX - Avatar.DrawX, tarY - Avatar.DrawY)); 
                //screen.Fill(new Rectangle(tarX + 1, tarY + 1, Settings.tileWidth - 2, Settings.tileHeight - 2), Color.FromArgb(50, Color.Red));
            }

            #endregion

        }

        public void DoEvents()
        {
            TickCount++;


            playerMovement();
            npcMovement();
            actionHandler();
            TriggerHandler(Avatar.X, Avatar.Y);

            // editor features 
            if (Settings.Editor.Visible)
            {
                mapScrolling();
            }


            // reset tick count 
            if (TickCount == 60)
            {
                TickCount = 0;
            }
        }

        private void mapScrolling()
        {
            if (TickCount % 5 == 0)
            {
                if (currentKey == Key.LeftArrow || currentKey == Key.A)
                    Camera.X--;
                else if (currentKey == Key.RightArrow || currentKey == Key.D)
                    Camera.X++;
                else if (currentKey == Key.UpArrow || currentKey == Key.W)
                    Camera.Y--;
                else if (currentKey == Key.DownArrow || currentKey == Key.S)
                    Camera.Y++;
            }
        }

        private void playerMovement()
        {
            int avatarmovementTicks = 7;

            if (Avatar.Run)
                avatarmovementTicks = 4;

            // regain stamina 
            if (Avatar.X == Avatar.TargetX && Avatar.Y == Avatar.Y || !Avatar.Run)
            {
                if (Avatar.CurrentStamina < Avatar.Stamina && !Avatar.Combat && TickCount % 30 == 0)
                {
                    Avatar.CurrentStamina += .25;
                }
            }

            if (TickCount % avatarmovementTicks == 0) // 60 / 4 = 15; 4 frames = 1 tile move for the player sprite 
            {
                if (Avatar.X == Avatar.TargetX && Avatar.Y == Avatar.TargetY)
                {
                    Avatar.CurrentFrame = 2; 
                    return;
                }
                else if (Avatar.X == Avatar.nextX && Avatar.Y == Avatar.nextY && Avatar.Points.Count > 0)
                {
                    //// get next point in path and update. 
                    //bool[,] map = new bool[Camera.Width, Camera.Height];
                    //Sprite.PathFinder p = new Sprite.PathFinder(new Sprite.PathFinder.SearchParameters(new Point(Avatar.X, Avatar.Y),
                    //    new Point(Avatar.TargetX, Avatar.TargetY),
                    //    map));

                    //Avatar.Points = p.FindPath();

                    Avatar.Points.RemoveAt(0);

                    if (Avatar.Points.Count > 0)
                    {
                        Avatar.nextX = Avatar.Points[0].X;
                        Avatar.nextY = Avatar.Points[0].Y;
                    }
                }


                if (Avatar.nextX > Avatar.X)
                {
                    Avatar.CurrentDirection = Sprite.Direction.Right;
                    Avatar.DrawX += Avatar.MoveSpeed;

                    //Avatar.DrawY = 0;
                    Avatar.CurrentFrame++;

                    if (Avatar.CurrentFrame == 4) // reset frame when we get to end of animation 
                        Avatar.CurrentFrame = 0;

                    if (Avatar.DrawX >= 32) // move tile 
                    {
                        Avatar.DrawX = 0;
                        Avatar.X++;
                        // run stamina 
                        if (Avatar.Run)
                        {
                            Avatar.CurrentStamina -= .25;
                        }

                        //if (Avatar.X == Camera.X + Camera.Width - Camera.xScroll) // move camera at end 
                        {
                            Camera.X += Camera.ScrollSpeed;
                        }
                    }
                }
                else if (Avatar.nextX < Avatar.X)
                {
                    Avatar.CurrentDirection = Sprite.Direction.Left;
                    Avatar.DrawX -= Avatar.MoveSpeed;
                    //Avatar.DrawY = 0;

                    Avatar.CurrentFrame++;
                    if (Avatar.CurrentFrame == 4)
                        Avatar.CurrentFrame = 0;

                    if (Avatar.DrawX <= -32)
                    {
                        Avatar.DrawX = 0;
                        Avatar.X--;
                        // run stamina 
                        if (Avatar.Run)
                        {
                            Avatar.CurrentStamina -= .25;
                        }

                        //if (Avatar.X == Camera.X + Camera.xScroll)
                        {
                            Camera.X -= Camera.ScrollSpeed;
                        }
                    }
                }
                else if (Avatar.nextY > Avatar.Y)
                {
                    Avatar.CurrentDirection = Sprite.Direction.Down;
                    Avatar.DrawY += Avatar.MoveSpeed;
                    //Avatar.DrawX = 0;

                    Avatar.CurrentFrame++;
                    if (Avatar.CurrentFrame == 4)
                        Avatar.CurrentFrame = 0;

                    if (Avatar.DrawY >= 32)
                    {
                        Avatar.DrawY = 0;
                        Avatar.Y++;
                        // run stamina 
                        if (Avatar.Run)
                        {
                            Avatar.CurrentStamina -= .25;
                        }

                        //if (Avatar.Y == Camera.Y + Camera.Height - Camera.yScroll)
                        {
                            Camera.Y += Camera.ScrollSpeed;
                        }
                    }
                }
                else if (Avatar.nextY < Avatar.Y)
                {
                    Avatar.CurrentDirection = Sprite.Direction.Up;
                    Avatar.DrawY -= Avatar.MoveSpeed;
                    //Avatar.DrawX = 0;

                    Avatar.CurrentFrame++;
                    if (Avatar.CurrentFrame == 4)
                        Avatar.CurrentFrame = 0;

                    if (Avatar.DrawY <= -32)
                    {
                        Avatar.DrawY = 0;
                        Avatar.Y--;

                        // run stamina 
                        if (Avatar.Run)
                        {
                            Avatar.CurrentStamina -= .25;
                        }


                        //if (Avatar.Y == Camera.Y + Camera.yScroll)
                        {
                            Camera.Y -= Camera.ScrollSpeed;
                        }
                    }
                }
                else
                {
                    Avatar.CurrentFrame = 2;
                }

                // disable run on no stamina 
                if(Avatar.CurrentStamina == 0)
                {
                    Avatar.Run = false;
                }

            }
        }

        Random r = new Random(9999);
        private void npcMovement()
        {
            #region NPC_Movement
            
            if (TickCount % 7 == 0) // npc can only walk 
            {
                // reuses for all npc 
                var blocks = Map.getBlocks(); 

                foreach (Sprite.NPC npc in this.NPC.Values)
                {
                    if (!npc.CanMove)
                        break;

                    if (npc.Wait)
                    {
                        // do nothing ? 
                        npc.CurrentFrame = 2;
                    }
                    else
                    {
                        if (npc.TargetX == npc.X && npc.TargetY == npc.Y)
                        {
                            // set next pos
                            //while (npc.X == npc.TargetX && npc.Y == npc.TargetY)
                            //{
                            //    npc.TargetX = r.Next(npc.OriginalX - 10, npc.OriginalX + 10);
                            //    npc.TargetY = r.Next(npc.OriginalY - 10, npc.OriginalY + 10);

                            //    while (npc.TargetX >= Map.Width)
                            //        npc.TargetX--;
                            //    while (npc.TargetX < 0)
                            //        npc.TargetX++;
                            //    while (npc.TargetY >= Map.Height)
                            //        npc.TargetY--;
                            //    while (npc.TargetY < 0)
                            //        npc.TargetY++;
                            //}

                            // set delay before next movement 
                            npc.WaitMovement(r.Next(2500, 10000));


                            //bool[,] map = new bool[Map.Width, Map.Height];

                            //for (int x = 0; x < Map.Width; x++)
                            //{
                            //    for (int y = 0; y < Map.Height; y++)
                            //    {
                            //        if (x > 0 && x < Map.Width && y > 0 && y < Map.Width)
                            //        {
                            //            map[x, y] = !Map.Tiles[x, y].Blocked;
                            //        }
                            //        else
                            //        {
                            //            map[x, y] = false;
                            //        }
                            //    }
                            //}


                            //     Sprite.PathFinder p = new Sprite.PathFinder(new Sprite.PathFinder.SearchParameters(new Point(npc.X, npc.Y),
                            //new Point(npc.TargetX, npc.TargetY),
                            //map));

                            //var finder = new Sprite.PathFinderv2(Map.getBlocks());
                            //var points = finder.FindPath(new Point(npc.X, npc.Y),
                            //    new Point(npc.TargetX, npc.TargetY));
                            var finder = new Sprite.PathFinderv2(blocks);
                            var path = new List<Sprite.PathFinderNode>(); // finder.FindPath(new Point(npc.X, npc.Y), new Point(npc.TargetX, npc.TargetY));

                            npc.TargetX = npc.OriginalX + r.Next(npc.MoveRadius * -1, npc.MoveRadius);
                            npc.TargetY = npc.OriginalY + r.Next(npc.MoveRadius * -1, npc.MoveRadius);

                            path = finder.FindPath(new Point(npc.X, npc.Y), new Point(npc.TargetX, npc.TargetY));
                            int fix = 0; 

                            while ((path == null || path.Count == 0) || npc.TargetX == npc.X)
                            {
                                fix++;

                                npc.TargetX = npc.OriginalX + r.Next(npc.MoveRadius * -1, npc.MoveRadius) + r.Next(fix * -1, fix);
                                npc.TargetY = npc.OriginalY + r.Next(npc.MoveRadius * -1, npc.MoveRadius) + r.Next(fix * -1, fix);
                                
                                path = finder.FindPath(new Point(npc.X, npc.Y), new Point(npc.TargetX, npc.TargetY));
                            }

                            // path found 
                            if (path != null && path.Count() > 0)
                            {
                                npc.Points = new List<Point>();
                                for (int i = 0; i < path.Count(); i++)
                                {

                                    npc.Points.Add(new Point(path[i].X, path[i].Y));
                                }

                                npc.nextX = npc.Points[0].X;
                                npc.nextY = npc.Points[0].Y;
                            }
                            else
                            {
                                npc.TargetX = npc.X;
                                npc.TargetY = npc.Y;
                            }


                            //if (points != null)
                            //{
                            //    npc.Points = new List<Point>();
                            //    for(int i = 0; i < points.Count; i++)
                            //    {
                            //        npc.Points.Add(new Point(points[i].X, points[i].Y)); 
                            //    }
                            //}

                            ////npc.Points = p.FindPath(); 

                            //if(npc.Points != null && npc.Points.Count > 0)
                            //{
                            //    npc.nextX = npc.Points[0].X;
                            //    npc.nextY = npc.Points[0].Y; 
                            //}
                        }

                        if (npc.Wait)
                            break; 

                        // movement 
                        if (npc.X == npc.TargetX && npc.Y == npc.TargetY)
                        {
                            npc.CurrentFrame = 2;
                            //return;
                        }
                        else if (npc.Points != null && npc.X == npc.nextX && npc.Y == npc.nextY && npc.Points.Count > 0)
                        {

                            npc.Points.RemoveAt(0);

                            if (npc.Points.Count > 0)
                            {
                                npc.nextX = npc.Points[0].X;
                                npc.nextY = npc.Points[0].Y;
                            }
                        }


                        if (npc.nextX > npc.X)
                        {
                            npc.CurrentDirection = Sprite.Direction.Right;
                            npc.DrawX += npc.MoveSpeed;

                            //npc.DrawY = 0;
                            npc.CurrentFrame++;

                            if (npc.CurrentFrame == 4) // reset frame when we get to end of animation 
                                npc.CurrentFrame = 0;

                            if (npc.DrawX >= 32) // move tile 
                            {
                                npc.DrawX = 0;
                                npc.X++;
                            }
                        }
                        else if (npc.nextX < npc.X)
                        {
                            npc.CurrentDirection = Sprite.Direction.Left;
                            npc.DrawX -= npc.MoveSpeed;
                            //npc.DrawY = 0;

                            npc.CurrentFrame++;
                            if (npc.CurrentFrame == 4)
                                npc.CurrentFrame = 0;

                            if (npc.DrawX <= -32)
                            {
                                npc.DrawX = 0;
                                npc.X--;

                            }
                        }
                        else if (npc.nextY > npc.Y)
                        {
                            npc.CurrentDirection = Sprite.Direction.Down;
                            npc.DrawY += npc.MoveSpeed;
                            //npc.DrawX = 0;

                            npc.CurrentFrame++;
                            if (npc.CurrentFrame == 4)
                                npc.CurrentFrame = 0;

                            if (npc.DrawY >= 32)
                            {
                                npc.DrawY = 0;
                                npc.Y++;
                            }
                        }
                        else if (npc.nextY < npc.Y)
                        {
                            npc.CurrentDirection = Sprite.Direction.Up;
                            npc.DrawY -= npc.MoveSpeed;
                            //npc.DrawX = 0;

                            npc.CurrentFrame++;
                            if (npc.CurrentFrame == 4)
                                npc.CurrentFrame = 0;

                            if (npc.DrawY <= -32)
                            {
                                npc.DrawY = 0;
                                npc.Y--;
                            }
                        }
                        else
                        {
                            //npc.CurrentFrame = 2;
                        }
                    }
                }
            }
            #endregion
        }

        private void actionHandler()
        {
            int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
            int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;
            #region MouseHighlights

            string cursor = "default";
            foreach (Sprite.NPC npc in NPC.Values)
            {
                if (npc.X == realMouseX)
                {
                    if (npc.Y == realMouseY || realMouseY + 1 == npc.Y)
                        cursor = "select";
                }
            }

            if(cursor == "default")
            {
                // check for items 
                //if(Map.GetTile(realMouseX, realMouseY).la)
            }

            displayCursor = cursor;
            #endregion
        }

        private void TriggerHandler(int x, int y)
        {
            if(Map.Tiles[x, y].TriggerId != -1)
            {
                var trigger = GameTriggers[Map.Tiles[x, y].TriggerId];

                // handle the trigger via the script handler
                handleTriggerScript(trigger.Data); 
            }
        }

        #region TriggerScripts
        public void updateCamera()
        {
            Camera.X = (Avatar.X - Camera.Width / 2) + 6;
            Camera.Y = Avatar.Y - Camera.Height / 2;
        }
        private void handleTriggerScript(string script)
        {

            Scripting.Executor e = new Scripting.Executor(this);
            e.runScript(script);
        }

        public void saveTriggerScripts()
        {
            for(int i = 0; i < GameTriggers.Count; i++)
            {
                try
                {
                    using (System.IO.BinaryWriter sw = new System.IO.BinaryWriter(System.IO.File.Open(@"Data\scripts\triggers\" + i + ".trig", System.IO.FileMode.OpenOrCreate)))
                    {
                        sw.Write(GameTriggers[i].Id);
                        sw.Write(GameTriggers[i].Name);
                        sw.Write(GameTriggers[i].Data);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        public void LoadTriggerScripts()
        {
            for (int i = 0; i < System.IO.Directory.GetFiles(@"Data\scripts\triggers\").Count(); i++)
            {
                using (System.IO.BinaryReader sr = new System.IO.BinaryReader(System.IO.File.Open(@"Data\scripts\triggers\" + i + ".trig", System.IO.FileMode.Open)))
                {
                    var t = new World.Triggers.Trigger();
                    t.Id = sr.ReadInt32();
                    t.Name = sr.ReadString();
                    t.Data = sr.ReadString();
                    GameTriggers.Add(t);
                }
            }

        }
        #endregion

        public void Keyboard_down(KeyboardEventArgs e)
        {
            if (Settings.Editor.Visible)
            {
                //if (e.Key == Key.LeftArrow || e.Key == Key.A)
                //    Camera.X--;
                //else if (e.Key == Key.RightArrow || e.Key == Key.D)
                //    Camera.X++;
                //else if (e.Key == Key.UpArrow || e.Key == Key.W)
                //    Camera.Y--;
                //else if (e.Key == Key.DownArrow || e.Key == Key.S )
                //    Camera.Y++;
                currentKey = e.Key;
            }
            else
            {
                //throw new NotImplementedException();
                if (e.Key == Key.LeftArrow || e.Key == Key.A)
                    // this.Avatar.X--;
                    this.Avatar.CurrentDirection = Sprite.Direction.Left;
                else if (e.Key == Key.RightArrow || e.Key == Key.D)
                    // this.Avatar.X++;
                    this.Avatar.CurrentDirection = Sprite.Direction.Right;
                else if (e.Key == Key.UpArrow || e.Key == Key.W)
                    // this.Avatar.Y--;
                    this.Avatar.CurrentDirection = Sprite.Direction.Up;
                else if (e.Key == Key.DownArrow || e.Key == Key.S)
                    //this.Avatar.Y++;
                    this.Avatar.CurrentDirection = Sprite.Direction.Down;
                else if (e.Key == Key.F1)
                {
                    Settings.Editor.Show();
                }
                else if (e.Key == Key.Space)
                {
                    Avatar.Run = !Avatar.Run;
                }
                else if(e.Key == Key.Escape)
                {
                    Settings.displayAvatarLocation = !Settings.displayAvatarLocation;
                    Settings.displayAvatarPath = !Settings.displayAvatarPath;
                    Settings.displayFPS = !Settings.displayFPS;
                    Settings.displayCameraLocation = !Settings.displayCameraLocation;
                    Settings.displayMouseLocation = !Settings.displayMouseLocation;
                }
                else if(e.Key == Key.KeypadMinus)
                {
                    Avatar.TakeDammage(1);

                    // REMOVE
                    Emitters.Add(new Particles.Emitters.Blood(((Avatar.X - Camera.X) * Settings.tileWidth) + 16, ((Avatar.Y - Camera.Y) * Settings.tileHeight) - 8) { Color = Color.Red });

                }
                else if (e.Key == Key.KeypadPlus)
                {
                    Avatar.Skills["hp"].Value++;
                }
                else if (e.Key == Key.KeypadMultiply)
                {
                    Map.DarkMap = !Map.DarkMap;
                }
                else if(e.Key == Key.KeypadPeriod)
                {
                    // REMOVE
                    Emitters.Add(new Particles.Emitters.Firework(((Avatar.X - Camera.X) * Settings.tileWidth) + 16, ((Avatar.Y - Camera.Y) * Settings.tileHeight)) { Color = Color.Red });

                }
            }
        }

        public void Keyboard_up(KeyboardEventArgs e)
        {
            currentKey = Key.Unknown;
            //throw new NotImplementedException();
        }

        public void MouseButton_down(MouseButtonEventArgs e)
        {
            int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
            int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;


            // side panel managment 
            if (Mouse.MousePosition.X > Settings.Width - ((Settings.PanelWidth - 1) * Settings.tileWidth))
            {
                int selectedIdx = -1; 

                for(int i = 0; i < PanelButtons.Count; i++)
                {
                    if (PanelButtons[i].IntersectsWith(e.X, e.Y))
                        selectedIdx = i;
                }

                if(selectedIdx != -1)
                {
                    for(int i = 0; i < PanelButtons.Count; i++)
                    {
                        PanelButtons[i].Selected = false;
                    }
                    PanelButtons[selectedIdx].Selected = true;
                }
                else
                {
                    // no panel is being chosen, so this is probably panel input 
                    foreach(GamePanel.PanelButton pb in PanelButtons)
                    {
                        if(pb.Selected)
                        {
                            pb.MouseDown(e);
                        }
                    }
                }


                // exit when done with side panel, other stuff is for other parts of game that should not be touched right now... 
                return; 
            }


            // editor movement 
            if (Settings.Editor.Visible)
            {
                if (Settings.Editor.radBlocks.Checked)
                {
                    // primary to block, secondary to unblock 
                    Map.Tiles[realMouseX, realMouseY].Blocked = e.Button == MouseButton.PrimaryButton;
                }
                else if (Settings.Editor.radSpawn.Checked)
                {
                    // set map span location
                    Map.SpawnX = realMouseX;
                    Map.SpawnY = realMouseY;
                }
                else if (Settings.Editor.TriggerEditor)
                {
                    if (e.Button == MouseButton.PrimaryButton)
                    {
                        Map.Tiles[realMouseX, realMouseY].TriggerId = Settings.Editor.lstTriggers.SelectedIndex;
                    }
                    else
                    {
                        Map.Tiles[realMouseX, realMouseY].TriggerId = -1; 
                    }
                }
                else
                {
                    // tile editing 
                    if (e.Button == MouseButton.PrimaryButton)
                    {
                        if (Settings.Editor.MultiSelected != null)
                        {
                            // Map.Tiles[realMouseX, realMouseY].Layers[Settings.Editor.SelectedLayer] = Settings.Editor.SelectedTile;
                            for (int x = 0; x < Settings.Editor.MultiSelected.GetLength(0); x++)
                            {
                                for (int y = 0; y < Settings.Editor.MultiSelected.GetLength(1); y++)
                                {
                                    Map.Tiles[realMouseX + x, realMouseY + y].Layers[Settings.Editor.SelectedLayer] = Settings.Editor.MultiSelected[x, y];
                                }
                            }
                        }
                    }
                    else
                    {
                        Map.Tiles[realMouseX, realMouseY].Layers[Settings.Editor.SelectedLayer] = 0;
                    }
                }
            }
            else
            {
                // normal game movement 
                

                //throw new NotImplementedException();
                if (e.Button == MouseButton.PrimaryButton)
                {
                    // Avatar.TargetX = realMouseX;
                    // Avatar.TargetY = realMouseY;

                    //bool[,] map = new bool[Camera.Width, Camera.Height];

                    //for(int x = Camera.X; x < Camera.Width; x++)
                    //{
                    //    for(int y = Camera.Y; y < Camera.Height; y++)
                    //    {
                    //        if (x > 0 && x < Map.Width && y > 0 && y < Map.Width)
                    //        {
                    //            map[x, y] = !Map.Tiles[x, y].Blocked;
                    //        }
                    //        else
                    //        {
                    //            //map[x, y] = false;
                    //        }
                    //    }
                    //}

                    //bool[,] map = new bool[Map.Width, Map.Height];

                    //for (int x = 0; x < Map.Width; x++)
                    //{
                    //    for (int y = 0; y < Map.Height; y++)
                    //    {
                    //        if (x > 0 && x < Map.Width && y > 0 && y < Map.Width)
                    //        {
                    //            map[x, y] = !Map.Tiles[x, y].Blocked;
                    //        }
                    //        else
                    //        {
                    //            map[x, y] = false;
                    //        }
                    //    }
                    //}

                    //Sprite.PathFinder p = new Sprite.PathFinder(new Sprite.PathFinder.SearchParameters(new Point(Avatar.X, Avatar.Y),
                    //    new Point(realMouseX, realMouseY),
                    //    map));

                    //Avatar.Points = p.FindPath();

                    //if(Avatar.Points.Count > 0)
                    //{
                    //    Avatar.nextX = Avatar.Points[0].X;
                    //    Avatar.nextY = Avatar.Points[0].Y;
                    //    Avatar.TargetX = realMouseX;
                    //    Avatar.TargetY = realMouseY;
                    //} 

                    
                    var finder = new Sprite.PathFinderv2(Map.getBlocks());
                    var path = finder.FindPath(new Point(Avatar.X, Avatar.Y), new Point(realMouseX, realMouseY));

                    // path found 
                    if(path != null && path.Count() > 0)
                    {
                        Avatar.TargetX = realMouseX;
                        Avatar.TargetY = realMouseY;
                        Avatar.Points = new List<Point>();
                        for(int i = 0; i < path.Count(); i++)
                        {
                            
                            Avatar.Points.Add(new Point(path[i].X, path[i].Y)); 
                        }

                        Avatar.nextX = Avatar.Points[0].X;
                        Avatar.nextY = Avatar.Points[0].Y;
                    }

                }
            }
        }

        public void MouseButton_up(MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();

            // side panel managment 
            if (Mouse.MousePosition.X > Settings.Width - ((Settings.PanelWidth - 1) * Settings.tileWidth))
            {
                // no panel is being chosen, so this is probably panel input 
                foreach (GamePanel.PanelButton pb in PanelButtons)
                {
                    if (pb.Selected)
                    {
                        pb.MouseUp(e);
                    }
                }
            }
        }

        public void MouseMotion(MouseMotionEventArgs e)
        {
            
            int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
            int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;

            if(Mouse.MousePosition.X > Settings.Width - (Settings.PanelWidth * Settings.tileWidth))
            {
                // no panel is being chosen, so this is probably panel input 
                foreach (GamePanel.PanelButton pb in PanelButtons)
                {
                    if (pb.Selected)
                    {
                        pb.MouseMove(e);
                    }
                }
                return; 
            }

            if (Settings.Editor.Visible)
            {
                if (Settings.Editor.radBlocks.Checked)
                {
                    // primary to block, secondary to unblock 
                    if(e.Button == MouseButton.PrimaryButton)
                    {
                        Map.Tiles[realMouseX, realMouseY].Blocked = true;
                    }
                    else if(e.Button == MouseButton.SecondaryButton)
                    {
                        Map.Tiles[realMouseX, realMouseY].Blocked = false;
                    }
                }
                else if (Settings.Editor.radSpawn.Checked)
                {
                    // place holder? 
                }
                else if (Settings.Editor.TriggerEditor)
                {
                    if (e.Button == MouseButton.PrimaryButton)
                    {
                        Map.Tiles[realMouseX, realMouseY].TriggerId = Settings.Editor.lstTriggers.SelectedIndex;
                    }
                    else if(e.Button == MouseButton.SecondaryButton)
                    {
                        Map.Tiles[realMouseX, realMouseY].TriggerId = -1;
                    }
                }
                else
                {
                    // simple tile editing 
                    if (e.Button == MouseButton.PrimaryButton)
                    {
                        Map.Tiles[realMouseX, realMouseY].Layers[Settings.Editor.SelectedLayer] = Settings.Editor.SelectedTile;
                    }
                    else if (e.Button == MouseButton.SecondaryButton)
                    {
                        Map.Tiles[realMouseX, realMouseY].Layers[Settings.Editor.SelectedLayer] = 0;
                    }
                }
            }
        }
    }
}
