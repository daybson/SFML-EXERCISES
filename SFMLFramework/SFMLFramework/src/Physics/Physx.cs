using SFMLFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Contém informações relativas a simulação de física do jogo
/// </summary>
public static class Physx
{
    /// <summary>
    /// Força fravitacional
    /// </summary>
    public static readonly float Gravity = 10f;

    /// <summary>
    /// Fricção do ar
    /// </summary>
    public static readonly float Air = 5.0f;

    /// <summary>
    /// Fricção da água
    /// </summary>
    public static readonly float Water = 9.5f;

    /// <summary>
    /// Elasticidade mínima de um material
    /// </summary>
    public static readonly float MinElasticity = 0f;

    /// <summary>
    ///  Elasticidade máxima de um material
    /// </summary>
    public static readonly float MaxElasticity = 1.0f;
}

