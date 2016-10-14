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
    /// Delegate dos m�todos de teclas pressionadas do teclado
    /// </summary>
    public OnKeyPressed OnKeyPressed;

    /// <summary>
    /// Delegate dos m�todos de teclas liberadas do teclado
    /// </summary>
    public OnKeyReleased OnKeyReleased;

    /// <summary>
    /// Construtor padr�o
    /// </summary>
    /// <param name="window">Janela sobre a qual se escutar�o os eventos</param>
    public KeyboardInput(ref RenderWindow window) : base()
    {
        OnKeyPressed += (k) => { };
        OnKeyReleased += (k) => { };

        window.KeyPressed  += ProcessKeyboardPressed;
        window.KeyReleased += ProcessKeyboardReleased;        
    }
    
    /// <summary>
    /// Evento que dispara a execu��o dos m�todos de teclas pressionadas 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ProcessKeyboardPressed(object sender, KeyEventArgs e)
    {
        OnKeyPressed(e.Code);
    }

    /// <summary>
    /// Evento que dispara a execu��o dos m�todos de teclas liberadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ProcessKeyboardReleased(object sender, KeyEventArgs e)
    {
        OnKeyReleased(e.Code);
    }
}
