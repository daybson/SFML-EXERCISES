using SFML.Window;
using SFMLFramework.src.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    /// <summary>
    /// Define o controllador do personagem para jogos de plataforma.
    /// </summary>
    public class PlatformPlayerController : IComponent
    {
        /// <summary>
        /// Personagem está caindo em queda livre?
        /// </summary>
        protected bool isFalling;

        /// <summary>
        /// Personagem está pulando? (true até que toque o solo novamente)
        /// </summary>
        protected bool isJumping;

        /// <summary>
        /// Personagem está se movendo para esquerda?
        /// </summary>
        protected bool moveLeft;

        /// <summary>
        /// Personagem está se movendo para direita?
        /// </summary>
        protected bool moveRigth;

        /// <summary>
        /// Modificador escalar do vetor de pulo
        /// </summary>
        protected readonly float JUMP_FORCE = 10.0f;

        /// <summary>
        /// Modificador escalar do vetor de caminhada
        /// </summary>
        protected readonly float WALK_FORCE = 150.0f;

        /// <summary>
        /// Expedidor de eventos do teclado
        /// </summary>
        public KeyboardEventDispatcher PlayerKeyboardController { get; set; }

        /// <summary>
        /// Interface de conversão do sinal expedido pelo controlador de input (KeyboardEventDispatcher) para o mundo do jogo
        /// </summary>
        public IKineticController IKineticController { get; set; }

        /// <summary>
        /// Delegado responsável por disparar a mudança de orientação do spritesheet
        /// </summary>
        public OnSpriteSheetOrientationChange OnSpriteSheetOrientationChange { get; set; }

        public bool IsEnabled { get; set; }

        public GameObject Root { get; set; }

        public PlatformPlayerController(IKineticController iKineticController, ISpritesheetOrientable iSpritesheetOrientable)
        {
            PlayerKeyboardController = new KeyboardEventDispatcher();
            IKineticController = iKineticController;

            OnSpriteSheetOrientationChange = iSpritesheetOrientable.OrientateSpriteSheetTo;

            PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.A, () => Walk(EDirection.Left, true));
            PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.D, () => Walk(EDirection.Right, true));
            PlayerKeyboardController.keyReleasedActions.Add(Keyboard.Key.A, () => Walk(EDirection.Left, false));
            PlayerKeyboardController.keyReleasedActions.Add(Keyboard.Key.D, () => Walk(EDirection.Right, false));
            PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.Space, () => Jump());

            this.isFalling = true;
            this.isJumping = true;
        }

        /// <summary>
        /// Efetua caminhada, adicionando força Left/Right ao rigidbody
        /// </summary>
        public void Walk(EDirection direction, bool value)
        {
            switch (direction)
            {
                case EDirection.Right:
                    this.moveRigth = value;
                    break;

                case EDirection.Left:
                    this.moveLeft = value;
                    break;
            }
            this.OnSpriteSheetOrientationChange(direction);
        }

        /// <summary>
        /// Efetura um salto caso esteja colidindo na base com alguma superfície, adicionando força Up ao rigidbody
        /// </summary>
        public void Jump()
        {
            if (!this.isJumping)
            {
                IKineticController.AddForce(Extension.Top * this.JUMP_FORCE);
                this.isJumping = true;
                this.OnSpriteSheetOrientationChange(EDirection.Up);
            }
        }

        public void Update(float deltaTime)
        {
            Move();
        }

        /// <summary>
        /// Efetua movimentação adicionando força ao IKineticController com base no input de movimento
        /// </summary>
        private void Move()
        {
            if (this.moveRigth)
                IKineticController.AddForce(Extension.Right * this.WALK_FORCE);
            else if (this.moveLeft)
                IKineticController.AddForce(Extension.Left * this.WALK_FORCE);
        }
    }
}