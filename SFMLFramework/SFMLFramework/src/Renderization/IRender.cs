using SFML.Graphics;

/// <summary>
/// Defnie uma interface para objetos renderizáveis
/// </summary>
public interface IRender
{
    /// <summary>
    /// Renderiza o objeto na tela
    /// </summary>
    /// <param name="window">Janela de renderização</param>
    void Render(ref RenderWindow window);
}
