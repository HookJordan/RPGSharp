using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Core.GUI
{
    public partial class MapEditor : Form
    {
        //mouse locations
        public int mouseX;
        public int mouseY;
        public int MouseEndX;
        public int MouseEndY;
        public int MouseStartX;
        public int MouseStartY;

        // selections 
        public int SelectedTile;
        public int SelectedLayer;

        public int[,] MultiSelected;

        List<Bitmap> TileSets { get; set; }
        public Core.Scenes.OpenWorld Scene { get; set; }

        public MapEditor(Core.Scenes.OpenWorld sc)
        {
            this.Scene = sc;
            InitializeComponent();


            this.FormClosing += MapEditor_FormClosing;
        }

        private void MapEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void MapEditor_Load(object sender, EventArgs e)
        {
            this.TileSets = new List<Bitmap>();

            string dir = @"Data\sheets\tilesets\"; 
            int tileSheetcount = System.IO.Directory.GetFiles(dir).Length;

            for (int i = 1; i <= tileSheetcount; i++)
            {
                if (System.IO.File.Exists(dir + i + ".png"))
                {
                    this.TileSets.Add((Bitmap)Bitmap.FromFile(dir + i + ".png"));
                    lstTileSets.Items.Add("Tile Set " + TileSets.Count);
                }
            }

            //foreach (string filePath in Directory.GetFiles(@"Data\sheets\tilesets\"))
            //{
            //    this.TileSets.Add((Bitmap)Bitmap.FromFile(filePath));
            //    lstTileSets.Items.Add("Tile Set " + TileSets.Count); 
            //}

            pbSet.Image = TileSets[0]; 
            
            for(int i = 0; i < this.Scene.Map.Tiles[0, 0].Layers.Length; i++)
            {
                this.lstLayers.Items.Add("Layer " + (i + 1));
            }

            lstTileSets.SelectedIndex = 0;
            lstLayers.SelectedIndex = 0;

            SelectedLayer = 0; 
        }

        private void lstTileSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbSet.Image = TileSets[lstTileSets.SelectedIndex];
          
        }

        private void pbSet_Click(object sender, EventArgs e)
        {
            
        }

        private int getID(int xLoc, int yLoc)
        {
            int tilesPerSheet = (pbSet.Image.Width / Settings.tileWidth) * (pbSet.Image.Height / Settings.tileHeight);
            int startIndex = (lstTileSets.SelectedIndex * tilesPerSheet) + 1;

            startIndex += (pbSet.Image.Width / Settings.tileWidth) * yLoc;
            startIndex += xLoc;

            pbPreview.Image = this.Scene.Map.Surfaces[startIndex].Bitmap;

            return startIndex;
        }

        private void lstLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedLayer = lstLayers.SelectedIndex;
        }

        private void pbSet_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X / Settings.tileWidth;
            mouseY = e.Y / Settings.tileHeight;

            MouseEndX = mouseX + 1;
            MouseEndY = mouseY + 1;

            if(e.Button == MouseButtons.Left)
            {
                pbSet.Refresh(); // remove old outline... 

                // set tile 
                SelectedTile = getID(mouseX, mouseY);
                lblTile.Text = "Selected: " + SelectedTile;

                int width = (MouseEndX - MouseStartX) * Settings.tileWidth;
                int height = (MouseEndY - MouseStartY) * Settings.tileHeight;

                if (width < 0 || height < 0)
                    return; // can only select right and down 

                // set array of tiles 
                MultiSelected = new int[MouseEndX - MouseStartX, MouseEndY - MouseStartY];

                for(int x = 0; x < MultiSelected.GetLength(0); x++)
                {
                    for(int y = 0; y < MultiSelected.GetLength(1); y++)
                    {
                        MultiSelected[x, y] = getID(x + MouseStartX, y + MouseStartY); 
                    }
                }

                Rectangle r = new Rectangle(MouseStartX * Settings.tileWidth, MouseStartY * Settings.tileHeight, width, height);

                // outline selected 
                Graphics g = pbSet.CreateGraphics();
                g.DrawRectangle(Pens.Red, r);
                g.Dispose();
            }
            // EndX = mouseX;
            // EndY = mouseY;
        }

        private void pbSet_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEndX = mouseX;
            MouseEndY = mouseY;
        }

        private void pbSet_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStartX = mouseX;
            MouseStartY = mouseY;

            pbSet.Refresh(); // remove old outline... 

            // set tile 
            SelectedTile = getID(mouseX, mouseY);
            lblTile.Text = "Selected: " + SelectedTile;

            // outline selected 
            Graphics g = pbSet.CreateGraphics();
            g.DrawRectangle(Pens.Red, new Rectangle(mouseX * Settings.tileWidth, mouseY * Settings.tileHeight, Settings.tileWidth, Settings.tileHeight));
            g.Dispose();

            MultiSelected = new int[1, 1];
            MultiSelected[0, 0] = SelectedTile;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scene.Map = new World.Map.WorldMap();
        }

        private void btnSeed_Click(object sender, EventArgs e)
        {
            for(int x = 0; x < Scene.Map.Width; x++)
            {
                for(int y= 0; y < Scene.Map.Height; y++)
                {
                    Scene.Map.Tiles[x, y].Layers[SelectedLayer] = SelectedTile;
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Map Files | *.map";
                sfd.InitialDirectory = @"Data\maps\"; 

                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    Core.World.Map.MapFactory.ExportMap(Scene.Map, sfd.FileName); 
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = @"Data\maps\";
                ofd.Filter = "Map Files | *.map"; 

                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    Scene.Map = Core.World.Map.MapFactory.ImportMap(ofd.FileName); 
                }
            }
        }
    }
}
