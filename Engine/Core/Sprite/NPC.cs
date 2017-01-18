using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Sprite
{
    public class NPC : Avatar
    {

        public int OriginalX { get; set; }
        public int OriginalY { get; set; }

        public bool Wait { get { return Paused; } }
        private bool Paused { get; set; }

        public bool CanMove { get; set; }
        public int MoveRadius { get; set; }

        public int Id { get; set; }

        public NPC(int x, int y, int id, int MoveRadius = 7) : base(x, y, @"Data\\sheets\\avatar\1.png")
        {
            this.Id = id;
            this.OriginalX = x;
            this.OriginalY = y;
            this.CanMove = true;
            this.MoveRadius = MoveRadius;
        }

        // seconds? 
        public void WaitMovement(int delay)
        {
            this.Paused = true;
            Console.WriteLine("NPC: " + Id + " has paused movement.");

            new System.Threading.Thread(() =>
            {
                System.Threading.Thread.Sleep(delay);
                this.Paused = false;
                Console.WriteLine("NPC: " + Id + " has resumed movement.");
            }).Start();

        }
       
    }
}
