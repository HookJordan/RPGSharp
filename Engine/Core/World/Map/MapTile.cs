using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.World.Map
{
    public class MapTile
    {
        public int[] Layers { get; set; }
        public int TriggerId { get; set; }
        public bool Blocked { get; set; }
        public List<Sprite.Items.Item> Items { get; set; }

        public MapTile()
        {
            // 4 drawable layers 
            this.Layers = new int[4];
            this.Blocked = false;
            this.TriggerId = -1; // no trigger? 
            this.Items = new List<Sprite.Items.Item>(); 
        }

        public int GetLayer (int layer)
        {
            if(layer > Layers.Length)
            {
                return -1; 
            }
            else
            {
                return Layers[layer];
            }
        }

        public void SetLayer(int layer, int value)
        {
            if(layer < Layers.Length)
            {
                Layers[layer] = value; 
            }
        }
    }
}
