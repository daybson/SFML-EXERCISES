using SFML.Audio;
using SFMLFramework.src.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace SFMLFramework.src.Audio
{
    public class AudioListener3D : IComponent, IObserver<GameObject>, IRender
    {
        public bool IsEnabled { get; set; }
        public GameObject Root { get; set; }

        private CircleShape gizmo;
        private Vector2f direction;


        public AudioListener3D(GameObject root)
        {
            Listener.GlobalVolume = 50;
            Root = root;
            Root.Subscribe(this);
            gizmo = new CircleShape(4);
            gizmo.OutlineThickness = 4;
            gizmo.OutlineColor = Color.Blue;
            gizmo.FillColor = Color.Transparent;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(GameObject value)
        {
            Listener.Position = new SFML.System.Vector3f(value.Position.X, 0, value.Position.Y);
            gizmo.Position = value.Position;
            Listener.Direction = new SFML.System.Vector3f(1, 0, 0);
            this.direction = value.Position;
        }

        public void Render(ref RenderWindow window)
        {
            window.Draw(this.gizmo);
            window.Draw(new Vertex[] { new Vertex(gizmo.Position), new Vertex(V2.Right) }, (uint)0, (uint)2, PrimitiveType.Lines);
        }

        public void Update(float deltaTime)
        {

        }
    }
}
