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
    public class UIText : IRender, IComponent, IObserver<GameObject>
    {
        #region Fields

        private string defaultFont = "Roboto-Regular.ttf";
        private Font font;

        private Text text;
        public Text Text { get { return text; } }

        private Vector2i offset;

        public bool IsEnabled { get; set; }
        public GameObject Root { get; set; }

        private uint defaultCharacterSize = 12;
        public uint DefaultCharacterSize { get { return defaultCharacterSize; } }

        private Color defaultColor = Color.Black;
        private Styles defaultStyle = Styles.Regular;

        public Action<Vector2f> Display;

        #endregion


        #region Constructors

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

        public void SetMessage(string message)
        {
            this.text.DisplayedString = message;
        }

        public void Update(float deltaTime)
        {

        }

        public void Render(ref RenderWindow window)
        {
            window.Draw(this.text);
        }

        #endregion


        #region IObserver

        public void OnNext(GameObject value)
        {
            this.text.Position = new Vector2f(value.Position.X + this.offset.X, value.Position.Y + this.offset.Y);
        }

        public void OnError(Exception error) { }

        public void OnCompleted() { }

        #endregion
    }
}
