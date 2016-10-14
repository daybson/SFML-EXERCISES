using SFML.Window;

/// <summary>
/// Define os eventos usados pelo Keyboard do jogo
/// </summary>
public interface IKeyboardInput 
{
    /// <summary>
    /// Evento de teclas pressionadas 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	void ProcessKeyboardPressed(object sender, KeyEventArgs e);

    /// <summary>
    /// Evento de teclas liberadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	void ProcessKeyboardReleased(object sender, KeyEventArgs e);
}
