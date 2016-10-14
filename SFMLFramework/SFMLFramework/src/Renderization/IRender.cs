using SFML.Graphics;

/// <summary>
/// Defnie uma interface para objetos renderiz�veis
/// </summary>
public interface IRender
{
    /// <summary>
    /// Renderiza o objeto na tela
    /// </summary>
    /// <param name="window">Janela de renderiza��o</param>
    void Render(ref RenderWindow window);
}
