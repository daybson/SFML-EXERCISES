using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;

namespace SFMLFramework
{
    /// <summary>
    /// Delegate de métodos que devam ser executados ao ocorrer alguma colisão
    /// </summary>
    /// <param name="direction">Direção da colisão</param>
    public delegate void CollisionResponse(EDirection direction);
    
    /// <summary>
    /// Define a interface de conversão do input em uma força atuante sobre um Rigidbody
    /// </summary>
    public interface IKineticController
    {
        /// <summary>
        /// Armazena os métodos a serem executados em resposta a alguma colisão
        /// </summary>
        CollisionResponse OnCollisionResponse { get; set; }

        /// <summary>
        /// Adiciona uma força de atuação ao corpo
        /// </summary>
        /// <param name="force">vetor da força sendo aplicada</param>
        void AddForce(Vector2f force);

        /// <summary>
        /// Substitui a força atual no corpo pela informada
        /// </summary>
        /// <param name="vector2f">Nova força atuante no corpo</param>
        void SetForce(Vector2f vector2f);
    }
}