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
        private Particles.Emitters.Fountain fountain;
        private List<Particles.Emitters.Fountain> fountains;// = new List<Particles.Emitters.Fountain>();
        List<Particles.Emitters.Explosion> Explositions = new List<Particles.Emitters.Explosion>(); 
        public void Draw(Surface screen)
        {
            //throw new NotImplementedException();

            var message = Content.debugFont.Render("Press any key to enter the game world...Don't Mind the random particle testing...", System.Drawing.Color.Red);
            screen.Blit(message, new Point(100, 100));
            message.Dispose();

            #region TestZone
            if(fountains == null)
            {
                fountains = new List<Particles.Emitters.Fountain>();
                fountains.Add(new Particles.Emitters.Fountain(50, 300, Color.Magenta, Color.MediumPurple));
                fountains.Add(new Particles.Emitters.Fountain(screen.Width - 50, 300, Color.Magenta, Color.Purple)); 
            }

            foreach (var f in fountains)
            {
                f.Draw(screen);
                f.Update();
            }

                //if(fountain == null)
                //{
                //    fountain = new Particles.Emitters.Fountain(50, 500, Color.LightBlue, Color.Blue);
                //}

                //fountain.Draw(screen);
                //fountain.Update();


            int count = Explositions.Count;
            for (int i = 0; i < count && i > -1; i++)
            {
                Explositions[i].Draw(screen);
                if (!Explositions[i].Update())
                {
                    Explositions.RemoveAt(i);
                    i--;
                    count = Explositions.Count;
                }
            }
            #endregion
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
            Explositions.Add(new Particles.Emitters.Explosion(e.X, e.Y)); //, Color.Red));
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
