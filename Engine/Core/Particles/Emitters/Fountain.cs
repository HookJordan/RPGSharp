using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Particles.Emitters
{
    public class Fountain : ParticleSystem
    {
        private static readonly int TARGET_NUM_PARTICLES = 150; 

        private Random rand = new Random();
        public Color MaxColor { get; private set; }

        public Fountain(int x, int  y, Color min, Color max)
        {
            this.Position = new SdlDotNet.Core.Vector(new Point(x, y));
            this.Color = min;
            this.MaxColor = max;
            this.Regenerate = true;

            // create a few starting particles 
            for(int i = 0; i < 5; i++)
            {
                this.Particles.Add(GenerateParticle()); 
            }
        }

        public override bool Update()
        {
            Particle current;

            // num of particles currently in system 
            int count = Particles.Count; 

            // loop threach each partcile 
            for(int i = 0; i < count && i > -1; i++)
            {
                // get current particle 
                current = Particles[i]; 

                // move and remove if needed 
                if(!current.Update() || current.Life > current.MAX_LIFE)
                {
                    Particles.RemoveAt(i);
                    i--;
                    count = Particles.Count; 
                }
            }

            // if not enough particles 
            if(Particles.Count < Fountain.TARGET_NUM_PARTICLES)
            {
                //int random = rand.Next(10);
                //for(int i = 0; i < random; i++)
                    Particles.Add(GenerateParticle());
            }

            return true; //always true becuase we are regnerating 
        }

        protected override Particle GenerateParticle()
        {
            // generate direction and such 
            float rndX = 0.5f * ((float)rand.NextDouble() - 0.4f);
            float rndY = -1 - 1 * (float)rand.NextDouble();
            float rndZ = 2 * ((float)rand.NextDouble() - 0.4f);

            // return the new particle 
            return new Particle(Position, new SdlDotNet.Core.Vector(rndX, rndY, rndZ), this.Color, this.MaxColor, rand.Next(50), 150);
        }
    }
}
