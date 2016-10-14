using SFML.Graphics;
using SFML.System;
using SFMLFramework;
using System;


/// <summary>
/// Determina os tipos de colisão possíveis
/// </summary>
public enum ECollisionType
{
    Elastic,
    PartialInelastic,
    Inelastic,
    Trigger,
    None
}

/// <summary>
/// Representa um collider retangular ao redor de um sprite
/// </summary>
public class Collider : IComponent, IRender, IObserver<GameObject>
{
    #region Fields

    /// <summary>
    /// Shape de visualização do collider
    /// </summary>
    private RectangleShape shape;

    /// <summary>
    /// Barreira do collider
    /// </summary>
    private FloatRect bound;

    /// <summary>
    /// Direção onde o collider será posicionado em relação ao sprite
    /// </summary>
    private EDirection direction;

    /// <summary>
    /// Espessura do collider quando renderizado
    /// </summary>
    private int colliderThickness;

    /// <summary>
    /// Dimensão do sprite (width, height)
    /// </summary>
    private Vector2i spriteDimension;

    public RectangleShape Shape { get { return shape; } }

    public FloatRect Bound { get { return bound; } }

    public EDirection Direction { get { return direction; } }

    public bool IsEnabled { get; set; }

    public GameObject Root { get; set; }

    #endregion


    #region Public

    /// <summary>
    /// Cria um novo collider em uma das extremidades do sprite informado
    /// </summary>
    /// <param name="spriteDimension">Dimensão do sprite</param>
    /// <param name="direction">Direção onde o collider será posicionado</param>
    /// <param name="colliderThickness">Espessura visual do collider (usado apenas para debug visual)</param>
    /// <param name="root">GameObject ao qual o corpo será atribuido</param>
    public Collider(Vector2i spriteDimension, EDirection direction, int colliderThickness, GameObject root)
    {
        this.Root = root;
        this.direction = direction;
        this.colliderThickness = colliderThickness;
        this.spriteDimension = spriteDimension;

        switch (this.direction)
        {
            case EDirection.Down:
                shape = new RectangleShape(new Vector2f(this.spriteDimension.X, this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X, this.Root.Position.Y + this.spriteDimension.Y - this.colliderThickness, this.spriteDimension.X, this.colliderThickness);
                break;
            case EDirection.Up:
                shape = new RectangleShape(new Vector2f(this.spriteDimension.X, this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X, this.Root.Position.Y, this.spriteDimension.X, this.colliderThickness);
                break;
            case EDirection.Right:
                shape = new RectangleShape(new Vector2f(this.colliderThickness, this.spriteDimension.Y - 2 * this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X + this.spriteDimension.X - this.colliderThickness, this.Root.Position.Y + this.colliderThickness, this.colliderThickness, this.spriteDimension.Y - this.colliderThickness);
                break;
            case EDirection.Left:
                shape = new RectangleShape(new Vector2f(this.colliderThickness, this.spriteDimension.Y - 2 * this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X, this.Root.Position.Y + this.colliderThickness, this.colliderThickness, this.spriteDimension.Y - this.colliderThickness);
                break;
        }

        shape.OutlineColor = Color.Blue;
        shape.OutlineThickness = 1f;
        shape.FillColor = Color.Transparent;

        this.Root.Subscribe(this);
    }

    public void Update(float deltaTime)
    {

    }

    /// <summary>
    /// Renderiza o Sprite da SpriteSheet na janela informada
    /// </summary>
    /// <param name="window">Janela de renderização</param>
    public void Render(ref RenderWindow window)
    {
        window.Draw(this.shape);
    }

    /// <summary>
    /// Evento de IObserver que atualiza a posição dos bounds do collider
    /// </summary>
    /// <param name="value">GameObject com a posição atualizada</param>
    public void OnNext(GameObject value)
    {
        switch (this.direction)
        {
            case EDirection.Down:
                this.bound.Left = value.Position.X;
                this.bound.Top = value.Position.Y + this.spriteDimension.Y - this.colliderThickness;
                break;
            case EDirection.Up:
                this.bound.Left = value.Position.X;
                this.bound.Top = value.Position.Y;
                break;
            case EDirection.Right:
                this.bound.Left = value.Position.X + this.spriteDimension.X - this.colliderThickness;
                this.bound.Top = value.Position.Y + this.colliderThickness;
                break;
            case EDirection.Left:
                this.bound.Left = value.Position.X;
                this.bound.Top = value.Position.Y + this.colliderThickness;
                break;
        }

        this.shape.Position = new Vector2f(this.bound.Left, this.bound.Top);
    }

    public void OnError(Exception error) { }

    public void OnCompleted() { }

    #endregion
}
