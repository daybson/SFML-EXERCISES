using SFML.System;
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
    private Vector2f netForce;

    /// <summary>
    /// Espessura do collider
    /// </summary>
    private int colliderThickness;

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

    /// <summary>
    /// Delegate de reação para ocorrência de colisões
    /// </summary>
    public CollisionResponse OnCollisionResponse { get; set; }

    /// <summary>
    /// Dimensão do sprite
    /// </summary>
    public Vector2f SpriteDimension { get { return spriteDimension; } }

    /// <summary>
    /// Massa do corpo
    /// </summary>
    public float Mass { get { return mass; } }

    /// <summary>
    /// Velocidade de movimento atual do corpo. Internamente, tratada como força net.
    /// </summary>
    public Vector2f Velocity { get { return this.netForce; } }

    /// <summary>
    /// Controlador de movimento que atua sobre o corpo (mock para NPCs)
    /// </summary>
    public PlatformPlayerController PlatformPlayerController { get; set; }

    public bool IsEnabled { get; set; }

    public GameObject Root { get; set; }

    public Vector2f MaxVelocity { get { return new Vector2f(150, -500); } }

    /// <summary>
    /// Taxa de cálculo da espessura do collider (80% da menor medida entre altura e largura)
    /// </summary>
    private static readonly float COLLIDER_THICKNESS_RATIO = 0.8f;

    #endregion


    #region Methods

    public Rigidbody(float mass, float elasticity, Vector2f spriteDimension, Material material, bool isKinematic, GameObject root)
    {
        this.mass = mass;
        this.Root = root;
        this.Material = material;
        this.isKinematic = isKinematic;
        this.netForce = V2.Zero;
        this.spriteDimension = spriteDimension;

        this.colliderThickness = (int)(Math.Min(this.spriteDimension.X, this.spriteDimension.Y) * COLLIDER_THICKNESS_RATIO);

        this.ColliderTop = new Collider(this.spriteDimension, EDirection.Up, this.colliderThickness, this.Root);
        this.ColliderBottom = new Collider(this.spriteDimension, EDirection.Down, this.colliderThickness, this.Root);
        this.ColliderLeft = new Collider(this.spriteDimension, EDirection.Left, this.colliderThickness, this.Root);
        this.ColliderRight = new Collider(this.spriteDimension, EDirection.Right, this.colliderThickness, this.Root);

        OnCollisionResponse += (x) => { };
    }

    /// <summary>
    /// Transforma o corpo para uma posição imediatamente, sem afetar a força net
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector2f position)
    {
        this.Root.Position = position;
    }

    public void Update(float deltaTime)
    {
        //G-FORCE
        if (!isKinematic && this.netForce.Y < 0)
            this.netForce += new Vector2f(0, this.mass * Physx.GAcc);

        #region Fricção do piso/ambiente
        if (Material.CollisionType != ECollisionType.Elastic)
        {
            if (this.netForce.X > 0)
            {
                this.netForce.X -= EnvironmentFriction;
                if (this.netForce.X - EnvironmentFriction < 0)
                    this.netForce.X = 0;
            }
            else if (this.netForce.X < 0)
            {
                this.netForce.X += EnvironmentFriction;
                if (this.netForce.X + EnvironmentFriction > 0)
                    this.netForce.X = 0;
            }
        }
        #endregion

        Root.Position += this.netForce * deltaTime;
    }

    /// <summary>
    /// Soma uma força à força net atuante no momento sobre o corpo
    /// </summary>
    /// <param name="force">Vetor de força à ser somado à força net</param>
    public void AddForce(Vector2f force)
    {
        this.netForce += force;

        //TODO: definir impacto do uso ou não de velocidade máxima
        //this.finalForce.X = Extensions.Clamp(this.finalForce.X, -MaxVelocity.X, MaxVelocity.X);
        //this.finalForce.Y = Extensions.Clamp(this.finalForce.Y, -MaxVelocity.Y, MaxVelocity.Y);
    }

    /// <summary>
    /// Resolve a colisão de acordo com as informações da colisão e o estado interno do RigidBody
    /// </summary>
    public void SolveCollision(CollisionInfo hitInfo)
    {
        if (this.isKinematic || Material?.CollisionType == ECollisionType.None)
            return;

        OnCollisionResponse(hitInfo.Direction);

        switch (Material.CollisionType)
        {
            case ECollisionType.Elastic:
                #region

                #region Doc
                /*
                * Equação da velocidade final dos corpos em colisão elástica
                *http://www.real-world-physics-problems.com/elastic-collision.html
                * Ei = Ef
                * Ei = (0.5 * ma * vai * vai) + (0.5 * mb * vbi * vbi) 
                * Ef = (0.5 * ma * vaf * vaf) + (0.5 * mb * vbf * vbf)                
                * 
                * Vaf = ((ma - mb) / (ma + mb)) * Vai  +  2 * mb / (ma + mb) * Vbi
                * Vbf = 2 * ma / (ma + mb) * Vai  +  ((mb - ma) / (ma + mb)) * Vbi                  

                var Vaf = ((this.mass - hitInfo.RigidBody.Mass) / (this.mass + hitInfo.RigidBody.Mass)) * this.finalForce.X + 2 * hitInfo.RigidBody.Mass / (this.mass + hitInfo.RigidBody.Mass) * hitInfo.RigidBody.Velocity.X;
                var Vbf = 2 * this.mass / (this.mass + hitInfo.RigidBody.Mass) * this.finalForce.X + ((hitInfo.RigidBody.Mass - this.mass) / (this.mass + hitInfo.RigidBody.Mass)) * hitInfo.RigidBody.Velocity.X;

                var Ei = (0.5 * this.mass * this.finalForce.X * this.finalForce.X) + (0.5 * hitInfo.RigidBody.Mass * hitInfo.RigidBody.Velocity.X * hitInfo.RigidBody.Velocity.X);
                var Ef = (0.5 * this.mass * Vaf * Vaf) + (0.5 * hitInfo.RigidBody.Mass * Vbf * Vbf);
                */
                #endregion

                var previousNetForce = this.netForce;

                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        SetPosition(new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height));
                        this.netForce.Y = 0;
                        AddForce(new Vector2f(previousNetForce.X, -Math.Abs(previousNetForce.Y)));
                        break;
                    case EDirection.Up:
                        SetPosition(new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height));
                        this.netForce.Y = 0;
                        AddForce(new Vector2f(previousNetForce.X, Math.Abs(previousNetForce.Y)));
                        break;
                    case EDirection.Right:
                        SetPosition(new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y));
                        this.netForce.X = 0;
                        AddForce(new Vector2f(-Math.Abs(previousNetForce.X), previousNetForce.Y));
                        break;
                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        this.netForce.X = 0;
                        AddForce(new Vector2f(Math.Abs(previousNetForce.X), previousNetForce.Y));
                        break;
                }
                break;
            #endregion

            case ECollisionType.Inelastic:
                #region

                #region Doc
                /*
                 * Equação da velocidade final dos corpos em colisão inelástica
                 * https://en.wikipedia.org/wiki/Inelastic_collision
                 * v = (5kg * 4ms + 3kg * 0ms) / 8kg
                 * v = 20 / 8
                 * v = 2.5
                 */
                #endregion
                
                //TODO: Calcular para Y

                //a nova velocidade final do sistema em colisão é calculada. Como a massa é constante, a menos que alguma força externa (gravidade/fricção) esteja atuando,
                //a velocidade será sempre a mesma e os corpos nunca cessarão o movimento
                var newVelocity = new Vector2f((this.mass * Velocity.X + hitInfo.RigidBody.Mass * hitInfo.RigidBody.Velocity.X) / (this.mass + hitInfo.RigidBody.Mass), 0);

                //a velocidade a ser incrementada no sistema é a diferença entre a velocidade final do sistema e a força atual do corpo, ou seja, o quanto falta para que
                //o corpo atinja a velocidade final, ou o quanto é reduzido sobre sua velocidade atual para que ele se equilibre ao sistema
                var displacementVelocityX = newVelocity - this.netForce;

                this.OnCollisionResponse(hitInfo.Direction);

                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        SetPosition(new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height));
                        EnvironmentFriction = hitInfo.RigidBody.Material.Friction;
                        break;
                    case EDirection.Up:
                        SetPosition(new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height));
                        break;
                    case EDirection.Right:
                        SetPosition(new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y));
                        AddForce(displacementVelocityX);
                        break;
                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        AddForce(displacementVelocityX);
                        break;
                }
                break;
            #endregion

            case ECollisionType.PartialInelastic:
                #region
                #region Doc
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
                #endregion

                //TODO: Calcular para Y

                var Va = (Material.Bounciness * hitInfo.RigidBody.Mass * (hitInfo.RigidBody.Velocity - this.netForce) + this.mass * this.netForce + hitInfo.RigidBody.Mass * hitInfo.RigidBody.Velocity) / (this.mass + hitInfo.RigidBody.Mass);
                var Vb = (hitInfo.RigidBody.Material.Bounciness * this.mass * (this.netForce - hitInfo.RigidBody.Velocity) + this.mass * this.netForce + hitInfo.RigidBody.Mass * hitInfo.RigidBody.Velocity) / (this.mass + hitInfo.RigidBody.Mass);

                var newVelocityA = new Vector2f((this.mass * Va.X + hitInfo.RigidBody.Mass * Vb.X) / (this.mass + hitInfo.RigidBody.Mass), 0);

                var displacementVelocity2 = newVelocityA - this.netForce;
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
                        this.netForce.X = 0;
                        AddForce(displacementVelocity2 * V2.Left.X);
                        break;

                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        this.netForce.X = 0;
                        AddForce(new Vector2f(displacementVelocity2.X, displacementVelocity2.Y));
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