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
        public int CurrentXP { get; set; }
        public int NextLevel { get; set; }
        public int AccumulatedXP { get; set; }

        public Skill()
        {
            this.Level = 1;
            this.CurrentXP = 0;
            this.NextLevel = 128;
        }

        public void Gain(int xp)
        {
            this.CurrentXP += xp;

            if(this.CurrentXP > this.NextLevel)
            {
                this.CurrentXP = this.CurrentXP - this.NextLevel;
                this.Level++;
                this.NextLevel = this.Level * 128; 
            }
        }
    }
}
