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
    public interface IComponent
    {
        /// <summary>
        /// Componente está habilidado? (somente executa caso esteja)
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// GameObject ao qual o componente está relacionado
        /// </summary>
        GameObject Root { get; set; }

        /// <summary>
        /// Atualiza o componente a cada frame
        /// </summary>
        /// <param name="deltaTime">Tempo decorrido desde a última atualização</param>
        void Update(float deltaTime);
    }
}