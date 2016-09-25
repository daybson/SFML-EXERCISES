using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

public class CollisionInfo
{
    #region Fields

    private EDirection direction;
    public EDirection Direction { get { return direction; } }

    private FloatRect overlap;
    public FloatRect Overlap { get { return overlap; } }

    #endregion


    #region Public 

    /// <summary>
    /// Cria um novo objeto com as informações de colisão
    /// </summary>
    /// <param name="depth">Profundidade de penetração do bound do objeto ativo no bound do objeto passivo (X,Y são convertido para valor absoluto)</param>
    /// <param name="direction">Direção do movimento do objeto ativo</param>
    public CollisionInfo(FloatRect overlap, EDirection direction)
    {
        this.direction = direction;
        this.overlap = overlap;
    }

    override public string ToString()
    {
        return this.overlap.ToString() + " " + this.direction.ToString();
    }

    public CollisionInfo Inverse()
    {
        var inverseCollision = this;
        switch(inverseCollision.direction)
        {
            case EDirection.Botton:
                inverseCollision.direction = EDirection.Top;
                break;
            case EDirection.Top:
                inverseCollision.direction = EDirection.Botton;
                break;
            case EDirection.Right:
                inverseCollision.direction = EDirection.Left;
                break;
            case EDirection.Left:
                inverseCollision.direction = EDirection.Right;
                break;
        }
        return inverseCollision;
    }

    #endregion
}
