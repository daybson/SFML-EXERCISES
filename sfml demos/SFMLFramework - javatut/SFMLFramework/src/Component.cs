using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    public abstract class Component
    {
        private int isEnabled;

        public GameObject Root
        {
            get;
            set;
        }

        public virtual void Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}