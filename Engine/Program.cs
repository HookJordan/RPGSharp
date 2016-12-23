using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Core.Game g = new Core.Game();
            g.Run(); 
        }
    }
}
