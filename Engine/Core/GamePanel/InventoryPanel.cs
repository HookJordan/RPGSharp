using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Core.Sprite.Items;
using SdlDotNet.Graphics;
using System.Drawing;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

namespace Engine.Core.GamePanel
{
    public class InventoryPanel : PanelButton
    {
        public int HoverSlot = -1;
        public int SelectedSlot = -1;
        public int SecondaryX = 0;
        public int SecondaryY = 0; 
        public bool SecondaryMenu = false; 

        public InventoryItem[] Slots { get; set; }

        public InventoryPanel(int x, int y, int height, int width, bool selected = false) : base("Inventory", x, y, height, width, selected)
        {
            this.Slots = new InventoryItem[28];
        }

        public override void DrawPanel(Surface s)
        {
            int x = Settings.tileWidth;
            int y = 0; 

            // Loop through each item in the inventory 
            for(int i = 0; i < Slots.Length; i++)
            {
                // rows of 4 
                if (i % 4 == 0 && i != 0)
                {
                    y += Settings.tileHeight * 2;
                    x = Settings.tileWidth;
                }

                // outline the item... -> switch line below to draw item index image 
                if (Slots[i] != null)
                {

                    // Draw item icon 
                    s.Blit(Slots[i].GetIcon(), new Point(x, y)); 

                    // Check if the item is stackable 
                    if (Slots[i].Stackable)
                    {
                        // If it is, draw amount under it? 
                        var amount = Content.gameFont.Render(Slots[i].Amount.ToString(), Color.White);
                        s.Blit(amount, new Point(x + 16, y + 24));
                        amount.Dispose(); 
                    }
                }
                else
                {
                    // outline? 
                    s.Draw(new Box(new Point(x, y), new Point(x + Settings.tileWidth, y + Settings.tileHeight)), Color.White);
                }

                // keep moving 
                x += Settings.tileWidth * 2;
            }
            int mouseX = Mouse.MousePosition.X - (Settings.Width - (Settings.PanelWidth - 3 ) * Settings.tileWidth);
            int mouseY = Mouse.MousePosition.Y - Settings.tileHeight * 5;
            if (SelectedSlot != -1 && Slots[SelectedSlot] != null)
            {
                
                s.Blit(Slots[SelectedSlot].GetIcon(), new Point(mouseX, mouseY));
            }

            if(SecondaryMenu)
            {
                s.Fill(new Rectangle(SecondaryX, SecondaryY, 3 * Settings.tileWidth, 4 * Settings.tileHeight), Color.Gray);
            }

            var inventoryXY = Content.debugFont.Render(mouseX + "," + mouseY + " hover: " + HoverSlot, Color.Red);
            s.Blit(inventoryXY, new Point(0, 354));
            inventoryXY.Dispose(); 

            base.DrawPanel(s);  
        }

        private void SwapItems(int x, int y)
        {
            InventoryItem temp = Slots[x];
            Slots[x] = Slots[y];
            Slots[y] = temp; 
        }

        public override void MouseDown(MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            int mouseX = Mouse.MousePosition.X - (Settings.Width - (Settings.PanelWidth - 3) * Settings.tileWidth);
            int mouseY = e.Y - Settings.tileHeight * 5;

            if (e.Button == MouseButton.PrimaryButton)
            {
                if (!SecondaryMenu)
                {
                    SelectedSlot = HoverSlot;
                }
                else
                {
                    // close secondary menu now... 
                    SecondaryMenu = false; 
                }
            }
            else
            {
                SecondaryMenu = true;
                SecondaryX = mouseX;
                SecondaryY = mouseY;
            }
        }

        public override void MouseUp(MouseButtonEventArgs e)
        {
            base.MouseUp(e);

            if(SelectedSlot != -1 && HoverSlot != -1 && Slots[SelectedSlot] != null)
            {
                SwapItems(SelectedSlot, HoverSlot);
            }

            SelectedSlot = -1; 
        }

        public override void MouseMove(MouseMotionEventArgs e)
        {
            base.MouseMove(e);

            HoverSlot = -1;

            int x = 0;
            int y = 0;

            bool xMatch = false;
            bool yMatch = false;

            int mouseX = Mouse.MousePosition.X - (Settings.Width - (Settings.PanelWidth - 3) * Settings.tileWidth);
            int mouseY = e.Y - Settings.tileHeight * 5; 

            // Loop through all the slots... 
            for(int i = 0; i < Slots.Length; i++)
            {
                // rows of 4 
                if (i % 4 == 0 && i != 0)
                {
                    y += Settings.tileHeight * 2;
                    x = 0;
                }

                // check x and y bounds of each slot 
                xMatch = (mouseX  >= x && mouseX <= x + Settings.tileWidth);
                yMatch = (mouseY >= y && mouseY <= y + Settings.tileHeight); 

                if(xMatch && yMatch)
                {
                    HoverSlot = i; 
                    break;
                }

                x += Settings.tileWidth * 2; 
            }

            if (e.Button == MouseButton.PrimaryButton)
            {
                // draw preview of the dragging / selected slot ? 
            }
        }
    }

}
