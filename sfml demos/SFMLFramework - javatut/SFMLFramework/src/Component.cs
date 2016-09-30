using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    /// <summary>
    /// Classe abstrata que define um comportamento/funcionalidade para um game object. 
    /// Um componente é um termo genérico para um objeto que executa determinada tarefa relacionada a um GameObject,
    /// porém com implementação desacoplada ao mesmo.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Componente está habilidado? (somente executa caso esteja)
        /// </summary>
        private int isEnabled;

        /// <summary>
        /// GameObject ao qual o componente está relacionado
        /// </summary>
        public GameObject Root
        {
            get;
            set;
        }

        public virtual void Update(float deltaTime)
        {
        }
    }
}