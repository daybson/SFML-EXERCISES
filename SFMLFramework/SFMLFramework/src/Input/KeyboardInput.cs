using SFML.Graphics;
using SFML.Window;

public delegate void OnKeyPressed(Keyboard.Key key);
public delegate void OnKeyReleased(Keyboard.Key key);

/// <summary>
/// Gerencia os eventos de entrada do teclado
/// </summary>
public class KeyboardInput : IKeyboardInput
{
    /// <summary>
    /// Delegate dos métodos de teclas pressionadas do teclado
    /// </summary>
    public OnKeyPressed OnKeyPressed;

    /// <summary>
    /// Delegate dos métodos de teclas liberadas do teclado
    /// </summary>
    public OnKeyReleased OnKeyReleased;

    /// <summary>
    /// Construtor padrão
    /// </summary>
    /// <param name="window">Janela sobre a qual se escutarão os eventos</param>
    public KeyboardInput(ref RenderWindow window) : base()
    {
        OnKeyPressed += (k) => { };
        OnKeyReleased += (k) => { };

        window.KeyPressed  += ProcessKeyboardPressed;
        window.KeyReleased += ProcessKeyboardReleased;        
    }
    
    /// <summary>
    /// Evento que dispara a execução dos métodos de teclas pressionadas 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ProcessKeyboardPressed(object sender, KeyEventArgs e)
    {
        OnKeyPressed(e.Code);
    }

    /// <summary>
    /// Evento que dispara a execução dos métodos de teclas liberadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ProcessKeyboardReleased(object sender, KeyEventArgs e)
    {
        OnKeyReleased(e.Code);
    }
}
