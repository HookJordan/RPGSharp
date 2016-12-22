using SdlDotNet.Graphics;
using SdlDotNet.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Scenes
{
    public interface GameScene
    {
        // tick event 
        void Draw(Surface screen);

        // key board events 
        void Keyboard_up(KeyboardEventArgs e);
        void Keyboard_down(KeyboardEventArgs e);

        // mouse events
        void MouseButton_up(MouseButtonEventArgs e);
        void MouseButton_down(MouseButtonEventArgs e);
        void MouseMotion(MouseMotionEventArgs e); 
    }
}
