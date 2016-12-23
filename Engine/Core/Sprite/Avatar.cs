using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Sprite
{
    public enum Direction
    {
        Down = 0, 
        Left = 1, 
        Right = 2,
        Up = 3
    }

    public class Avatar
    {
        // information 
        public string CharacterName { get; set; }

        // Location 
        public int X { get; set; }
        public int Y { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }

        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public Direction CurrentDirection { get; set; }

        private Bitmap SpriteSheet { get; set; }
        private Surface[,] Frames { get; set; }
        public int CurrentFrame { get; set; }

        public Avatar(int x = 50, int y = 50)
        {
            this.X = x;
            this.Y = y;
            this.TargetX = x;
            this.TargetY = y; 

            this.CharacterName = "Jordan";

            this.SpriteSheet = (Bitmap)Bitmap.FromFile(@"Data\sheets\avatar\0.png");
            this.SpriteSheet.MakeTransparent(); //? 

            Frames = new Surface[4, 4];
            buildFrames();
           
            this.CurrentDirection = Direction.Down;
            this.CurrentFrame = 2;
        }

        public void buildFrames()
        {
            for(int y = 0; y < 4; y++)
            {
                for(int x = 0; x < 4; x++)
                {
                    Frames[x, y] = new Surface(SpriteSheet.Clone(new Rectangle(x * Settings.spriteWidth, y * Settings.spriteHeight, Settings.spriteWidth, Settings.spriteHeight), System.Drawing.Imaging.PixelFormat.DontCare)); 
                }
            }
        }

        public Surface GetFrame()
        {
            return Frames[CurrentFrame, (int)CurrentDirection];
        }


    }
}
