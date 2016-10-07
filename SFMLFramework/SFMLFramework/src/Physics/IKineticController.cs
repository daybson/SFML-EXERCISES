using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    public delegate void CollisionResponse(EDirection direction);
    
    /// <summary>
    /// Define a interface de conversão do input em uma força atuante sobre um Rigidbody
    /// </summary>
    public interface IKineticController
    {
        /// <summary>
        /// Executa uma resposta a alguma colisão
        /// </summary>
        CollisionResponse OnCollisionResponse { get; set; }

        /// <summary>
        /// Adiciona uma força de atuação ao corpo
        /// </summary>
        /// <param name="force">vetor da força sendo aplicada</param>
        void AddForce(SFML.System.Vector2f force);
    }
}