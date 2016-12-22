using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using System.Drawing;

namespace Engine.Core.Scenes
{
    public class MainMenu : GameScene
    {
        public void Draw(Surface screen)
        {
            //throw new NotImplementedException();

            var message = Content.debugFont.Render("Press any key to enter the game world...", System.Drawing.Color.Red);
            screen.Blit(message, new Point(240, 240));
            message.Dispose();
        }

        public void Keyboard_down(KeyboardEventArgs e)
        {
            //throw new NotImplementedException();
            Settings.Game.CurrentScene = "World";
        }

        public void Keyboard_up(KeyboardEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void MouseButton_down(MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void MouseButton_up(MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void MouseMotion(MouseMotionEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
