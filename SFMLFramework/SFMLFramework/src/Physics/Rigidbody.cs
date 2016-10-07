﻿using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFMLFramework;
using SFMLFramework.src.Helper;

/// <summary>
/// Define um objeto rígido que ocupa um lugar no espaço, passível a movimentação
/// </summary>
public sealed class Rigidbody : IComponent, ICollisionable, IKineticController
{
    #region Fields

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
    /// Coeficiente de restituição do corpo (elasticidade)
    /// </summary>
    private float elasticity;

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

    /// <summary>
    /// Fricção do ambiente com o qual se está colidindo. Default do contrário.
    /// </summary>
    public float EnvironmentFriction { get; set; }

    public Vector2f Acceleration { get { return acceleration; } }

    public Vector2f Displacement { get { return displacement; } }

    public Vector2f SpriteDimension { get { return spriteDimension; } }

    public float Mass { get { return mass; } }

    public Vector2f Velocity { get { return this.finalForce; } }

    public float Elasticity { get { return elasticity; } }

    public PlatformPlayerController PlatformPlayerController { get; set; }

    public bool IsEnabled { get; set; }

    public GameObject Root { get; set; }

    public Vector2f MaxVelocity { get { return new Vector2f(150, 10); } }

    #endregion


    #region Methods

    public Rigidbody(float mass, float elasticity, Vector2f dimension, Material material, bool isKinematic, GameObject root)
    {
        this.mass = mass;
        this.Root = root;
        this.Material = material;
        this.isKinematic = isKinematic;
        this.finalForce = V2.Zero;

        this.elasticity = Extensions.Clamp(elasticity, Physx.MinElasticity, Physx.MaxElasticity);

        this.ColliderTop = new Collider(dimension, EDirection.Up, this.colliderThickness, this.Root);
        this.ColliderBottom = new Collider(dimension, EDirection.Down, this.colliderThickness, this.Root);
        this.ColliderLeft = new Collider(dimension, EDirection.Left, this.colliderThickness, this.Root);
        this.ColliderRight = new Collider(dimension, EDirection.Right, this.colliderThickness, this.Root);

        this.gravityForce = new Vector2f(0, this.mass * Physx.GAcc);
    }

    public void SetPosition(Vector2f position)
    {
        this.Root.Position = position;
    }

    public void Update(float deltaTime)
    {
        // Gravidade
        if(!isKinematic)
        { this.finalForce += this.gravityForce;

        #region Fricção do piso/ambiente
        if (this.finalForce.X > 0)
        {
            this.finalForce.X -= EnvironmentFriction;
            if (this.finalForce.X - EnvironmentFriction < 0)
                this.finalForce.X = 0;
        }
        else if (this.finalForce.X < 0)
        {
            this.finalForce.X += EnvironmentFriction;
            if (this.finalForce.X + EnvironmentFriction > 0)
                this.finalForce.X = 0;
        }
        #endregion

        Root.Position += (this.finalForce) * deltaTime;
        
        //impede overflow
        this.finalForce -= this.gravityForce;
            }
    }

    public void AddForce(Vector2f force)
    {
        this.finalForce += force;
        //this.finalForce.X = Extensions.Clamp(this.finalForce.X, -MaxVelocity.X, MaxVelocity.X);
        //this.finalForce.Y = Extensions.Clamp(this.finalForce.Y, -MaxVelocity.Y, MaxVelocity.Y);
    }

    public void ReduceForce(Vector2f force)
    {
        this.finalForce -= force;
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
                break;
            #endregion

            case ECollisionType.Inelastic:
                #region

                /*
                 * Equação da velocidade final dos corpos em colisão inelástica
                 * https://en.wikipedia.org/wiki/Inelastic_collision
                 * v = (5kg * 4ms + 3kg * 0ms) / 8kg
                 * v = 20 / 8
                 * v = 2.5
                 */

                //a nova velocidade final do sistema em colisão é calculada. Como a massa é constante, a menos que alguma força externa (gravidade/fricção) esteja atuando,
                //a velocidade será sempre a mesma e os corpos nunca cessarão o movimento
                var newVelocity = new Vector2f((this.mass * Velocity.X + hitInfo.RigidBody.Mass * hitInfo.RigidBody.Velocity.X) / (this.mass + hitInfo.RigidBody.Mass), 0);

                //a velocidade a ser incrementada no sistema é a diferença entre a velocidade final do sistema e a força atual do corpo, ou seja, o quanto falta para que
                //o corpo atinja a velocidade final, ou o quanto é reduzido sobre sua velocidade atual para que ele se equilibre ao sistema
                var displacementVelocity1 = newVelocity - this.finalForce;

                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        SetPosition(new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height));
                        //this.isFalling = false;
                        //this.isJumping = false;
                        //this.currSpeed.Y = 0;
                        EnvironmentFriction = hitInfo.RigidBody.Material.Friction;
                        break;
                    case EDirection.Up:
                        SetPosition(new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height));
                        velocity.Y = 0;
                        break;
                    case EDirection.Right:
                        SetPosition(new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y));
                        AddForce(displacementVelocity1);
                        break;
                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        AddForce(displacementVelocity1);
                        break;
                }
                break;
            #endregion

            case ECollisionType.PartialInelastic:
                #region

                /*
                * Equação da velocidade final dos corpos em colisão parcialmente inelástica
                * https://pt.wikipedia.org/wiki/Colis%C3%A3o_inel%C3%A1stica
                * Va = (e * mb * (vb - va) + ma * va + mb * vb) / (ma + mb)
                * Vb = (e * ma * (va - vb) + ma * va + mb * vb) / (ma + mb)
                * 
                * Coeficiente de Restituição (e - elasticity)
                * e = 0: colisão elástica; 
                * e = 1: colisão inelástica; 
                * e > 0 && e < 1: parcialmente inelástica
                * 
                */

                // (0.5 * 10 * ([4,0] - [0,0]) + 3 * [0,0] + 3 * [4,0]) / (3 + 10)
                // (5 * [4,0]) + [0,0] + [12,0]) / 13
                // ([20,0] + [12,0]) / 13
                // [32, 0] / 13
                // Va = [2.46, 0] - ocorre perda de energia

                var Va = (this.elasticity * hitInfo.RigidBody.Mass * (hitInfo.RigidBody.Velocity - this.finalForce) + this.mass * this.finalForce + hitInfo.RigidBody.Mass * hitInfo.RigidBody.Velocity) / (this.mass + hitInfo.RigidBody.Mass);
                var Vb = (hitInfo.RigidBody.Elasticity * this.mass * (this.finalForce - hitInfo.RigidBody.Velocity) + this.mass * this.finalForce + hitInfo.RigidBody.Mass * hitInfo.RigidBody.Velocity) / (this.mass + hitInfo.RigidBody.Mass);

                var newVelocityA = new Vector2f((this.mass * Va.X + hitInfo.RigidBody.Mass * Vb.X) / (this.mass + hitInfo.RigidBody.Mass), 0);

                var displacementVelocity2 = newVelocityA - this.finalForce;
                displacementVelocity2 = new Vector2f(Math.Abs(displacementVelocity2.X), Math.Abs(displacementVelocity2.Y));

                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        SetPosition(new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height));
                        break;

                    case EDirection.Up:
                        SetPosition(new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height));
                        break;

                    case EDirection.Right:
                        SetPosition(new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y));
                        this.finalForce.X = 0;
                        AddForce(displacementVelocity2 * V2.Left.X);
                        Console.WriteLine("Reflection force R: " + (displacementVelocity2 * V2.Left.X).ToString());
                        break;

                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        this.finalForce.X = 0;
                        AddForce(new Vector2f(displacementVelocity2.X, displacementVelocity2.Y));
                        Console.WriteLine("Reflection force L: " + (displacementVelocity2 * V2.Left.X).ToString());
                        break;
                }

                break;
            #endregion

            case ECollisionType.Trigger:
                #region
                break;
                #endregion
        }
    }

    #endregion
}