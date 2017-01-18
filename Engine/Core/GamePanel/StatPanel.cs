using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Graphics;

namespace Engine.Core.GamePanel
{
    public class StatPanel : PanelButton
    {
        // list of player stats to display 
        public StatPanel(int x, int y, int height, int width, bool selected = false) : base("Skills", x, y, height, width, selected)
        {

        }

        public override void DrawPanel(Surface s)
        {
            // do our drawing here 

            base.DrawPanel(s);
        }
    }
}
