using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Scripting
{
    public class Executor
    {
        public Scenes.OpenWorld Game { get; private set; }

        public Executor(Scenes.OpenWorld world)
        {
            // Create script engine for specified game scene 
            this.Game = world; 
        }

        public void runScript(string _script)
        {
            // convert script to list of lines 
            var script = getScriptLines(_script);

            // reusable variables 
            int x, y;

            // loop through each line in the script 
            for(int i = 0; i < script.Count; i++)
            {
                // convert each string line to a structured command line 
                var c = FromLine(script[i]); 

                // based on the commands... 
                switch(c.Command)
                {
                    // jump for loops and just 
                    case "jump":
                        i = Convert.ToInt32(c.Args[0]); //  move iterator to where jumps says 
                        break;
                    // Dim and undim lights in game to simulate night and shades, etc 
                    case "light":
                        if(c.Args[0] == "normal")
                        {
                            Game.Map.DarkMap = false; 
                        }
                        else if (c.Args[0] == "dim")
                        {
                            Game.Map.DarkMap = true; 
                        }
                        break;
                    case "warp":
                        // warp player 
                        x = int.Parse(c.Args[0]);
                        y = int.Parse(c.Args[1]);

                        // update avatar location 
                        Game.Avatar.X = x;
                        Game.Avatar.Y = y;
                        Game.Avatar.TargetX = x;
                        Game.Avatar.TargetY = y;

                        // move camera to follow avatar 
                        Game.updateCamera(); 

                        break;

                    case "map":
                        // change map -- load new map file 
                        Game.Map = World.Map.MapFactory.FromFile(@"Data\maps\" + c.Args[0] + ".map");

                        // set avatar location to the maps default spawning location 
                        Game.Avatar.X = Game.Map.SpawnX;
                        Game.Avatar.Y = Game.Map.SpawnY;
                        Game.Avatar.TargetX = Game.Map.SpawnX;
                        Game.Avatar.TargetY = Game.Map.SpawnY;

                        // move camera to match updated location
                        Game.updateCamera(); 
                        break;

                    case "walk":
                        // set player target location 

                        // check for a valid path to the requested location 
                        var finder = new Sprite.PathFinderv2(Game.Map.getBlocks());
                        x = int.Parse(c.Args[0]);
                        y = int.Parse(c.Args[1]);

                        // find the path if it's possible
                        var points = finder.FindPath(new System.Drawing.Point(Game.Avatar.X, Game.Avatar.Y), new System.Drawing.Point(x, y)); 

                        // if a path was found
                        if(points != null)
                        {
                            // set players path 
                            Game.Avatar.TargetX = x;
                            Game.Avatar.TargetY = y;

                            Game.Avatar.Points = new List<System.Drawing.Point>(); 
                            for(int p = 0; p < points.Count(); p++)
                            {
                                Game.Avatar.Points.Add(new System.Drawing.Point(points[p].X, points[p].Y));
                            }

                            Game.Avatar.nextX = Game.Avatar.Points[0].X;
                            Game.Avatar.nextY = Game.Avatar.Points[0].Y; 
                        }

                        // check for the additional paramter to enable / disable run 
                        if(c.Args.Length >= 3)
                        {
                            if(c.Args[2] == "run")
                            {
                                Game.Avatar.Run = true;
                            }
                            else if (c.Args[2] == "walk")
                            {
                                Game.Avatar.Run = false; 
                            }
                        }

                        break;
                    case "player":
                        if(c.Args[0] == "stamina")
                        {
                            if(c.Args[1] == "increase")
                            {
                                if (Game.Avatar.CurrentStamina < Game.Avatar.Stamina)
                                {
                                    Game.Avatar.CurrentStamina += 0.05;
                                }
                                else if(Game.Avatar.CurrentStamina > Game.Avatar.Stamina)
                                {
                                    Game.Avatar.CurrentStamina = Game.Avatar.Stamina;
                                }
                            }
                            else if(c.Args[1] == "decrease")
                            {
                                if (Game.Avatar.CurrentStamina > 0)
                                {
                                    Game.Avatar.CurrentStamina -= 0.05;
                                }
                            }
                            else if (c.Args[1] == "mod")
                            {
                                int value = int.Parse(c.Args[2]);

                                Game.Avatar.CurrentStamina += value;
                            }
                        }
                        break;
                    default:
                        // unsupport command 
                        break;
                }
            }
        }

        private List<string> getScriptLines(string script)
        {
            if(script.Contains("\n"))
            {
                // Create list to store the script lines in 
                List<string> clean = new List<string>(); 

                // convert the script to a list of commands 
                string[] lines = script.Split('\n'); 

                // loop through each script 
                foreach(string line in lines)
                {
                    clean.Add(line.Replace("\r", "").ToLower().Trim()); 
                }

                return clean; 
            }
            else
            {
                return new List<string>() { script }; // only one line in the script 
            }
        }

        public CommandLine FromLine(string line)
        {
            // check if command contains arguements 
            if (line.Contains(" "))
            {
                // split the command line into peices 
                string[] parts = line.Split(' ');
                
                // create an array to hold the args of the command (length = amount of words on line - 1 for the command itself) 
                string[] args = new string[parts.Length - 1];

                // copy args over 
                for(int i = 1; i < parts.Length; i++)
                {
                    args[i - 1] = parts[i]; 
                }

                return new CommandLine() { Command = parts[0], Args = args };
            }
            else
            {
                // no arguments 
                return new CommandLine() { Command = line, Args = null };
            }

        }
    }

    public struct CommandLine
    {
        public string Command { get; set; }
        public string[] Args { get; set; }
    }
}
