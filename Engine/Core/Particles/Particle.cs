using SdlDotNet.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Particles
{
    public class Particle
    {
        public int MAX_LIFE { get; set; }

        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public int Life { get; set; }
        public Color Color { get; set; }
        public Color MaxColor { get; set; }

        public Particle()
        {
            this.Position = new Vector();
            this.Velocity = new Vector();
            this.Life = 0;
            this.Color = Color.White;
        }

        public Particle(Vector pos, Vector vel, Color col, Color maxColor, int life, int maxLife)
        {
            this.Position = pos;
            this.Velocity = vel;
            this.Life = life;
            this.Color = col;
            this.MaxColor = maxColor;
            this.MAX_LIFE = maxLife;
        }

        public bool Update()
        {
            // update location 
            this.Velocity = this.Velocity - Environment.Instance.Gravity + Environment.Instance.Wind;
            this.Position = this.Position + this.Velocity;

            // age particle 
            this.Life++;

            // check if particle should expire 
            if (this.Life > MAX_LIFE)
                return false;

            // otherwise return true 
            return true; 
        }
    }
}
