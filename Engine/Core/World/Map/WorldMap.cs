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

        public int SpawnX { get; set; }
        public int SpawnY { get; set; }

        // Tiles for this map 
        public MapTile[,]Tiles { get; set; }

        // public List<Surface> Surfaces { get; set; }

        public Dictionary<int, Surface> SurfaceTable { get; set; }
        public List<TileSheet> TileSheets { get; set; }

        public bool DarkMap { get; set; }

        public Dictionary<int, Sprite.NPC> NPC { get; set; }

        /// <summary>
        /// new Blank map
        /// </summary>
        public WorldMap(int xWidth = 100, int yHeight = 100)
        {
            this.NPC = new Dictionary<int, Sprite.NPC>();

            // set defaults 
            this.Width = xWidth;
            this.Height = yHeight;

            this.SpawnX = Width / 2;
            this.SpawnY = Height / 2;

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
            // LoadSurfaces(@"Data\sheets\tilesets\");
            SurfaceTable = new Dictionary<int, Surface>();
            loadSheets(@"Data\sheets\tilesets\"); 

        }

        private void loadSheets(string dir)
        {
            // default tile (blank?) 
            SurfaceTable.Add(0, new Surface(new Bitmap(Settings.tileWidth, Settings.tileHeight)));
            TileSheets = new List<TileSheet>(); 
            int sheetCount = System.IO.Directory.GetFiles(dir).Length;

            for(int i = 0; i < sheetCount; i++)
            {
                Bitmap sheet = (Bitmap)Bitmap.FromFile(dir + (i + 1) + ".png");
                if(TileSheets.Count == 0)
                {
                    TileSheets.Add(new TileSheet(sheet)); 
                } 
                else
                {
                    TileSheets.Add(new TileSheet(sheet, TileSheets[TileSheets.Count - 1].CountTiles()));
                }
            }
        }

        public Surface GetSurface(int id)
        {
            if (SurfaceTable.ContainsKey(id))
            {
                return SurfaceTable[id]; 
            }
            else
            {
                for(int i = 0; i < TileSheets.Count; i++)
                {
                    if(id < TileSheets[i].CountTiles())
                    {
                        Surface tile = new Surface(TileSheets[i].getTile(id));

                        SurfaceTable.Add(id, tile);

                        return tile;
                    }
                }
            }

            return GetSurface(0); 
        }

        //public void LoadSurfaces(string dir)
        //{
        //    Console.WriteLine("Loading surfaces."); 
        //    if (Content.Surfaces == null)
        //    {
        //        // for each tile sheet in the directory
        //        Bitmap sheet = null;
        //        Surfaces = new List<Surface>();
        //        Surfaces.Add(new Surface(new Bitmap(Settings.tileWidth, Settings.tileHeight)));

        //        int tileSheetcount = System.IO.Directory.GetFiles(dir).Length;

        //        for (int i = 1; i <= tileSheetcount; i++)
        //        {
        //            if (System.IO.File.Exists(dir + i + ".png"))
        //            {
        //                sheet = (Bitmap)Image.FromFile(dir + i + ".png");
        //                sheet.MakeTransparent();
        //                for (int y = 0; y < sheet.Height; y += Settings.tileHeight)
        //                {
        //                    for (int x = 0; x < sheet.Width; x += Settings.tileWidth)
        //                    {
        //                        Bitmap t = sheet.Clone(new Rectangle(x, y, Settings.tileWidth, Settings.tileHeight), System.Drawing.Imaging.PixelFormat.DontCare);
        //                        Surfaces.Add(new Surface(t));
        //                    }
        //                }
        //            }
        //        }

        //        Console.WriteLine("Loaded " + tileSheetcount + " tile sheets.");

        //        //List<string> sheets = System.IO.Directory.GetFiles(dir).ToList();
        //        //sheets.Sort();

        //        //foreach (string file in sheets)
        //        //{
        //        //    // load the graphic 
        //        //    sheet = (Bitmap)Image.FromFile(file);
        //        //    sheet.MakeTransparent();
        //        //    for (int y = 0; y < sheet.Height; y += Settings.tileHeight)
        //        //    {
        //        //        for (int x = 0; x < sheet.Width; x += Settings.tileWidth)
        //        //        {
        //        //            Bitmap t = sheet.Clone(new Rectangle(x, y, Settings.tileWidth, Settings.tileHeight), System.Drawing.Imaging.PixelFormat.DontCare);
        //        //            Surfaces.Add(new Surface(t));
        //        //        }
        //        //    }
        //        //}
        //        Content.Surfaces = this.Surfaces;
        //    }
        //    else
        //    {
        //        this.Surfaces = Content.Surfaces;
        //    }
        //}

        public MapTile GetTile(int x, int y)
        {
            if(x < Width && x >= 0 && y < Height && y >= 0)
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
                    Tiles[x, y].SetLayer(layer, tileID); 
                }
            }
        }

        public byte[,] getBlocks()
        {
            byte[,] grid = new byte[Width, Height];
            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    if (Tiles[x, y].Blocked)
                        grid[x, y] = 0;
                    else
                        grid[x, y] = 1;

            return grid;
        }
    }
}
