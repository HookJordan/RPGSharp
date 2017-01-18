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

        public int nextX { get; set; }
        public int nextY { get; set; }
        public List<Point> Points { get; set; }

        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public Direction CurrentDirection { get; set; }

        private Bitmap SpriteSheet { get; set; }
        private Surface[,] Frames { get; set; }
        public int CurrentFrame { get; set; }

        public bool Moving { get { return TargetX != X && TargetY != Y; } }

        public int MoveSpeed { get { return 8; } } //if (Run) { return 16; } else { return 8; } } }

        public bool Run { get; set; }
        public double Stamina { get; set; }
        public double CurrentStamina { get; set; }

        public bool Combat { get; set; }

        // skills 
        public Dictionary<string, SpriteSkill.Skill> Skills { get; set; }

        public Avatar(int x = 50, int y = 50, string spriteSheet = @"Data\sheets\avatar\0.png")
        {
            this.X = x;
            this.Y = y;
            this.TargetX = x;
            this.TargetY = y; 

            this.CharacterName = "Jordan";

            this.SpriteSheet = (Bitmap)Bitmap.FromFile(spriteSheet);
            this.SpriteSheet.MakeTransparent(); //? 

            Frames = new Surface[4, 4];
            buildFrames();
           
            this.CurrentDirection = Direction.Down;
            this.CurrentFrame = 2;

            // setup skill table
            this.Skills = new Dictionary<string, SpriteSkill.Skill>();
            this.Skills.Add("hp", new SpriteSkill.Skill(10, SpriteSkill.Skill.Experience(10), 10));

            foreach(string skill in this.Skills.Keys)
            {
                this.Stamina += Skills[skill].Level;
            }

            this.CurrentStamina = this.Stamina;
        }

        /// <summary>
        /// Call this function to cut the sprite sheet into separate images.. (easier to work with) 
        /// </summary>
        public void buildFrames()
        {
            // Implement paperdoll items here :D 
            // draw each layer one at a time for the sheet here starting with the base, then cut it up 
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
            //get the current frame
            return Frames[CurrentFrame, (int)CurrentDirection];
        }


        public double GetStaminaPercentage()
        {
            // simple math for stamina percentage 
            return (CurrentStamina / Stamina) * 100.0;
        }

        #region HPHelper
        public int GetHP()
        {
            return Skills["hp"].Value;
        }

        public double GetHPPercentage()
        {
            // Simple math for hp percentage -> used to draw UI bars 
            return ((double)Skills["hp"].Value / (double)Skills["hp"].Level) * 100.0;
        }

        public int GetTotalHP()
        {
            return Skills["hp"].Level;
        }

        public int TakeDammage(int a)
        {
            Skills["hp"].Value -= a;

            return GetHP(); 
        }
        #endregion

    }
}
