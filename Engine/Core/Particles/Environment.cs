using SdlDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Particles
{
    public class Environment
    {
        public static Environment Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Environment();
                return _instance;
            }
        }

        private static Environment _instance { get; set; }
        public Vector Gravity { get; set; }
        public Vector Wind { get; set; }

        // not accessible 
        protected Environment()
        {
            Gravity = new Vector();
            Wind = new Vector(); 
        }
    }
}
