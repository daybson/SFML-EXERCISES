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
        #region Fields

        /// <summary>
        /// Personagem est� caindo em queda livre?
        /// </summary>
        private bool isFalling;

        /// <summary>
        /// Personagem est� pulando? (true at� que toque o solo novamente)
        /// </summary>
        protected bool isJumping;

        /// <summary>
        /// Personagem est� se movendo para esquerda?
        /// </summary>
        protected bool moveLeft;

        /// <summary>
        /// Personagem est� se movendo para direita?
        /// </summary>
        protected bool moveRigth;

        /// <summary>
        /// Modificador escalar do vetor de pulo
        /// </summary>
        protected readonly float JUMP_FORCE = 3500.0f;

        /// <summary>
        /// Modificador escalar do vetor de caminhada
        /// </summary>
        protected readonly float WALK_FORCE = 250.0f;

        /// <summary>
        /// Expedidor de eventos do teclado
        /// </summary>
        public KeyboardEventDispatcher PlayerKeyboardController { get; set; }

        /// <summary>
        /// Interface de convers�o do sinal expedido pelo controlador de input (KeyboardEventDispatcher) para o mundo do jogo
        /// </summary>
        public IKineticController IKineticController { get; set; }

        /// <summary>
        /// Delegado respons�vel por disparar a mudan�a de orienta��o do spritesheet
        /// </summary>
        public OnSpriteSheetOrientationChange OnSpriteSheetOrientationChange { get; set; }

        public bool IsEnabled { get; set; }

        public GameObject Root { get; set; }

        #endregion


        #region Public 

        public PlatformPlayerController(IKineticController iKineticController, ISpritesheetOrientable iSpritesheetOrientable)
        {
            PlayerKeyboardController = new KeyboardEventDispatcher();
            IKineticController = iKineticController;

            OnSpriteSheetOrientationChange = iSpritesheetOrientable.OrientateSpriteSheetTo;

            this.isFalling = true;
            this.isJumping = true;
        }

        /// <summary>
        /// Efetua caminhada, adicionando for�a Left/Right ao rigidbody
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
        /// Efetura um salto caso esteja colidindo na base com alguma superf�cie, adicionando for�a Up ao rigidbody
        /// </summary>
        public void Jump()
        {
            if (!this.isJumping)
            {
                IKineticController.AddForce(V2.Top * this.JUMP_FORCE);
                this.isJumping = true;
                this.OnSpriteSheetOrientationChange(EDirection.Up);
            }
        }

        public void Update(float deltaTime)
        {
            Move();
        }

        /// <summary>
        /// Efetua movimenta��o adicionando for�a ao IKineticController com base no input de movimento
        /// </summary>
        private void Move()
        {
            if (this.moveRigth)
                IKineticController.SetForce(V2.Right * this.WALK_FORCE);
            else if (this.moveLeft)
                IKineticController.SetForce(V2.Left * this.WALK_FORCE);
        }

        /// <summary>
        /// M�todo delegado de resposta a alguma colis�o, para atualizar as devidas vari�veis do controlador do personagem
        /// </summary>
        /// <param name="direction">Dire��o de ocorr�ncia da colis�o</param>
        public void OnCollisionResponse(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.Down:
                    this.isFalling = false;
                    this.isJumping = false;
                    break;
                case EDirection.Left:
                    this.moveLeft = false;
                    break;
                case EDirection.Right:
                    this.moveRigth = false;
                    break;
            }
        }

        #endregion
    }
}