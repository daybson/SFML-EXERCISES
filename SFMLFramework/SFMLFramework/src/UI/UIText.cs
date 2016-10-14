using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Graphics.Text;

namespace SFMLFramework.src.Helper
{
    /// <summary>
    /// Define um texto a ser renderizado na tela
    /// </summary>
    public class UIText : IRender, IComponent, IObserver<GameObject>
    {
        #region Fields

        /// <summary>
        /// Fonte padrão
        /// </summary>
        private string defaultFont = "Roboto-Regular.ttf";
        private Font font;

        /// <summary>
        /// Objeto Text
        /// </summary>
        private Text text;
        public Text Text { get { return text; } }

        /// <summary>
        /// Deslocamento em relação a posição do Root
        /// </summary>
        private Vector2i offset;

        public bool IsEnabled { get; set; }
        
        public GameObject Root { get; set; }

        /// <summary>
        /// Tamanho do caractere
        /// </summary>
        private uint defaultCharacterSize = 12;
        public uint DefaultCharacterSize { get { return defaultCharacterSize; } }

        /// <summary>
        /// Cor do texto
        /// </summary>
        private Color defaultColor = Color.Black;

        /// <summary>
        /// Estilo do caractere
        /// </summary>
        private Styles defaultStyle = Styles.Regular;

        /// <summary>
        /// Action que desenha o texto na tela na posição <code>Vector2f</code> informada
        /// </summary>
        public Action<Vector2f> Display;

        #endregion


        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root">Objeto do qual será herdada a posição</param>
        public UIText(GameObject root)
        {
            this.font = Resources.LoadFont(defaultFont);
            this.text = new Text();
            this.text.Font = this.font;
            this.text.Color = this.defaultColor;
            this.text.CharacterSize = this.defaultCharacterSize;
            this.text.Style = this.defaultStyle;
            root.Subscribe(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root">Objeto do qual será herdada a posição</param>
        /// <param name="offset">Deslocamento em relação a posição do Root</param>
        public UIText(GameObject root, Vector2i offset)
        {
            this.font = Resources.LoadFont(defaultFont);
            this.text = new Text();
            this.text.Font = this.font;
            this.text.Color = this.defaultColor;
            this.text.CharacterSize = this.defaultCharacterSize;
            this.text.Style = this.defaultStyle;
            this.offset = offset;
            root.Subscribe(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root">Objeto do qual será herdada a posição</param>
        /// <param name="message">Mensagem a ser escrita na tela</param>
        public UIText(GameObject root, string message)
        {
            this.font = Resources.LoadFont(defaultFont);
            this.text = new Text();
            this.text.Font = this.font;
            this.text.Color = this.defaultColor;
            this.text.CharacterSize = this.defaultCharacterSize;
            this.text.Style = this.defaultStyle;
            SetMessage(message);
            root.Subscribe(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root">Objeto do qual será herdada a posição</param>
        /// <param name="offset">Deslocamento em relação a posição do Root</param>
        /// <param name="message">Mensagem a ser escrita na tela</param>
        public UIText(GameObject root, Vector2i offset, string message)
        {
            this.font = Resources.LoadFont(defaultFont);
            this.text = new Text();
            this.text.Font = this.font;
            this.text.Color = this.defaultColor;
            this.text.CharacterSize = this.defaultCharacterSize;
            this.text.Style = this.defaultStyle;
            this.offset = offset;
            SetMessage(message);
            root.Subscribe(this);
        }

        #endregion


        #region Public

        /// <summary>
        /// Atribui a mensagem a ser escrita ao objeto Text
        /// </summary>
        /// <param name="message">Mensagem a ser escrita na tela</param>
        public void SetMessage(string message)
        {
            this.text.DisplayedString = message;
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
            window.Draw(this.text);
        }

        #endregion


        #region IObserver

        /// <summary>
        /// Evento de IObserver que atualiza a posição dos bounds do collider
        /// </summary>
        /// <param name="value">GameObject com a posição atualizada</param>
        public void OnNext(GameObject value)
        {
            this.text.Position = new Vector2f(value.Position.X + this.offset.X, value.Position.Y + this.offset.Y);
        }

        public void OnError(Exception error) { }

        public void OnCompleted() { }

        #endregion
    }
}
