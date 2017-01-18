using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.World
{
    public class Camera
    {
        // Location variables for camera 
        public int X { get; set; }
        public int Y { get; set; }
        public int xScroll { get; set; }
        public int yScroll { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int ScrollSpeed { get; set; }
     
        /// <summary>
        /// Create new camera at location 
        /// </summary>
        public Camera(int x = 0, int y = 0, int scrollSpeed = 1, int xscroll = 10, int yscroll = 7)
        {
            // take location 
            this.X = x;
            this.Y = y;
            this.ScrollSpeed = scrollSpeed;
            this.xScroll = xscroll;
            this.yScroll = yscroll;

            // calculate width of view and height
            this.Width = (Settings.Width / Settings.tileWidth); // - Settings.PanelWidth;
            this.Height = Settings.Height / Settings.tileHeight;

            this.X = (this.X - this.Width / 2 ) + 6;
            this.Y = (this.Y - this.Height / 2);
        }

        public bool Within(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }
    }
}
