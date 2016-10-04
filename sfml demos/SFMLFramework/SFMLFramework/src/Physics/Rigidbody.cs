﻿using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFMLFramework;

/// <summary>
/// Define um objeto rígido que ocupa um lugar no espaço, passível a movimentação
/// </summary>
public sealed class Rigidbody : IComponent, ICollisionable, IKineticController
{
    /// <summary>
    /// Aceleração atuante no corpo
    /// </summary>
    private Vector2f acceleration;
    
    /// <summary>
    /// Velocidade de movimento do corpo
    /// </summary>
    private Vector2f velocity;
    
    /// <summary>
    /// Deslocamento do corpo em movimento
    /// </summary>
    private Vector2f displacement;
   
    /// <summary>
    /// Massa do corpo
    /// </summary>
    private float mass;
    
    /// <summary>
    /// Tamanho do sprite (width, height)
    /// </summary>
    private Vector2f spriteDimension;

    /// <summary>
    /// Especifica se o corpo deve responder à colisões ou se mantém fiel à sua posição atual (incluindo uma posição ditada por uma animação)
    /// </summary>
    private bool isKinematic;

    /// <summary>
    /// Somatório de todas as forças sendo aplicadas ao corpo
    /// </summary>
    private Vector2f finalForce;
    
    /// <summary>
    /// Força gravitacional atuante sobre o corpo
    /// </summary>
    private Vector2f gravityForce;
   
    /// <summary>
    /// Espessura do collider
    /// </summary>
    private int colliderThickness = 4;

    /// <summary>
    /// Material de que é composto o corpo rígido
    /// </summary>
    public Material Material { get; set; }

    /// <summary>
    /// Collider do topo
    /// </summary>
    public Collider ColliderTop { get; set; }

    /// <summary>
    /// Collider direito
    /// </summary>
    public Collider ColliderRight { get; set; }

    /// <summary>
    /// Collider esquerdo
    /// </summary>
    public Collider ColliderLeft { get; set; }

    /// <summary>
    /// Collider de baixo
    /// </summary>
    public Collider ColliderBottom { get; set; }

    public Vector2f Acceleration { get { return acceleration; } }

    public Vector2f Displacement { get { return displacement; } }

    public Vector2f SpriteDimension { get { return spriteDimension; } }

    public float Mass { get { return mass; } }

    public Vector2f Velocity { get { return velocity; } }

    public PlatformPlayerController PlatformPlayerController { get; set; }

    public bool IsEnabled { get; set; }

    public GameObject Root { get; set; }

    public Rigidbody(Vector2f acceleration, Vector2f displacement, float mass, Vector2f velocity, Vector2f dimension, Material material, bool isKinematic, GameObject root)
    {
        this.mass = mass;
        this.Root = root;
        this.Material = material;
        this.isKinematic = isKinematic;

        this.ColliderTop = new Collider(dimension, EDirection.Up, this.colliderThickness, this.Root);
        this.ColliderBottom = new Collider(dimension, EDirection.Down, this.colliderThickness, this.Root);
        this.ColliderLeft = new Collider(dimension, EDirection.Left, this.colliderThickness, this.Root);
        this.ColliderRight = new Collider(dimension, EDirection.Right, this.colliderThickness, this.Root);

        this.gravityForce = new Vector2f(0, this.mass * Physx.GAcc);
    }

    public void Update(float deltaTime)
    {
        this.finalForce += this.gravityForce;
        this.finalForce *= deltaTime;

        Root.Position += this.finalForce;
        Console.WriteLine("F:" + finalForce.ToString());
    }

    /// <summary>
    /// Resolve a colisão de acordo com as informações da colisão e o estado interno do RigidBody
    /// </summary>
    public void SolveCollision(CollisionInfo hitInfo)
    {
        if (this.isKinematic || Material?.CollisionType == ECollisionType.None)
            return;

        switch (Material.CollisionType)
        {
            case ECollisionType.Elastic:
                #region
                AddMovement(new Vector2f(-hitInfo.Force.X, -hitInfo.Force.Y));
                break;
            #endregion

            case ECollisionType.Inelastic:
                #region
                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        SetPosition(new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height));
                        //this.isFalling = false;
                        //this.isJumping = false;
                        //this.currSpeed.Y = 0;
                        break;
                    case EDirection.Up:
                        SetPosition(new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height));
                        velocity.Y = 0;
                        break;
                    case EDirection.Right:
                        SetPosition(new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y));
                        break;
                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        break;
                }
                break;
            #endregion

            case ECollisionType.PartialInelastic:
                #region
                AddMovement(hitInfo.Force - this.velocity);
                break;
            #endregion

            case ECollisionType.Trigger:
                #region
                break;
                #endregion
        }
    }

    private void AddMovement(Vector2f vector2f)
    {

    }

    public void SetPosition(Vector2f position)
    {
        this.Root.Position = position;
    }

    public void AddForce(Vector2f force)
    {        
        this.finalForce += force;
    }
}