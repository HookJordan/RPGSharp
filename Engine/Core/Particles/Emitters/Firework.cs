using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Particles.Emitters
{
    class Firework : ParticleSystem
    {
        public static readonly int TARGET_NUM_PARTICLES = 150;
        private Random rand = new Random();


        public Firework(int x, int y)
        {
            this.Position = new SdlDotNet.Core.Vector(new Point(x, y));
            // this.Color = color; 
            this.Regenerate = false;
            // load up the explosive particles
            for (int i = 0; i < TARGET_NUM_PARTICLES; i++)
            {
                Particles.Add(GenerateParticle());
            }
        }


        public override bool Update()
        {
            Particle current;
            int count = Particles.Count;

            //while (Particles.Count < Firework.TARGET_NUM_PARTICLES)
                //Particles.Add(GenerateParticle());


            for (int i = 0; i < count && i > -1; i++)
            {
                current = Particles[i];

                if (!current.Update() || current.Life > current.MAX_LIFE)
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

        Color[] options = new Color[] { Color.Cyan, Color.Magenta, Color.Red, Color.Yellow };

        protected override Particle GenerateParticle()
        {
            // Generate random direction & speed for new particle
            float rndX = rand.Next(-5, 5) * ((float)rand.NextDouble() - 0.5f);
            float rndY = -1 * 3 * ((float)rand.NextDouble());
            float rndZ = 8 * ((float)rand.NextDouble() - 0.5f);

            

            return new Particle(this.Position,
                new SdlDotNet.Core.Vector(rndX, rndY, rndZ),
                options[rand.Next(0, options.Length)], Color.Yellow,
                rand.Next(50),
                75);

            //return new Particle(this.Position,
            //    new SdlDotNet.Core.Vector(rndX, rndY, rndZ),
            //    Color.Red, Color.Yellow,
            //    rand.Next(10),
            //    25);
        }
    }
}
