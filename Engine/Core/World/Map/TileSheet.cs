using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.World.Map
{
    public class TileSheet
    {
        public Bitmap Sheet { get; private set; }
        public int StartIndex { get; private set; }

        public TileSheet(Bitmap sheet, int start = 1)
        {
            this.Sheet = sheet;
            this.StartIndex = start;
        }

        public int CountTiles()
        {
            return ((Sheet.Width / Settings.tileWidth) * (Sheet.Height / Settings.tileHeight)) + StartIndex;
        }

        public Surface getTile(int id)
        {
            int count = StartIndex;
            Bitmap tile = new Bitmap(Settings.tileWidth, Settings.tileHeight);

            for(int y = 0; y < Sheet.Height / Settings.tileHeight; y++)
            {
                for(int x = 0; x < Sheet.Width / Settings.tileWidth; x++)
                {
                    if(count == id)
                    {
                        Graphics g = Graphics.FromImage(tile);
                        g.DrawImage(Sheet, new Rectangle(0, 0, Settings.tileWidth, Settings.tileHeight), new Rectangle(x * Settings.tileWidth, y * Settings.tileHeight, Settings.tileWidth, Settings.tileHeight), GraphicsUnit.Pixel);
                        g.Dispose();

                        return new Surface(tile);
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            //for (int x = 0; x < Sheet.Width / Settings.tileWidth; x++)
            //{
            //    for (int y = 0; y < Sheet.Height / Settings.tileHeight; y++)
            //    {
            //        if (count == id)
            //        {
            //            Graphics g = Graphics.FromImage(tile);
            //            g.DrawImage(Sheet, new Rectangle(0, 0, Settings.tileWidth, Settings.tileHeight), new Rectangle(x * Settings.tileWidth, y * Settings.tileHeight, Settings.tileWidth, Settings.tileHeight), GraphicsUnit.Pixel);
            //            g.Dispose();

            //            return new Surface(tile);
            //        }
            //        else
            //        {
            //            count++;
            //        }
            //    }
            //}

            return new Surface(tile);
        }
    }
}
