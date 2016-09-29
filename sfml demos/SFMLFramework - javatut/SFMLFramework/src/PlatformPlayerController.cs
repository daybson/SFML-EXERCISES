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

        protected EDirection spriteDirection;
        public PlayerKeyboardController PlayerKeyboardController
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public OnDirectionChange OnChangeDirection { get; set; }

        private void ProccessInput()
        {
            if (this.moveLeft)
            {
                /*
                currSpeed.X += -this.velocity.X;
                if (currSpeed.X < -velocity.X)
                    currSpeed.X = -velocity.X;
                    */
            }
            else if (this.moveRigth)
            {
                /*
                currSpeed.X += this.velocity.X;
                if (currSpeed.X > velocity.X)
                    currSpeed.X = velocity.X;
                    */
            }
            /*
            else
                currSpeed.X = 0;
                */
        }

        private void SetDirectionMove(EDirection direction, bool value)
        {
            this.spriteDirection = direction;

            switch (direction)
            {
                case EDirection.Left:
                    this.moveLeft = value;
                    if (value)
                        this.moveRigth = !value;
                    break;
                case EDirection.Right:
                    this.moveRigth = value;
                    if (value)
                        this.moveLeft = !value;
                    break;
            }

            if (value)
                this.OnChangeDirection(direction);
        }
    }
}