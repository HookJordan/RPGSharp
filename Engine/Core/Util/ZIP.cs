using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Util
{
    public class ZIP
    {
        public static byte[] Decompress(byte[] zippedData)
        {
            byte[] decompressedData = null;
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (MemoryStream inputStream = new MemoryStream(zippedData))
                {
                    using (GZipStream zip = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zip.CopyTo(outputStream);
                    }
                }
                decompressedData = outputStream.ToArray();
            }

            return decompressedData;
        }

        public static byte[] Compress(byte[] plainData)
        {
            byte[] compressesData = null;
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    zip.Write(plainData, 0, plainData.Length);
                }
                compressesData = outputStream.ToArray();
            }

            return compressesData;
        }
    }
}
