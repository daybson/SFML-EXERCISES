using SFML.System;
using SFMLFramework;

/// <summary>
/// Define uma interface para objetos que sofram colisão
/// </summary>
public interface ICollisionable
{
    /// <summary>
    /// Material do corpo
    /// </summary>
    Material Material { get; }

    /// <summary>
    /// Fricção obtida do ambiente
    /// </summary>
    float EnvironmentFriction { get; set; }

    /// <summary>
    /// Collider do topo
    /// </summary>
    Collider ColliderTop { get; }

    /// <summary>
    /// Collider da base
    /// </summary>
    Collider ColliderBottom { get; }

    /// <summary>
    /// Collider da direita
    /// </summary>
    Collider ColliderRight { get; }

    /// <summary>
    /// Collider da esqueda
    /// </summary>
    Collider ColliderLeft { get; }

    /// <summary>
    /// Velocidade inicial antes da resolução de uma colisão
    /// </summary>
    Vector2f InitialVelocity { get; }

    /// <summary>
    /// Velocidade atual do corpo (somatório de todas as forças atuantes)
    /// </summary>
    Vector2f Velocity { get; }

    /// <summary>
    /// Velocidade máxima do corpo
    /// </summary>
    Vector2f MaxVelocity { get; }

    /// <summary>
    /// Massa do corpo
    /// </summary>
    float Mass { get; }

    GameObject Root { get; }

    /// <summary>
    /// Define se um corpo sofre ação de forças externas e internas ou se é estático na tela
    /// </summary>
    bool IsKinematic { get; }

    /// <summary>
    /// Propriedade física de movimento definida pelo produto da Velocidade x Massa
    /// </summary>
    Vector2f Momentum { get; }

    /// <summary>
    /// Define um método executado em resposta a uma colisão sofrida
    /// </summary>
    /// <param name="hitInfo">Objeto com informações da colisão</param>
    void SolveCollision(CollisionInfo hitInfo);

    /// <summary>
    /// Soma uma força ao corpo, que é ajustada no limite -MaxVelocity e +MaxVelocity
    /// </summary>
    /// <param name="netForce">Nova força atuante no corpo</param>
    void AddForce(Vector2f force);

    /// <summary>
    /// Atribui uma nova força ao corpo, que é ajustada no limite -MaxVelocity e +MaxVelocity
    /// </summary>
    /// <param name="netForce">Nova força atuante no corpo</param>
    void SetForce(Vector2f netForce);
}
