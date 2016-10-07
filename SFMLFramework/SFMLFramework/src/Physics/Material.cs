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
        #region Fields

        /// <summary>
        /// Nome do tipo de material (borracha, metal, etc.)
        /// </summary>
        private string name;
        public string Name { get { return name; } }

        /// <summary>
        /// Fricção do material (resistência a movimento)
        /// </summary>
        private float friction;
        public float Friction { get { return friction; } }

        /// <summary>
        /// Coeficiente de restituição do corpo (elasticidade)
        /// </summary>
        private float bounciness;
        public float Bounciness { get { return bounciness; } }

        /// <summary>
        /// Densidade do material
        /// </summary>
        private float density;
        public float Density { get { return density; } }

        /// <summary>
        /// Tipo da resposta de colisão do material
        /// </summary>
        public ECollisionType collisionType;
        public ECollisionType CollisionType { get { return collisionType; } }


        #endregion


        #region Public

        public Material(string name, float friction, float bounciness, float density, ECollisionType collisionType)
        {
            this.name = name;
            this.friction = friction;
            this.bounciness = Extensions.Clamp(bounciness, Physx.MinElasticity, Physx.MaxElasticity);
            this.density = density;
            this.collisionType = collisionType;
        }

        #endregion
    }
}