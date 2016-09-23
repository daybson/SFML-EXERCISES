//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : SFML Framework
//  @ File Name : Mover.cs
//  @ Date : 13/09/2016
//  @ Author : Daybson B. S. Paisante <daybson.paisante@outlook.com>
//
//

using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;


public class Mover : Component, IMove
{
    public enum EDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    #region Fields

    public delegate void OnDirectionChange(EDirection direction);

    private bool moveLeft;
    private bool moveRigth;
    private bool moveUp;
    private bool moveDown;

    public Vector2f Speed;
    protected Vector2f move;
    public Vector2f Position { get; set; }
    public EDirection Direction { get; set; }
    public OnDirectionChange OnChangeDirection { get; set; }

    #endregion


    #region Public

    public Mover()
    {
        this.enabled = true;
    }

    public override void Update(float deltaTime)
    {
        this.move = new Vector2f();

        if (this.moveLeft)
            this.move.X = -this.Speed.X * deltaTime;

        if (this.moveRigth)
            this.move.X = this.Speed.X * deltaTime;

        if (this.moveUp)
            this.move.Y = -this.Speed.Y * deltaTime;

        if (this.moveDown)
            this.move.Y = this.Speed.Y * deltaTime;

        this.Position += this.move;
    }

    /// <summary>
    /// Se n�o estiver movendo em nenhuma outra dire��o, atribui o novo sentido de movimento ou cancela o movimento atual
    /// </summary>
    /// <param name="direction">Dire��o do movimento</param>
    /// <param name="value">Valor habilita/desabilita movimento</param>
    public void SetDirectionMove(EDirection direction, bool value)
    {
        if (value && (this.moveDown || this.moveLeft || this.moveRigth || this.moveUp))
            return;

        switch (direction)
        {
            case EDirection.Left:
                this.moveLeft = value;
                if (value)
                {
                    this.Direction = EDirection.Left;
                    this.moveRigth = !value;
                }
                break;
            case EDirection.Right:
                this.moveRigth = value;
                if (value)
                {
                    this.Direction = EDirection.Right;
                    this.moveLeft = !value;
                }
                break;
            case EDirection.Up:
                this.moveUp = value;
                if (value)
                {
                    this.Direction = EDirection.Up;
                    this.moveDown = !value;
                }
                break;
            case EDirection.Down:
                this.moveDown = value;
                if (value)
                {
                    this.Direction = EDirection.Down;
                    this.moveUp = !value;
                }
                break;
            default:
                this.Direction = EDirection.None;
                this.moveLeft = false;
                this.moveRigth = false;
                this.moveUp = false;
                this.moveDown = false;
                break;
        }

        if (value)
            this.OnChangeDirection(direction);
    }

    public void ApplyMovement(Vector2f movement, EDirection direction)
    {
        this.move += movement;
        this.Position += this.move;
        this.Direction = direction;
    }

    #endregion
}