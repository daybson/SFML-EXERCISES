using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sfml.net.src
{
    class Gamepad
    {
        #region Fields

        private uint index;
        private bool isActive = false;
        private bool hasZ = false;
        private bool move = false;
        private uint buttonCount = 0;
        private int deadzone = 15;

        #endregion


        #region Public 

        public Gamepad(uint index)
        {
            this.index = index;
            if(Joystick.IsConnected(this.index))
            {
                Console.WriteLine(String.Format("Gamepad Connected\nID:{0}\n", Joystick.GetIdentification(this.index)));
                this.hasZ = Joystick.HasAxis(this.index, Joystick.Axis.Z);
                this.isActive = true;
                this.buttonCount = Joystick.GetButtonCount(this.index);
            }
            else
            {
                Console.WriteLine("No gamepad connected.");
                this.isActive = false;
            }
        }

        public uint GetButtonPressed()
        {
            for (uint i = 0; i < this.buttonCount; i++)
                if (Joystick.IsButtonPressed(this.index, i))
                    return i;
            return 0;
        }

        #endregion
    }
}
