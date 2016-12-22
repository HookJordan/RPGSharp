using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class Content
    {
        public static SdlDotNet.Graphics.Font debugFont = new SdlDotNet.Graphics.Font(@"Data\font\debug.ttf", 12);

        public static SdlDotNet.Graphics.Font gameFont = new SdlDotNet.Graphics.Font(@"Data\font\debug.ttf", 8); 
    }
}
