using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    public class GameObject
    {
        protected bool isEnabled;
        protected string name;
        protected int id;

        public List<Stick> Stick
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}