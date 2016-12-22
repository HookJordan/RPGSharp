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

        public OpenWorld()
        {
            // setup camera 
            this.Camera = new World.Camera(50, 50);
            Camera.X = Camera.X - (Camera.Width / 2);
            Camera.Y = Camera.Y - (Camera.Height / 2); 
            this.Avatar = new Sprite.Avatar(50, 50); 

            // setup game map
            this.Map = new World.Map.WorldMap(); 
        }

        public void Draw(Surface screen)
        {
            int realX = 0, realY = 0; 

            for(int x = 0; x < Camera.Width; x++)
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
                        for (int z = 0; z < tile.Layers.Length; z++)
                        {
                            // draw tile + layers 
                            if (tile.GetLayer(z) > 0)
                            {
                                // draw something 
                            }
                        }
                    }

                    if(realX == Avatar.X && realY == Avatar.Y)
                    {
                        screen.Fill(new Rectangle(x * Settings.tileWidth, y * Settings.tileWidth, Settings.tileWidth, Settings.tileHeight), Color.Orange); 
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

            #region Debug
            if(Settings.displayCameraLocation)
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
            #endregion
        }

        public void Keyboard_down(KeyboardEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Keyboard_up(KeyboardEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void MouseButton_down(MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
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
