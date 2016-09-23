using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CollisionInfo
{
    #region Fields

    private Vector2f depth;
    public Vector2f Depth { get { return depth; } }
    private EDirection direction;
    public EDirection Direction { get { return direction; } }

    #endregion


    #region Public 
    
    /// <summary>
    /// Cria um novo objeto com as informações de colisão
    /// </summary>
    /// <param name="depth">Profundidade de penetração do bound do objeto ativo no bound do objeto passivo (X,Y são convertido para valor absoluto)</param>
    /// <param name="direction">Direção do movimento do objeto ativo</param>
    public CollisionInfo(Vector2f depth, EDirection direction)
    {
        this.direction = direction;
        this.depth = depth;
        //this.depth = new Vector2f(Math.Abs(depth.X), Math.Abs(depth.Y));
    }

    public CollisionInfo(Vector2f depth)
    {
        this.depth = depth;
    }

    override public string ToString()
    {
        return depth.ToString() + " " + direction.ToString();
    }

    #endregion
}
