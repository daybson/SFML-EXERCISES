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

        public static SpriteSheet Load(string name)
        {
            return new SpriteSheet(spritePath + name);
        }
    }
}