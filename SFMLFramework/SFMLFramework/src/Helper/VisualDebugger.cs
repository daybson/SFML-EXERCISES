using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Graphics.Text;

namespace SFMLFramework.src.Helper
{
    public class VisualDebugger : IRender, IComponent
    {
        private string defaultFont = "Roboto-Regular.ttf";
        private Font font;
        private Text text;
        public Text Text { get { return text; } }

        public bool IsEnabled
        {
            get;

            set;
        }

        public GameObject Root
        {
            get;

            set;
        }

        private uint defaultCharacterSize = 10;
        private Color defaultColor = Color.Black;
        private Styles defaultStyle = Styles.Regular;

        public VisualDebugger()
        {
            this.text = new Text();
            this.font = Resources.LoadFont(defaultFont);
            this.text.Font = this.font;
            this.text.Color = this.defaultColor;
            this.text.CharacterSize = this.defaultCharacterSize;
            this.text.Style = this.defaultStyle;
        }

        public void SetMessage(string message)
        {
            this.text.DisplayedString = message;
        }

        public void Render(ref RenderWindow window)
        {
            window.Draw(this.text);
        }

        public void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
