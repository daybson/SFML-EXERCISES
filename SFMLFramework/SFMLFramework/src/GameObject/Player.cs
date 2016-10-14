using SFML.System;
using SFMLFramework;
using SFMLFramework.src.Helper;

public delegate void OnDirectionChange(EDirection direction);
public enum EDirection
{
    Up,
    Down,
    Left,
    Right,
    None
}

/// <summary>
/// Representa um avatar do jogador na tela
/// </summary>
public class Player : GameObject
{
    #region Fields

    /// <summary>
    /// Corpo rígido do avatar
    /// </summary>
    public Rigidbody Rigidbody { get; set; }

    /// <summary>
    /// Renderizador do avatar
    /// </summary>
    public Renderer Renderer { get; set; }

    /// <summary>
    /// Controlador de plataforma do avatar
    /// </summary>
    public PlatformPlayerController PlatformPlayerController { get; set; }

    #endregion


    #region Methods

    //TODO: parametrizar construtor futuramente...
    public Player()
    {
        this.name = "Player 1";
        Renderer = new Renderer(Resources.LoadSpriteSheet("dragon.png"), this);
        Rigidbody = new Rigidbody(5f, Renderer.SpriteSheet.Size, GameObjectCreator.InelasticMaterial, false, this, new Vector2f(300, 900));

        var label = new UIText(this, new Vector2i(0, -28));
        this.Components.Add(label);
        label.Display = (v) => label.SetMessage(string.Format("Vx: {0}\nVy: {1}", Rigidbody.Velocity.X.ToString("0.0"), Rigidbody.Velocity.Y.ToString("0.0")));

        PlatformPlayerController = new PlatformPlayerController(Rigidbody, Renderer);
        PlatformPlayerController.OnSpriteSheetOrientationChange += Renderer.OrientateSpriteSheetTo;
        this.Components.Add(PlatformPlayerController);
        this.Components.Add(Rigidbody);
        this.Components.Add(Renderer);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    /// <summary>
    /// Define sobre qual keyboardInput o controlador de plataforma irá atuar
    /// </summary>
    /// <param name="keyboardInput">Objeto de input do teclado no qual a janela está registrada para notificar eventos</param>
    public void SetKeyboardInput(ref KeyboardInput keyboardInput)
    {
        keyboardInput.OnKeyPressed += PlatformPlayerController.PlayerKeyboardController.OnKeyPressed;
        keyboardInput.OnKeyReleased += PlatformPlayerController.PlayerKeyboardController.OnKeyReleased;
    }

    #endregion
}
