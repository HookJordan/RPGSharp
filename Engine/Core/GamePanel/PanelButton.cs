using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.GamePanel
{
    public class PanelButton
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        protected bool LeftMouseClicked = false; 

        public PanelButton(string name, int x, int y, int height, int width, bool selected = false)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;
            this.Selected = selected; 
        }

        public bool IntersectsWith(int x, int y)
        {
            return (x >= X && x <= X + Width && y >= Y && y <= Y + Height) ;
        }

        public Surface RenderTitle()
        {
            return Content.debugFont.Render(Name, GetDrawColor());
        }

        public void DrawButton(Surface s)
        {
            s.Blit(RenderTitle(), new Point(X, Y)); 
        }

        public Color GetDrawColor()
        {
            if(Selected)
            {
                return Color.Red;
            }
            else
            {
                return Color.White;
            }
        }

        public virtual void DrawPanel(Surface s)
        {
            // any final touches? 
        }

        public virtual void MouseMove(SdlDotNet.Input.MouseMotionEventArgs e)
        {

        }

        public virtual void MouseDown(SdlDotNet.Input.MouseButtonEventArgs e)
        {
            if(e.Button == SdlDotNet.Input.MouseButton.PrimaryButton)
            {
                LeftMouseClicked = true; 
            }
        }

        public virtual void MouseUp(SdlDotNet.Input.MouseButtonEventArgs e)
        {
            if(e.Button == SdlDotNet.Input.MouseButton.PrimaryButton)
            {
                LeftMouseClicked = false; 
            }
        }
    }
}
