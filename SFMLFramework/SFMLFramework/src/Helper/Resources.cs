using SFML.Graphics;
using SFMLFramework.src.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    /// <summary>
    /// Auxilia no carregamento de recursos do disco
    /// </summary>
    public class Resources
    {
        #region Fields

        /// <summary>
        /// Caminho padrão dos sprites
        /// </summary>
        private static readonly string spritePath = "resources/textures/";

        /// <summary>
        /// Caminho padrão das fontes
        /// </summary>
        private static readonly string fontPath = "resources/fonts/";

        #endregion


        #region Public

        /// <summary>
        /// Carrega um SpriteSheet do disco
        /// </summary>
        /// <param name="name">Nome do arquivo de textura (com extensão)</param>
        /// <returns>Instância da SpriteSheet ou <code>null</code> caso ocorra um erro</returns>
        public static SpriteSheet LoadSpriteSheet(string name)
        {
            try
            {
                return new SpriteSheet(spritePath + name);
            }
            catch (Exception e)
            {
                Logger.Log(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Carrega uma Fonte do disco
        /// </summary>
        /// <param name="name">Nome do arquivo de fonte (com extensão)</param>
        /// <returns>Instância da Fonte ou <code>null</code> caso ocorra um erro</returns>
        public static Font LoadFont(string name)
        {
            try
            {
                return new Font(fontPath + name);
            }
            catch (Exception e)
            {
                Logger.Log(e.Message);
                return null;
            }
        }

        #endregion
    }
}