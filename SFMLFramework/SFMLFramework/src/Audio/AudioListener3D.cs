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


        public AudioListener3D(GameObject root)
        {
            Listener.GlobalVolume = 100;
            Root = root;
            Root.Subscribe(this);
            Listener.Direction = new Vector3f(0, 0, -1);
            Listener.UpVector = new Vector3f(0, 1, 0);
            gizmo = new CircleShape(4);
            gizmo.OutlineThickness = 4;
            gizmo.OutlineColor = Color.Blue;
            gizmo.FillColor = Color.Transparent;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(GameObject value)
        {
            Listener.Position = new Vector3f(value.Position.X, 0, value.Position.Y);
            gizmo.Position = value.Position;
        }

        public void Render(ref RenderWindow window)
        {
            window.Draw(this.gizmo);
        }

        public void Update(float deltaTime) { }
    }
}
