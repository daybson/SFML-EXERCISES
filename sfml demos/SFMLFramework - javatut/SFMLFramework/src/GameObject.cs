using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace SFMLFramework
{
    public class GameObject : Transformable
    {
        protected bool isEnabled;
        protected string name;
        protected int id;


        public GameObject()
        {
        }

        public List<Component> Component = new List<Component>();

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void Start()
        {
        }

        public void Destroy()
        {
        }
    }
}