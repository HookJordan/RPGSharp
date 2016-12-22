using Engine.Core.Scenes;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public class Game
    {
        // Surface for game to draw on 
        public Surface Screen;

        // Game scenes 
        public string CurrentScene = ""; 
        public Dictionary<string, GameScene> Scenes { get; set; }

        public Game()
        {
            // setup surface 
            Screen = Video.SetVideoMode(Settings.Width, Settings.Height, 32, false, false, false, true);

            // set caption 
            Video.WindowCaption = Settings.WindowTitle;

            // setup drawing events 
            Events.TargetFps = 60;
            Events.Tick += Events_Tick;

            // mouse input events
            Events.MouseButtonUp += Events_MouseButtonUp;
            Events.MouseButtonDown += Events_MouseButtonDown;
            Events.MouseMotion += Events_MouseMotion;

            // keyboard inputs
            Events.KeyboardDown += Events_KeyboardDown;
            Events.KeyboardUp += Events_KeyboardUp;

            // setup scenes
            this.Scenes = new Dictionary<string, Core.Scenes.GameScene>();
            this.Scenes.Add("MainMenu", new Core.Scenes.MainMenu());
            this.Scenes.Add("World", new Core.Scenes.OpenWorld());

            // setup default scene
            this.CurrentScene = "MainMenu";

            // set game in settings file for other classes to interact with 
            Settings.Game = this;
            SdlDotNet.Input.Mouse.ShowCursor = true;
        }

        #region Keyboard
        private void Events_KeyboardUp(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        {
            Scenes[CurrentScene].Keyboard_up(e);
        }

        private void Events_KeyboardDown(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        {
            Scenes[CurrentScene].Keyboard_down(e);
        }
        #endregion

        #region mouse
        private void Events_MouseMotion(object sender, SdlDotNet.Input.MouseMotionEventArgs e)
        {
            Scenes[CurrentScene].MouseMotion(e);
        }

        private void Events_MouseButtonDown(object sender, SdlDotNet.Input.MouseButtonEventArgs e)
        {
            Scenes[CurrentScene].MouseButton_down(e);
        }

        private void Events_MouseButtonUp(object sender, SdlDotNet.Input.MouseButtonEventArgs e)
        {
            Scenes[CurrentScene].MouseButton_up(e);
        }
        #endregion

        // draw frame 
        private void Events_Tick(object sender, TickEventArgs e)
        {
            // clear previous frame 
            Screen.Fill(Color.Black);

            // call draw 
            Scenes[CurrentScene].Draw(Screen);

            // render fps 
            #region Debug
            if (Settings.displayFPS)
            {
                var fps = Content.debugFont.Render("FPS: " + Events.Fps, Color.Yellow);
                // draw the fps on screen
                Screen.Blit(fps, new Point(10, 10));
            }
            #endregion

            // push to fg 
            Screen.Update(); 
        }

        public void Run()
        {
            // run sdl 
            Events.Run();
        }
    }
}
