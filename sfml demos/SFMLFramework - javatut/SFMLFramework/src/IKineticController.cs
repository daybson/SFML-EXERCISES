using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    /// <summary>
    /// Define a interface de conversão do input em uma força atuante sobre um Rigidbody
    /// </summary>
    public interface IKineticController
    {

        /// <summary>
        /// Adiciona uma força de atuação ao corpo
        /// </summary>
        void AddForce(SFML.System.Vector2f force);
    }
}