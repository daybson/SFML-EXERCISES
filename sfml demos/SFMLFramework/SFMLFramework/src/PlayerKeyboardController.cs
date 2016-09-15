//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : SFML Framework
//  @ File Name : PlayerKeyboardController.cs
//  @ Date : 14/09/2016
//  @ Author : Daybson B. S. Paisante <daybson.paisante@outlook.com>
//
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


public class PlayerKeyboardController : Component
{
    public Dictionary<Keyboard.Key, Action> keyPressedActions;
    public Dictionary<Keyboard.Key, Action> keyReleasedActions;

    public PlayerKeyboardController()
    {
        this.keyPressedActions = new Dictionary<Keyboard.Key, Action>();
        this.keyReleasedActions = new Dictionary<Keyboard.Key, Action>();
    }

    public void OnKeyPressed(Keyboard.Key key)
    {
        if (this.keyPressedActions.ContainsKey(key))
            this.keyPressedActions[key].Invoke();
    }

    public void OnKeyReleased(Keyboard.Key key)
    {
        if (this.keyReleasedActions.ContainsKey(key))
            this.keyReleasedActions[key].Invoke();
    }

    public override void Update(float deltaTime)
    {

    }
}
