using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SFML.Window.Joystick;

namespace sfml.net.src
{
    class Gamepad
    {
        #region Fields

        private uint index;
        public uint Index { get { return index; } }

        private bool isActive = false;
        private bool hasZ = false;
        private bool move = false;
        private uint buttonCount = 0;
        private int deadzone = 15;

        public float ButtonCount
        {
            get { return Joystick.GetButtonCount(this.index); }
        }
        public Identification Identification
        {
            get { return Joystick.GetIdentification(this.index); }
        }
        public bool IsConnected
        {
            get { return Joystick.IsConnected(this.index); }
        }


        public uint GetButtonPressed
        {
            get
            {
                for (uint i = 0; i < this.buttonCount; i++)
                    if (Joystick.IsButtonPressed(this.index, i))
                        return i;
                return
                    this.buttonCount + 1;
            }
        }

        #endregion


        #region Public 

        public Gamepad(uint index)
        {
            this.index = index;
            if (Joystick.IsConnected(this.index))
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

        public float GetAxisPosition(Axis axis)
        {
            return Joystick.GetAxisPosition(this.index, axis);
        }

        public bool HasAxis(Axis axis)
        {
            return Joystick.HasAxis(this.index, axis);
        }

        public bool IsButtonPressed(uint button)
        {
            return Joystick.IsButtonPressed(this.index, button);
        }

        

        #endregion
    }
}
