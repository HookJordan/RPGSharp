using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.World.Map
{
    public class WorldMap
    {
        // X,Y Tiles 
        public int Width { get; private set; }
        public int Height { get; private set; }

        // Tiles for this map 
        public MapTile[,]Tiles { get; set; }

        /// <summary>
        /// new Blank map
        /// </summary>
        public WorldMap(int xWidth = 100, int yHeight = 100)
        {
            // set defaults 
            this.Width = xWidth;
            this.Height = yHeight;

            // Init tiles
            this.Tiles = new Map.MapTile[xWidth, yHeight]; 

            // setup each tiles
            for(int x = 0; x < xWidth; x++)
            {
                for(int y = 0; y < yHeight; y++)
                {
                    // init each individual 
                    this.Tiles[x, y] = new Map.MapTile(); 
                }
            }
        }

        public MapTile GetTile(int x, int y)
        {
            if(x < Width && x > 0 && y < Height && y > 0)
            {
                return Tiles[x, y]; 
            }
            else
            {
                return null; 
            }
        }

        public void SetTitle(MapTile t)
        {

        }
    }
}
