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
        public static int Width = 1280;
        public static int Height = 720;

        // Tile settings
        public static int tileWidth = 32;
        public static int tileHeight = 32;
        public static int PanelWidth = 13; 

        // sprite size
        public static int spriteWidth = 32;
        public static int spriteHeight = 48;

        // strings 
        public static string WindowTitle = "Engine v0.1";

        // debug settings 
        public static bool displayFPS = false;
        public static bool displayGrid = false;
        public static bool displayCameraLocation = false;
        public static bool displayAvatarLocation = false;
        public static bool displayAvatarPath = false;
        public static bool displayMouseLocation = false;
        public static bool highlightMouseLocation = true;

        // Shared instances 
        public static Game Game { get; set; }
        public static Core.GUI.MapEditor Editor { get; set; }
    }
}
