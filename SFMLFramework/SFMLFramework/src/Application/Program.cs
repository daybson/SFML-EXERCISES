using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLFramework
{
    class Program
    {
        private static Game game;
        public Game Game { get { return game; } }

        static void Main(string[] args)
        {
            game = new Game("SFML.NET GAME");
            game.Start();
        }
    }
}
