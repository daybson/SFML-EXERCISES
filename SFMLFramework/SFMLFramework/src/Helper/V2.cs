using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src.Helper
{
    /// <summary>
    /// Classe com referências para vetores comumente usados
    /// </summary>
    public static class V2
    {
        public static readonly Vector2f Left = new Vector2f(-1, 0);
        
        public static readonly Vector2f Right = new Vector2f(1, 0);

        public static readonly Vector2f Top = new Vector2f(0, -1);

        public static readonly Vector2f Bottom = new Vector2f(0, 1);

        public static readonly Vector2f Zero = new Vector2f(0, 0);

        public static readonly Vector2f One = new Vector2f(1, 1);

        public static readonly Vector2f Negative = new Vector2f(-1, -1);
    }
}