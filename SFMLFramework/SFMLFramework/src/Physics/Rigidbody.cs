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
    private Vector2i spriteDimension;

    /// <summary>
    /// Especifica se o corpo deve responder à colisões ou se mantém fiel à sua posição atual (incluindo uma posição ditada por uma animação)
    /// </summary>
    private bool isKinematic;
    public bool IsKinematic { get { return isKinematic; } }

    /// <summary>
    /// Somatório de todas as forças sendo aplicadas ao corpo
    /// </summary>
    private Vector2f netForce;
    public Vector2f NetForce { get { return netForce; } }

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
    public Vector2i SpriteDimension { get { return spriteDimension; } }

    /// <summary>
    /// Massa do corpo
    /// </summary>
    public float Mass { get { return mass; } }

    /// <summary>
    /// Velocidade de movimento atual do corpo. Internamente, tratada como força net.
    /// </summary>
    public Vector2f Velocity { get { return this.netForce; } }

    protected Vector2f initialVelocity;
    public Vector2f InitialVelocity { get { return this.initialVelocity; } }

    /// <summary>
    /// Controlador de movimento que atua sobre o corpo (mock para NPCs)
    /// </summary>
    public PlatformPlayerController PlatformPlayerController { get; set; }

    private Vector2f momentum;
    public Vector2f Momentum { get { return momentum; } }

    public bool IsEnabled { get; set; }

    public GameObject Root { get; set; }

    private Vector2f maxVelocity;
    public Vector2f MaxVelocity { get { return maxVelocity; } }

    #endregion


    #region Methods

    public Rigidbody(float mass, Vector2i spriteDimension, Material material, bool isKinematic, GameObject root, Vector2f maxVelocity)
    {
        this.mass = mass;
        this.Root = root;
        this.Material = material;
        this.isKinematic = isKinematic;
        this.netForce = V2.Zero;
        this.spriteDimension = spriteDimension;
        this.colliderThickness = 6;
        this.maxVelocity = maxVelocity;
        this.IsEnabled = true;

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
        //Gravidade
        if (!this.isKinematic)
            AddForce(new Vector2f(0, this.mass * Physx.Gravity));

        #region Atrito

        /** No mundo real, embora a energia do sistema ELÁSTICO permacena a mesma após a colisão, 
         * a velocidade diminui decorrente da ação de atrito, sendo convertida para calor e vibração do ar.
         * Aqui, entretanto, o bouciness do material definirá o quanto do atrito do ambiente será aplicado 
         * como resistência ao movimento do objeto, inversamente proporcional. 
         * Assim, um material elástico com bounciness 1 recebe 0 atrito.
        */

        var materialFriction = EnvironmentFriction * Math.Abs(1 - this.Material.Bounciness);

        if (materialFriction != 0)
        {
            if (this.netForce.X > 0)
            {
                this.netForce.X -= materialFriction;
                if (this.netForce.X - materialFriction < 0)
                    this.netForce.X = 0;
            }
            else if (this.netForce.X < 0)
            {
                this.netForce.X += materialFriction;
                if (this.netForce.X + materialFriction > 0)
                    this.netForce.X = 0;
            }
        }
        #endregion

        //Cache do momentum do corpo
        this.momentum = this.mass * netForce;

        //Atualiza a posição via Property para atualizar os componentes inscritos em sua modificação
        Root.Position += this.netForce * deltaTime;

        Console.WriteLine(Root.Position.ToString());

        //exibe velocidade atual no label
        var label = Root.GetComponent<UIText>();
        if (label != null)
            label.Display.Invoke(this.netForce);
    }

    /// <summary>
    /// Soma uma força à força net atuante no momento sobre o corpo
    /// </summary>
    /// <param name="force">Vetor de força à ser somado à força net</param>
    public void AddForce(Vector2f force)
    {
        if (this.isKinematic)
            return;

        this.netForce += force;
        ClampForce();
    }

    /// <summary>
    /// Resolve a colisão de acordo com as informações da colisão e o estado interno do RigidBody
    /// </summary>
    public void SolveCollision(CollisionInfo hitInfo)
    {
        //Corpos cinemáticos não sofrem ação de forças
        if (this.isKinematic)
            return;

        this.initialVelocity = this.netForce;

        //Executa métodos inscritos no evento de colisão
        OnCollisionResponse(hitInfo.Direction);

        //Responde a colisão de acordo com o material do rigidbody
        switch (Material.CollisionType)
        {
            #region ELASTICA
            case ECollisionType.Elastic:
                /** A colisão é denominada elástica quando ocorre conservação da energia e do momento linear dos corpos envolvidos. 
                 * A principal característica desse tipo de colisão é que, após o choque, a velocidade das partículas muda de direção,
                 *  mas a velocidade relativa entre os dois corpos mantém-se igual. 
                 *  http://mundoeducacao.bol.uol.com.br/fisica/colisoes-elasticas-inelasticas.htm
                 * 
                 * Equação da velocidade final dos corpos em colisão elástica
                 * http://www.real-world-physics-problems.com/elastic-collision.html
                 * 
                 * Vaf = ((ma - mb) / (ma + mb)) * Vai  +  2 * mb / (ma + mb) * Vbi
                 * Vbf = 2 * ma / (ma + mb) * Vai  +  ((mb - ma) / (ma + mb)) * Vbi                  
                 */

                //TODO: rever cálculo: forças diferentes colidindo estão se igualando para a maior força...

                if (hitInfo.RigidBody.Material.collisionType == ECollisionType.Elastic)
                    Console.Write("");

                var sumMasses = this.mass + hitInfo.RigidBody.Mass;

                var myStep1 = Math.Abs(((this.mass - hitInfo.RigidBody.Mass) / sumMasses)) * this.initialVelocity;
                var myStep2 = Math.Abs(2 * hitInfo.RigidBody.Mass / sumMasses) * hitInfo.RigidBody.InitialVelocity;
                var finalElasticVelocity = new Vector2f(myStep1.X + myStep2.X, myStep1.Y + myStep2.Y);

                var obstacleStep1 = Math.Abs(2 * this.mass / sumMasses) * this.initialVelocity;
                var obstacleStep2 = Math.Abs(((this.mass - hitInfo.RigidBody.Mass) / sumMasses)) * hitInfo.RigidBody.InitialVelocity;
                var finalObstacleElasticVelocity = obstacleStep1 + obstacleStep2;

                var absElasticVelocity = new Vector2f(Math.Abs(finalElasticVelocity.X), Math.Abs(finalElasticVelocity.Y));
                var absObstacleElasticVelocity = new Vector2f(Math.Abs(finalObstacleElasticVelocity.X), Math.Abs(finalObstacleElasticVelocity.Y));

                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        SetPosition(new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height));
                        SetForce(new Vector2f(this.netForce.X, -absElasticVelocity.Y));
                        break;

                    case EDirection.Up:
                        SetPosition(new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height));
                        SetForce(new Vector2f(this.netForce.X, absElasticVelocity.Y));
                        break;

                    case EDirection.Right:
                        SetPosition(new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y));
                        SetForce(new Vector2f(-absElasticVelocity.X, this.netForce.Y));
                        break;

                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        SetForce(new Vector2f(absElasticVelocity.X, this.netForce.Y));
                        break;
                }

                break;
            #endregion


            #region INELASTICA
            case ECollisionType.Inelastic:
                /** Equação da velocidade final dos corpos em colisão inelástica
                 * https://en.wikipedia.org/wiki/Inelastic_collision => (momentumA + momentumB) / (massA + massB)
                 * Ex.: Vf = (1kg * 250ms + 8kg * 200ms) / 9kg
                 *      Vf = 1850/9
                 *      Vf = 205
                 
                 * A nova velocidade final do sistema em colisão é calculada.
                 * Como a massa é constante, a menos que alguma força externa (fricção) esteja atuando,
                 * a velocidade será sempre a mesma e os corpos nunca cessarão o movimento
                 **/
                var newInelasticVelocity = new Vector2f(
                    (this.momentum.X + hitInfo.RigidBody.Momentum.X) / (this.mass + hitInfo.RigidBody.Mass),
                    (this.momentum.Y + hitInfo.RigidBody.Momentum.Y) / (this.mass + hitInfo.RigidBody.Mass));

                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        this.Root.Position = new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height);
                        break;
                    case EDirection.Up:
                        this.Root.Position = new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height);
                        break;
                    case EDirection.Right:
                        this.Root.Position = new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y);
                        break;
                    case EDirection.Left:
                        this.Root.Position = new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y);
                        break;
                }

                if (hitInfo.Direction == EDirection.Right || hitInfo.Direction == EDirection.Left)
                {
                    this.netForce.X = 0;
                    if (!hitInfo.RigidBody.IsKinematic)
                        this.netForce.X = newInelasticVelocity.X;
                }
                else if (hitInfo.Direction == EDirection.Up || hitInfo.Direction == EDirection.Down)
                {
                    this.netForce.Y = 0;
                    if (!hitInfo.RigidBody.IsKinematic)
                        this.netForce.Y = newInelasticVelocity.Y;
                }
                break;
            #endregion


            #region PARCIAL INELASTICA
            case ECollisionType.PartialInelastic:
                /** Ocorre conservação de apenas uma parte da energia cinética de forma que a energia final é menor do que a energia inicial. 
                 * Constituem a maioria das colisões que ocorre na natureza. 
                 * Nesse caso, após o choque, as partículas separam-se, e a velocidade relativa final é menor do que a inicial.  
                 * http://mundoeducacao.bol.uol.com.br/fisica/colisoes-elasticas-inelasticas.htm
                 * 
                 * 
                 * Nestas colisões o coeficiente de restituição é um valor entre zero e um, 
                 * e consequentemente o valor da velocidade de afastamento é menor que o da de aproximação, 
                 * porém não é nulo: cada corpo terá uma velocidade diferente.                 * 
                 * 
                 * Equação da velocidade final dos corpos em colisão parcialmente inelástica 
                 * https://pt.wikipedia.org/wiki/Colis%C3%A3o_inel%C3%A1stica
                 * Va = (e * mb * (vb - va) + ma * va + mb * vb) / (ma + mb)
                 * Vb = (e * ma * (va - vb) + ma * va + mb * vb) / (ma + mb)
                 * 
                 * Coeficiente de Restituição (e = elasticity)
                 * e = 0: colisão elástica; 
                 * e = 1: colisão inelástica; 
                 * e > 0 && e < 1: parcialmente inelástica
                 * (0.5 * 10 * ([4,0] - [0,0]) + 3 * [0,0] + 3 * [4,0]) / (3 + 10)
                 * (5 * [4,0]) + [0,0] + [12,0]) / 13
                 * ([20,0] + [12,0]) / 13
                 * [32, 0] / 13
                 * Vfa = [2.46, 0] - ocorre perda de energia   
                 */

                var FinalVelocityA = (Material.Bounciness * hitInfo.RigidBody.Mass * (hitInfo.RigidBody.Velocity - this.netForce) + this.mass * this.netForce + hitInfo.RigidBody.Momentum) /
                    (this.mass + hitInfo.RigidBody.Mass);

                var FinalVelocityB = (hitInfo.RigidBody.Material.Bounciness * this.mass * (this.netForce - hitInfo.RigidBody.Velocity) + this.mass * this.netForce + hitInfo.RigidBody.Momentum) /
                    (this.mass + hitInfo.RigidBody.Mass);

                var newPartialInelasticVelocity = new Vector2f(
                    (this.mass * FinalVelocityA.X + hitInfo.RigidBody.Mass * FinalVelocityB.X) / (this.mass + hitInfo.RigidBody.Mass),
                    (this.mass * FinalVelocityA.Y + hitInfo.RigidBody.Mass * FinalVelocityB.Y) / (this.mass + hitInfo.RigidBody.Mass));

                var absVelocity = new Vector2f(Math.Abs(newPartialInelasticVelocity.X), Math.Abs(newPartialInelasticVelocity.Y));

                switch (hitInfo.Direction)
                {
                    case EDirection.Down:
                        SetPosition(new Vector2f(this.Root.Position.X, this.Root.Position.Y - hitInfo.Overlap.Height));
                        SetForce(new Vector2f(this.netForce.X, -absVelocity.Y));
                        break;

                    case EDirection.Up:
                        SetPosition(new Vector2f(this.Root.Position.X, Root.Position.Y + hitInfo.Overlap.Height));
                        SetForce(new Vector2f(this.netForce.X, absVelocity.Y));
                        break;

                    case EDirection.Right:
                        SetPosition(new Vector2f(this.Root.Position.X - hitInfo.Overlap.Width, this.Root.Position.Y));
                        SetForce(new Vector2f(-absVelocity.X, this.netForce.Y));
                        break;

                    case EDirection.Left:
                        SetPosition(new Vector2f(this.Root.Position.X + hitInfo.Overlap.Width, this.Root.Position.Y));
                        SetForce(new Vector2f(absVelocity.X, this.netForce.Y));
                        break;
                }
                break;
            #endregion


            #region TRIGGER
            case ECollisionType.Trigger:
                break;
                #endregion
        }
    }

    /// <summary>
    /// Substitui o valor atual de netforce e executa o ClampForce do novo valor
    /// </summary>
    /// <param name="netForce">Novo valor de netforce</param>
    public void SetForce(Vector2f netForce)
    {
        if (this.isKinematic)
            return;

        this.netForce = netForce;
        ClampForce();
    }

    /// <summary>
    /// Limita o vetor netforce dentro do limite +MaxVelocity e -MaxVelocity
    /// </summary>
    private void ClampForce()
    {
        if (this.netForce.X > MaxVelocity.X)
            this.netForce.X = MaxVelocity.X;
        else if (this.netForce.X < -MaxVelocity.X)
            this.netForce.X = -MaxVelocity.X;

        if (this.netForce.Y > MaxVelocity.Y)
            this.netForce.Y = MaxVelocity.Y;
        //nao clamp para -Y para permitir Jump()
        //else if (this.netForce.Y < -MaxVelocity.Y)
        // this.netForce.Y = -MaxVelocity.Y;
    }
    #endregion
}