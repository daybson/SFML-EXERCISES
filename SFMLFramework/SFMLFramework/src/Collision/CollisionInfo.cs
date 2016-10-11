using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFMLFramework;

public class CollisionInfo
{
    #region Fields

    private EDirection direction;
    public EDirection Direction { get { return direction; } }

    private FloatRect overlap;
    public FloatRect Overlap { get { return overlap; } }

    private ICollisionable obstacle;
    public ICollisionable RigidBody { get { return obstacle; } }


    #endregion


    #region Public 

    /// <summary>
    /// Cria um novo objeto com as informações de colisão
    /// </summary>
    /// <param name="depth">Profundidade de penetração do bound do objeto ativo no bound do objeto passivo (X,Y são convertido para valor absoluto)</param>
    /// <param name="direction">Direção do movimento do objeto ativo</param>
    /// <param name="obstacle">Corpo com o qual se colidiu</param>
    public CollisionInfo(FloatRect overlap, EDirection direction, ICollisionable obstacle)
    {
        this.direction = direction;
        this.overlap = overlap;
        this.obstacle = obstacle;
    }

    override public string ToString()
    {
        return this.overlap.ToString() + " " + this.direction.ToString();
    }

    #endregion
}
