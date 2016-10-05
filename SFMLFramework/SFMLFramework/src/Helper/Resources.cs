using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    /// <summary>
    /// Realiza os serviços de carregamento de recursos do disco
    /// </summary>
    public class Resources
    {
        private static readonly string spritePath = "resources/textures/";
        private static readonly string fontPath = "resources/fonts/";

        public static SpriteSheet LoadSpriteSheet(string name)
        {
            return new SpriteSheet(spritePath + name);
        }

        public static Font LoadFont(string name)
        {
            return new Font(fontPath + name);
        }
    }
}