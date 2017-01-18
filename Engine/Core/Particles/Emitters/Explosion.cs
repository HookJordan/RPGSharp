using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Particles.Emitters
{
    public class Explosion : ParticleSystem
    {
        public static readonly int TARGET_NUM_PARTICLES = 150;
        private Random rand = new Random();


        public Explosion(int x, int y)
        {
            this.Position = new SdlDotNet.Core.Vector(new Point(x, y));
            // this.Color = color; 

            // load up the explosive particles
            for(int i = 0; i < TARGET_NUM_PARTICLES; i++)
            {
                Particles.Add(GenerateParticle()); 
            }
        }


        public override bool Update()
        {
            Particle current;
            int count = Particles.Count;

            for (int i = 0; i < count && i > -1; i++)
            {
                current = Particles[i]; 

                if(!current.Update() || current.Life > current.MAX_LIFE)
                {
                    Particles.RemoveAt(i);
                    i--;
                    count = Particles.Count;
                }
            }

            if (Particles.Count <= 0)
                return false;

            return true; 
        }

        protected override Particle GenerateParticle()
        {
            // Generate random direction & speed for new particle
            float rndX = 2 * ((float)rand.NextDouble() - 0.5f);
            float rndY = 2 * ((float)rand.NextDouble() - 0.5f);
            float rndZ = 2 * ((float)rand.NextDouble() - 0.5f);

            return new Particle(this.Position,
                new SdlDotNet.Core.Vector(rndX, rndY, rndZ),
                Color.Red, Color.Yellow,
                rand.Next(50),
                150);

            //return new Particle(this.Position,
            //    new SdlDotNet.Core.Vector(rndX, rndY, rndZ),
            //    Color.Red, Color.Yellow,
            //    rand.Next(10),
            //    25);
        }
    }
}
