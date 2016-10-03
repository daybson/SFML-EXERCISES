using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    /// <summary>
    /// Define uma interface de sincronização entre a movimentação do PlatformPlayerController e um SpriteSheet
    /// </summary>
    public interface ISpritesheetOrientable
    {
        /// <summary>
        /// SpriteSheet a ser renderizada pelo componente
        /// </summary>
        SpriteSheet SpriteSheet { get; set; }

        /// <summary>
        /// Busca a sequência correta de tiles na sprite sheet a ser renderizada de acordo com uma direção informada
        /// </summary>
        /// <param name="direction">Direção do movimento atual</param>
        void OrientateSpriteSheetTo(EDirection direction);
    }
}