using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    /// <summary>
    /// Determina um tipo de material a ser usado por um corpo rígido
    /// </summary>
    public sealed class Material
    {
        /// <summary>
        /// Nome do tipo de material (borracha, metal, etc.)
        /// </summary>
        private string name;
        /// <summary>
        /// Fricção do material (resistência a movimento)
        /// </summary>
        private int friction;
        /// <summary>
        /// Permite uma variação da elasticidade do material.
        /// </summary>
        private int bounciness;
        /// <summary>
        /// Densidade do material
        /// </summary>
        private float density;

        public ECollisionType ECollisionType
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}