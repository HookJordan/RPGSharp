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
        }

        public void Draw(Surface screen)
        {
            // do game events before draw 
            DoEvents(); 

            int realX = 0, realY = 0; 

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


            // target location for debugging 
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

            #endregion
        }

        public void DoEvents()
        {
            TickCount++; 
            if(TickCount % 7 == 0) // 60 / 4 = 15; 4 frames = 1 tile move for the player sprite 
            {

                #region SpriteMovement
                if(Avatar.TargetX > Avatar.X) // right movement 
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
                                Camera.X++;
                            }
                        }
                    }

                    // rest of the movments are the same sequence just different directions
                    // will comment them at a later time... 
                } 
                else if (Avatar.TargetX < Avatar.X)
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
                                Camera.X--;
                            }
                        }
                    }
                }
                else if (Avatar.TargetY > Avatar.Y)
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
                                Camera.Y++;
                            }
                        }
                    }
                }
                else if (Avatar.TargetY < Avatar.Y)
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
                                Camera.Y--;
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
            //throw new NotImplementedException();
            if (e.Key == Key.LeftArrow)
                this.Avatar.X--;
            else if (e.Key == Key.RightArrow)
                this.Avatar.X++;
            else if (e.Key == Key.UpArrow)
                this.Avatar.Y--;
            else if (e.Key == Key.DownArrow)
                this.Avatar.Y++;
        }

        public void Keyboard_up(KeyboardEventArgs e)
        {
            int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
            int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;

            //throw new NotImplementedException();
        }

        public void MouseButton_down(MouseButtonEventArgs e)
        {
            int realMouseX = (Mouse.MousePosition.X / Settings.tileWidth) + Camera.X;
            int realMouseY = (Mouse.MousePosition.Y / Settings.tileHeight) + Camera.Y;

            //throw new NotImplementedException();
            if (e.Button == MouseButton.PrimaryButton)
            {
                Avatar.TargetX = realMouseX;
                Avatar.TargetY = realMouseY;
            }
        }

        public void MouseButton_up(MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void MouseMotion(MouseMotionEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
