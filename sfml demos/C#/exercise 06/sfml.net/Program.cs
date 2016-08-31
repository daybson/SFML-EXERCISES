using sfml.net.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfml.net
{
    class Program
    {
        private static Game game;

        static void Main(string[] args)
        {
            game = new Game("SFML.NET GAME");
            game.Run();
        }
    }
}
