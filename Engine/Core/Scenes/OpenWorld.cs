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

        public Sprite.Avatar Avatar { get; set; }

        // tick counters for movement and events 
        public int TickCount = 0;

        public OpenWorld()
        {
            // setup camera 
            this.Camera = new World.Camera(50, 50);
            Camera.X = Camera.X - (Camera.Width / 2);
            Camera.Y = Camera.Y - (Camera.Height / 2); 
            this.Avatar = new Sprite.Avatar(50, 50); 

            // setup game map
            this.Map = new World.Map.WorldMap();
            this.Map.SeetMap(0, 9); // grass everywhere 
            for(int x = 0; x < this.Map.Width; x += 3)
            {
                for(int y = 0; y < this.Map.Height; y+= 2)
                {
                    
                    {
                        this.Map.Tiles[x, y].SetLayer(1, 10);
                    }
                }
            }

            Settings.Editor = new GUI.MapEditor(this);
        }

        public void Draw(Surface screen)
        {
            // do game events before draw 
            DoEvents(); 

            int realX = 0, realY = 0;
            var blocked = Content.gameFont.Render("B", Color.Purple); 

            for(int x = 0; x < Camera.Width + 1; x++)
            {
                for(int y = 0; y < Camera.Height; y++)
                {
                    // calculate real locations
                    realX = x + Camera.X;
                    realY = y + Camera.Y;

                    // loop through tiles here 
                    var tile = Map.GetTile(realX, realY);
                    if (tile != null)
                    {
                        Point tileLocation = new Point(x * Settings.tileWidth, y * Settings.tileHeight);
                        for (int z = 0; z < tile.Layers.Length; z++)
                        {
                            // draw tile + layers 
                            if (tile.GetLayer(z) > 0)
                            {
                                screen.Blit(Map.Surfaces[tile.GetLayer(z)], tileLocation);
                            }
                        }

                        if (Settings.Editor.Visible && Settings.Editor.cbBlock.Checked && tile.Blocked)
                        {
                            screen.Blit(blocked, new Point(x * Settings.tileWidth + 12, y * Settings.tileHeight + 10));
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

            // avatar 
            #region AvatarDraw

            // character 
            int screenX = ((Avatar.X - Camera.X) * Settings.tileWidth) + Avatar.DrawX;
            int screenY = ((Avatar.Y - Camera.Y - 1) * Settings.tileHeight) + Avatar.DrawY; 
            screen.Blit(Avatar.GetFrame(), new Point(screenX, screenY));

            // name
            var name = Content.gameFont.Render(Avatar.CharacterName, Color.White);
            screen.Blit(name, new Point(screenX, screenY - 12));
            name.Dispose();
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
            }
            if(Settings.displayMouseLocation)
            {
                int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
                int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;
                var mLoc = Content.debugFont.Render("Mouse Location: " + realMouseX + ", " + realMouseY, Color.Yellow);
                screen.Blit(mLoc, new Point(10, 46));
                mLoc.Dispose(); 
            }

            if(Settings.highlightMouseLocation)
            {
                screen.Draw(new Box(new Point((Mouse.MousePosition.X / Settings.tileWidth) * Settings.tileWidth, (Mouse.MousePosition.Y / Settings.tileHeight) * Settings.tileHeight), new Size(Settings.tileWidth, Settings.tileHeight)), Color.Purple);
            }

            if(Settings.displayAvatarLocation)
            {
                int tarX = (Avatar.TargetX - Camera.X) * Settings.tileWidth;
                int tarY = (Avatar.TargetY - Camera.Y) * Settings.tileHeight;

                screen.Draw(new Box(new Point(tarX, tarY), new Size(Settings.tileWidth, Settings.tileHeight)), Color.Red);
            }

            #endregion
        }

        public void DoEvents()
        {
            TickCount++; 
            if(TickCount % 7 == 0) // 60 / 4 = 15; 4 frames = 1 tile move for the player sprite 
            {

                #region SpriteMovement
                if(Avatar.TargetX > Avatar.X && !Map.Tiles[Avatar.X + 1, Avatar.Y].Blocked) // right movement 
                {
                    if (Avatar.X + 1 != Map.Width)
                    {
                        Avatar.CurrentDirection = Sprite.Direction.Right;
                        Avatar.DrawX += 8; // 1/4 tile at a time 
                        Avatar.DrawY = 0;

                        Avatar.CurrentFrame++; // change frame 

                        if (Avatar.CurrentFrame == 4) // reset frame when we get to end of animation 
                            Avatar.CurrentFrame = 0;

                        if (Avatar.DrawX == 32) // move tile 
                        {
                            Avatar.DrawX = 0;
                            Avatar.X++;

                            if (Avatar.X == Camera.X + Camera.Width - 6) // move camera at end 
                            {
                                Camera.X += Camera.ScrollSpeed;
                            }
                        }
                    }

                    // rest of the movments are the same sequence just different directions
                    // will comment them at a later time... 
                } 
                else if (Avatar.TargetX < Avatar.X && !Map.Tiles[Avatar.X - 1, Avatar.Y].Blocked)
                {
                    if (Avatar.X - 1 != -1)
                    {
                        Avatar.CurrentDirection = Sprite.Direction.Left;
                        Avatar.DrawX -= 8;
                        Avatar.DrawY = 0;

                        Avatar.CurrentFrame++;
                        if (Avatar.CurrentFrame == 4)
                            Avatar.CurrentFrame = 0;

                        if (Avatar.DrawX == -32)
                        {
                            Avatar.DrawX = 0;
                            Avatar.X--;

                            if (Avatar.X == Camera.X + 6)
                            {
                                Camera.X -= Camera.ScrollSpeed;
                            }
                        }
                    }
                }
                else if (Avatar.TargetY > Avatar.Y && !Map.Tiles[Avatar.X, Avatar.Y + 1].Blocked)
                {
                    if (Avatar.Y + 1 != Map.Height)
                    {
                        Avatar.CurrentDirection = Sprite.Direction.Down;
                        Avatar.DrawY += 8;
                        Avatar.DrawX = 0;

                        Avatar.CurrentFrame++;
                        if (Avatar.CurrentFrame == 4)
                            Avatar.CurrentFrame = 0;

                        if (Avatar.DrawY == 32)
                        {
                            Avatar.DrawY = 0;
                            Avatar.Y++;

                            if (Avatar.Y == Camera.Y + Camera.Height - 4)
                            {
                                Camera.Y += Camera.ScrollSpeed;
                            }
                        }
                    }
                }
                else if (Avatar.TargetY < Avatar.Y && !Map.Tiles[Avatar.X, Avatar.Y - 1].Blocked)
                {
                    if (Avatar.Y - 1 != -1)
                    {
                        Avatar.CurrentDirection = Sprite.Direction.Up;
                        Avatar.DrawY -= 8;
                        Avatar.DrawX = 0;

                        Avatar.CurrentFrame++;
                        if (Avatar.CurrentFrame == 4)
                            Avatar.CurrentFrame = 0;

                        if (Avatar.DrawY == -32)
                        {
                            Avatar.DrawY = 0;
                            Avatar.Y--;

                            if (Avatar.Y == Camera.Y + 4)
                            {
                                Camera.Y -= Camera.ScrollSpeed;
                            }
                        }
                    }
                }
                else
                {
                    Avatar.CurrentFrame = 2; 
                }
                #endregion

                // reset tick count 
                if(TickCount == 60)
                {
                    TickCount = 0; 
                }
            }
        }

        public void Keyboard_down(KeyboardEventArgs e)
        {
            if (Settings.Editor.Visible)
            {
                if (e.Key == Key.LeftArrow || e.Key == Key.A)
                    Camera.X--;
                else if (e.Key == Key.RightArrow || e.Key == Key.D)
                    Camera.X++;
                else if (e.Key == Key.UpArrow || e.Key == Key.W)
                    Camera.Y--;
                else if (e.Key == Key.DownArrow || e.Key == Key.S )
                    Camera.Y++;
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
            }
        }

        public void Keyboard_up(KeyboardEventArgs e)
        {

            //throw new NotImplementedException();
        }

        public void MouseButton_down(MouseButtonEventArgs e)
        {
            int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
            int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;
            // editor movement 
            if (Settings.Editor.Visible)
            {
                if (Settings.Editor.cbBlock.Checked)
                {
                    // primary to block, secondary to unblock 
                    Map.Tiles[realMouseX, realMouseY].Blocked = e.Button == MouseButton.PrimaryButton;
                }
                else
                {
                    // tile editing 
                    if (e.Button == MouseButton.PrimaryButton)
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
                    Avatar.TargetX = realMouseX;
                    Avatar.TargetY = realMouseY;
                }
            }
        }

        public void MouseButton_up(MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void MouseMotion(MouseMotionEventArgs e)
        {
            int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
            int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;

            if (Settings.Editor.Visible)
            {
                if (Settings.Editor.cbBlock.Checked)
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
