using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class Content
    {
        public static SdlDotNet.Graphics.Font debugFont = new SdlDotNet.Graphics.Font(@"Data\font\debug.ttf", 12);

        public static SdlDotNet.Graphics.Font gameFont = new SdlDotNet.Graphics.Font(@"Data\font\debug.ttf", 8);

        public static SdlDotNet.Graphics.Font statsFont = new SdlDotNet.Graphics.Font(@"Data\font\debug.ttf", 24);

        public static Surface PanelTop { get { if (panelTop == null) { panelTop = LoadContent(@"Data\gui\panel-top.png"); } return panelTop; } }
        private static Surface panelTop; 

        private static Surface LoadContent(string path)
        {
            return new Surface((Bitmap)Bitmap.FromFile(path)); 
        }

        private static Surface NightlyOverLay { get; set; }
        public static Surface NightlyOverlay
        {
            get
            {
                // not created yet, so render it 
                if(NightlyOverLay == null)
                {
                    Bitmap overlay = new Bitmap(Settings.Width, Settings.Height);
                    Graphics g = Graphics.FromImage(overlay);
                    SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 8));
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
                    g.FillRectangle(semiTransBrush, new Rectangle(0, 0, overlay.Width, overlay.Height));
                    g.Dispose();

                    NightlyOverLay = new Surface(overlay);
                }

                // return the nightly surface 
                return NightlyOverLay;
            }
        }

        public static Surface PathTile
        {
            get
            {
                if(pTile == null)
                {
                    pTile = new Surface(TransparentTile);
                }

                return pTile;
            }
        }

        private static Surface pTile { get; set; }

        private static Bitmap transParentOverlay { get; set; }
        public static Bitmap TransparentTile
        {
            get
            {
                if(transParentOverlay == null)
                {
                    transParentOverlay = new Bitmap(Settings.tileWidth, Settings.tileHeight);
                    Graphics g = Graphics.FromImage(transParentOverlay);
                    //SolidBrush opaqueBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));
                    SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 255));
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
                    g.FillRectangle(semiTransBrush, new Rectangle(0, 0, transParentOverlay.Width, transParentOverlay.Height));
                    g.Dispose();

                    return transParentOverlay;
                }
                else
                {
                    return transParentOverlay;
                }
            }
        }

    }
}
