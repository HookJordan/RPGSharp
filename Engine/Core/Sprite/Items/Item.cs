using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Sprite.Items
{
    public enum ItemType
    {
        Currency = 0,
        Food = 1, 
        Gear = 2,
        Weapon = 3
    }
    public class Item
    {
        public int Id { get; set; }
        public bool Stackable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool TradeAble { get; set; }
        public string SpriteSheetPath { get { return @"Data\sheets\item\" + Id + ".png"; } }

        public ItemType Type { get; set; }
    }

    public class InventoryItem : Item
    {
        private Surface[] icons; 

        public InventoryItem(int id, string name, string desc, ItemType type, bool stackable = false, bool tradeable = true)
        {
            this.Id = id;
            this.Name = name;
            this.Description = desc;
            this.Type = type;
            this.Stackable = stackable;
            this.TradeAble = tradeable;
            this.Amount = 1; // default 

            // load icon into surface 
            loadIcon();
        }

        private void loadIcon()
        {
            if(Stackable)
            {
                // stackable items can have muliple images to represent different amounts 
                icons = new Surface[4];

                // Load the bitmap of the item sheet
                Bitmap sheet = (Bitmap)Bitmap.FromFile(SpriteSheetPath); 
                
                for(int i = 0; i < icons.Length; i++)
                {
                    // init the surface 
                    //icons[i] = new Surface(new Bitmap(Settings.tileWidth, Settings.tileHeight));

                    Bitmap b = new Bitmap(Settings.tileWidth, Settings.tileHeight); 

                    // create graphics from the bitmap 
                    Graphics g = Graphics.FromImage(b);

                    // copy frame into surface 
                    g.DrawImage(sheet, new Rectangle(0, 0, Settings.tileWidth, Settings.tileHeight), new Rectangle(i * Settings.tileWidth, 0, Settings.tileWidth, Settings.tileHeight), GraphicsUnit.Pixel);

                    // done with GDI 
                    g.Dispose();

                    // b.Save(@"Data\sheets\item\" + i + "_.png");

                    icons[i] = new Surface(b); 
                }
            }
            else
            {
                // non stackable only have 1 icon 
                icons = new Surface[1];
                icons[0] = new Surface((Bitmap)Bitmap.FromFile(SpriteSheetPath)); 
            }
        }

        public int Amount { get; set; }

        public void Add(int amount)
        {
            this.Amount += amount;
        }

        public void Remove(int amount)
        {
            this.Amount -= amount; 
        }

        public Surface GetIcon()
        {
            if (Amount == 1)
            {
                return icons[0]; 
            }
            else if(Amount < 3)
            {
                return icons[1];
            }
            else if (Amount < 30)
            {
                return icons[2];
            }
            else
            {
                return icons[3]; 
            }
        }
    }
}
