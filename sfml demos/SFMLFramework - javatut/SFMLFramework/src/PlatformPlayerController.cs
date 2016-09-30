using SFML.Window;
using SFMLFramework.src.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework
{
    public class PlatformPlayerController
    {
        protected bool isFalling;

        protected bool isJumping;

        protected bool moveLeft;

        protected bool moveRigth;

        protected readonly float JUMP_FORCE = 10.0f;

        protected readonly float WALK_FORCE = 4.0f;

        protected EDirection spriteDirection;

        public KeyboardEventDispatcher PlayerKeyboardController { get; set; }

        public IKineticController IKineticController { get; set; }

        public OnSpriteSheetOrientationChange OnSpriteSheetOrientationChange { get; set; }

        public PlatformPlayerController(IKineticController iKineticController, ISpritesheetOrientable iSpritesheetOrientable)
        {
            PlayerKeyboardController = new KeyboardEventDispatcher();
            IKineticController = iKineticController;

            OnSpriteSheetOrientationChange = iSpritesheetOrientable.OrientateSpriteSheetTo;

            PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.A, () => Walk(EDirection.Left));
            PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.D, () => Walk(EDirection.Right));
            PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.Space, () => Jump());

            this.isFalling = true;
            this.isJumping = true;
        }

        public void Walk(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.Right:
                    IKineticController.AddForce(Extension.Right * this.WALK_FORCE);
                    break;

                case EDirection.Left:
                    IKineticController.AddForce(Extension.Left * this.WALK_FORCE);
                    break;
            }
            this.OnSpriteSheetOrientationChange(direction);
        }

        public void Jump()
        {
            if (!this.isJumping)
            {
                IKineticController.AddForce(Extension.Top * this.JUMP_FORCE);
                this.isJumping = true;
                this.OnSpriteSheetOrientationChange(EDirection.Up);
            }
        }        
    }
}