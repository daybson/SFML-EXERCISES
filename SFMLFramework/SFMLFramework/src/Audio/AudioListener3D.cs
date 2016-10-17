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
            Listener.GlobalVolume = 50;
            Root = root;
            Root.Subscribe(this);
            Listener.Direction = new Vector3f(1, 0.5f, 1);
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
            Listener.Position = new Vector3f(value.Position.X, 0, value.Position.Y);
            gizmo.Position = value.Position;
            //Console.WriteLine("Listener:"+ Listener.Position.ToString());

        }

        public void Render(ref RenderWindow window)
        {
            window.Draw(this.gizmo);
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
