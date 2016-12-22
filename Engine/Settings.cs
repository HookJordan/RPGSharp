using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class Settings
    {
        // Window settings 
        public static int Width = 720;
        public static int Height = 480;

        // Tile settings
        public static int tileWidth = 32;
        public static int tileHeight = 32;

        // strings 
        public static string WindowTitle = "Engine v0.1";

        // debug settings 
        public static bool displayFPS = true;
        public static bool displayGrid = true;
        public static bool displayCameraLocation = true;
        public static bool displayAvatarLocation = true; 

        public static Game Game { get; set; }
    }
}
