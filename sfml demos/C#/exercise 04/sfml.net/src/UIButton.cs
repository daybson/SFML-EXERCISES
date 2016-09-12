using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sfml.net.src
{
    public delegate void OnClickEvent();
    class UIButton
    {
        #region Fields

        private Sprite spriteNormal;
        private Sprite spriteClicked;
        private Sprite spriteCurrent;
        private bool currentState;
        private Texture textureNormal;
        private Texture textureClicked;
        public Sprite Sprite { get { return spriteCurrent; } }
        public bool State { get { return currentState; } }
        public OnClickEvent OnClickEvent;
        public bool IsInteractable { get; set; }

        #endregion


        #region Public

        public UIButton(string normalTexturePath, string clickedTexturePath, Vector2f uiPosition)
        {
            if ((textureNormal = new Texture(normalTexturePath)) == null)
                throw new Exception("Error loading texture");

            if ((textureClicked = new Texture(clickedTexturePath)) == null)
                throw new Exception("Error loading texture");

            OnClickEvent += () => { };
            spriteNormal = new Sprite(textureNormal);
            spriteClicked = new Sprite(textureClicked);
            spriteNormal.Position = uiPosition;
            spriteClicked.Position = uiPosition;
            IsInteractable = true;
            SetState(false);
        }

        public void CheckClick(Vector2f mousePos)
        {
            var width = spriteCurrent.GetGlobalBounds().Width;
            var height = spriteCurrent.GetGlobalBounds().Height;

            if (mousePos.X > spriteCurrent.Position.X && mousePos.X < (spriteCurrent.Position.X + width) &&
                mousePos.Y > spriteCurrent.Position.Y && mousePos.Y < (spriteCurrent.Position.Y + height))
                SetState(!currentState);
        }

        #endregion


        #region Private

        private void SetState(bool v)
        {
            if (IsInteractable)
            {
                currentState = v;
                if (currentState)
                {
                    spriteCurrent = spriteClicked;
                    OnClickEvent();
                }
                else
                    spriteCurrent = spriteNormal;
            }
        }

        #endregion
    }
}
