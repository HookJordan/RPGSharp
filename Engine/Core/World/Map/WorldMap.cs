using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public List<Surface> Surfaces { get; set; }

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

            // load map textures
            LoadSurfaces(@"Data\sheets\tilesets\");
        }

        public void LoadSurfaces(string dir)
        {
            if (Content.Surfaces == null)
            {
                // for each tile sheet in the directory
                Bitmap sheet = null;
                Surfaces = new List<Surface>();
                Surfaces.Add(new Surface(new Bitmap(Settings.tileWidth, Settings.tileHeight)));

                int tileSheetcount = System.IO.Directory.GetFiles(dir).Length;

                for (int i = 1; i <= tileSheetcount; i++)
                {
                    if (System.IO.File.Exists(dir + i + ".png"))
                    {
                        sheet = (Bitmap)Image.FromFile(dir + i + ".png");
                        sheet.MakeTransparent();
                        for (int y = 0; y < sheet.Height; y += Settings.tileHeight)
                        {
                            for (int x = 0; x < sheet.Width; x += Settings.tileWidth)
                            {
                                Bitmap t = sheet.Clone(new Rectangle(x, y, Settings.tileWidth, Settings.tileHeight), System.Drawing.Imaging.PixelFormat.DontCare);
                                Surfaces.Add(new Surface(t));
                            }
                        }
                    }
                }

                //List<string> sheets = System.IO.Directory.GetFiles(dir).ToList();
                //sheets.Sort();

                //foreach (string file in sheets)
                //{
                //    // load the graphic 
                //    sheet = (Bitmap)Image.FromFile(file);
                //    sheet.MakeTransparent();
                //    for (int y = 0; y < sheet.Height; y += Settings.tileHeight)
                //    {
                //        for (int x = 0; x < sheet.Width; x += Settings.tileWidth)
                //        {
                //            Bitmap t = sheet.Clone(new Rectangle(x, y, Settings.tileWidth, Settings.tileHeight), System.Drawing.Imaging.PixelFormat.DontCare);
                //            Surfaces.Add(new Surface(t));
                //        }
                //    }
                //}
                Content.Surfaces = this.Surfaces;
            }
            else
            {
                this.Surfaces = Content.Surfaces;
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

        public void SeetMap(int layer, int tileID)
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    Tiles[x, y].SetLayer(0, tileID); 
                }
            }
        }
    }
}
