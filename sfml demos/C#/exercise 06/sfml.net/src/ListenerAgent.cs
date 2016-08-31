using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sfml.net.src
{
    class ListenerAgent
    {
        #region Fields

        private CircleShape gizmo;
        public CircleShape Gizmo { get { return gizmo; } }

        #endregion


        #region Public

        public ListenerAgent(Vector2f position)
        {
            gizmo = new CircleShape(4, 3);
            gizmo.OutlineThickness = 4;
            gizmo.OutlineColor = Color.Blue;
            gizmo.FillColor = Color.Transparent;

            SetPosition(position);
        }
        
        public void SetPosition(Vector2f position)
        {
            Listener.Position = new Vector3f(position.X, 0, position.Y);
            gizmo.Position = new Vector2f(position.X, position.Y);
        }

        public void Update(Player player)
        {
            SetPosition(player.SpriteSheet.Sprite.Position);
            Listener.Direction = new Vector3f(1, 0, 0);
        }

        #endregion
    }
}
