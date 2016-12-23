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

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int ScrollSpeed { get; set; }
     
        /// <summary>
        /// Create new camera at location 
        /// </summary>
        public Camera(int x = 0, int y = 0, int scrollSpeed = 1)
        {
            // take location 
            this.X = x;
            this.Y = y;
            this.ScrollSpeed = scrollSpeed;

            // calculate width of view and height
            this.Width = Settings.Width / Settings.tileWidth;
            this.Height = Settings.Height / Settings.tileHeight;
        }
    }
}
