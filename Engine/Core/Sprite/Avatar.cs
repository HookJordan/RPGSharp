using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Sprite
{
    public class Avatar
    {
        // information 
        public string CharacterName { get; set; }

        // Location 
        public int X { get; set; }
        public int Y { get; set; }

        public Avatar(int x = 50, int y = 50)
        {
            this.X = x;
            this.Y = y;

            this.CharacterName = "Jordan"; 
        }
    }
}
