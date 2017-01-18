using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.World.Triggers
{
    public class Trigger
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // store scripted paramters here that can be used for anything 
        public string Data { get; set; }
    }
}
