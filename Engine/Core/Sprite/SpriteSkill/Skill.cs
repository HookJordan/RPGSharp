using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Sprite.SpriteSkill
{
    public class Skill
    {
        public int Level { get; set; }
        public int Value { get; set; } // boosts ? and HP counter 

        public double CurrentXP { get; set; }
        public double xpRequired { get; set; }


        public Skill()
        {
            this.Level = 1;
            this.Value = 1; 
            this.CurrentXP = 0;
        }

        public Skill(int level, double currentXP, int value)
        {
            // level skill to match current xp 
            this.CurrentXP = currentXP;
            this.Level = level;
            this.Value = value;
            xpRequired = Experience(level + 1);
        }

        public static double Experience(int level)
        {
            double a = 0;
            for (int i = 1; i < level; i++)
            {
                a += Math.Floor((i + 300) * (Math.Pow(2, i / 7)));
            }

            return Math.Floor(a / 4);

        }
    }
}
