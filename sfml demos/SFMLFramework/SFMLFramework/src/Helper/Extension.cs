using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src.Helper
{
    public static class Extension
    {
        public static Vector2f Left
        {
            get { return new Vector2f(-1, 0); }
        }

        public static Vector2f Right
        {
            get { return new Vector2f(1, 0); }
        }

        public static Vector2f Top
        {
            get { return new Vector2f(0, -1); }
        }

        public static Vector2f Bottom
        {
            get { return new Vector2f(0, 1); }
        }

        public static Vector2f Zero
        {
            get { return new Vector2f(0, 0); }
        }

        public static Vector2f One
        {
            get { return new Vector2f(1, 1); }
        }

        public static Vector2f Negative
        {
            get { return new Vector2f(-1, -1); }
        }
    }
}
