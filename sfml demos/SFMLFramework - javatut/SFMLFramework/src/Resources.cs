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
        public static SpriteSheet Load(string path)
        {
            return new SpriteSheet(path);
            //this.iMove.OnChangeDirection += this.spriteSheet.SetDirection;            
        }
    }
}