using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Particles
{
    public abstract class ParticleSystem
    {
        public List<Particle> Particles = new List<Particle>();
        public bool Regenerate { get; set; }
        public Vector Position = new Vector();
        public Color Color { get; set; }

        protected abstract Particle GenerateParticle();
        public abstract bool Update(); 

        public virtual void Draw(Surface surface)
        {
            Color c;
            double intense = 0;
            //int r = 0, g = 0, b = 0; 
            Particle current; 

            // loop all particles 
            for(int i = 0; i < Particles.Count; i++)
            {
                // get current particle 
                current = Particles[i];

                // calculate it's intensite based on life 
                intense = (double)(current.Life) / (double)current.MAX_LIFE;
                //intense = intense * 2;
                //intense = current.Life * 2 / (float)current.MAX_LIFE;

                //if (intense > 1)
                //    intense = 1;

                // generate pen for the particle 
                c = Color.FromArgb((int)(intense * (double)current.Color.R),
                    (int)(intense * (double)current.Color.G),
                    (int)(intense * (double)current.Color.B));

                ////r = (current.MaxColor.R - current.Color.R) * intense;
                ////g = (current.MaxColor.G - current.Color.G) * intense;
                ////b = (current.MaxColor.B - current.Color.B) * intense;

                //r = (current.MaxColor.R - current.Color.R) * intense;
                //g = (current.MaxColor.G - current.Color.G) * intense;
                //b = (current.MaxColor.B - current.Color.B) * intense;

                //c = Color.FromArgb(r + current.Color.R,
                //g + current.Color.G,
                //b + current.Color.B);
                //c = Color.FromArgb(r, g, b);

                // create ellipse to draw 
                Ellipse e = new Ellipse();
                e.PositionX = (short)current.Position.X;
                e.PositionY = (short)current.Position.Y;
                e.RadiusX = (short)Math.Max(1, 4 * current.Life / 1000);
                e.RadiusY = (short)Math.Max(1, 4 * current.Life / 1000);

                // draw it 
                surface.Draw(e, c);
            }
        }
    }
}
