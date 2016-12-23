using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.World.Map
{
    public class MapFactory
    {

        public static WorldMap FromFile(string path)
        {
            if(!File.Exists(path))
            {
                throw new Exception("Map file not found!"); 
            }
            else
            {
                using (MemoryStream ms = new MemoryStream(Core.Util.ZIP.Decompress(File.ReadAllBytes(path))))
                {
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        // read header into new map object 
                        WorldMap map = new WorldMap(br.ReadInt32(), br.ReadInt32());

                        // now we read the tiles
                        for (int x = 0; x < map.Width; x++)
                        {
                            for(int y= 0; y < map.Height; y++)
                            {
                                //map.Tiles[x, y] = new MapTile();

                                // properties 
                                map.Tiles[x, y].Blocked = br.ReadBoolean();
                                map.Tiles[x, y].TriggerId = br.ReadInt32(); 

                                // layers 
                                for(int z = 0; z < map.Tiles[x, y].Layers.Length; z++)
                                {
                                    map.Tiles[x, y].Layers[z] = br.ReadInt32();
                                }
                            }
                        }
                    }
                }
            }
            return null; 
        }

        // for names sake :D 
        public static WorldMap ImportMap(string path)
        {
            return FromFile(path); 
        }

        public static void ExportMap(WorldMap map, string path)
        {
            // create a memory stream to write to 
            using (MemoryStream ms = new MemoryStream())
            {
                // create a binary writer to write with 
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    // map header information 
                    bw.Write(map.Width);
                    bw.Write(map.Height);

                    // loop through the map and save all the tiles 
                    for(int x = 0; x < map.Width; x++)
                    {
                        for(int y = 0; y < map.Height; y++)
                        {
                            // write the properties 
                            bw.Write(map.Tiles[x, y].Blocked);
                            bw.Write(map.Tiles[x, y].TriggerId); 

                            // write the layers 
                            for(int z = 0; z < map.Tiles[x, y].Layers.Length; z++)
                            {
                                bw.Write(map.Tiles[x, y].Layers[z]); 
                            }
                        }
                    }
                }

                // all tile data is in ms
                // save compressed ms to path
                File.WriteAllBytes(path, Core.Util.ZIP.Compress(ms.ToArray())); 
            }
        }
    }
}
