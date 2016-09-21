using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Player : Actor
{
    public Mover mover;
    public Renderer renderer;
    public PlayerKeyboardController keyboardController;
    public ICollision collider;
    public CollisionRender collisionRender;
    public Rigidbody rigidbody;

    private string spriteSheedPath = "dragon.png";

    public Player()
    {
        this.name = "Player 1";

        this.mover = AddComponent<Mover>();
        this.mover.Speed = new Vector2f(200, 200);

        this.renderer = AddComponent<Renderer>();
        this.renderer.iMove = this.mover;
        this.renderer.LoadSpriteSheet(spriteSheedPath);

        this.keyboardController = AddComponent<PlayerKeyboardController>();
        this.keyboardController.keyPressedActions.Add(
                Keyboard.Key.A, new Action(() =>
                    this.mover.SetDirectionMove(Mover.EDirection.Left, true)));
        this.keyboardController.keyPressedActions.Add(
                Keyboard.Key.D, new Action(() =>
                   this.mover.SetDirectionMove(Mover.EDirection.Right, true)));
        /*
        this.keyboardController.keyPressedActions.Add(
                Keyboard.Key.W, new Action(() =>
                   this.mover.SetDirectionMove(Mover.EDirection.Up, true)));
        this.keyboardController.keyPressedActions.Add(
                Keyboard.Key.S, new Action(() =>
                   this.mover.SetDirectionMove(Mover.EDirection.Down, true)));
        */

        this.keyboardController.keyReleasedActions.Add(
                Keyboard.Key.A, new Action(() =>
                   this.mover.SetDirectionMove(Mover.EDirection.Left, false)));
        this.keyboardController.keyReleasedActions.Add(
                Keyboard.Key.D, new Action(() =>
                   this.mover.SetDirectionMove(Mover.EDirection.Right, false)));
        /*
        this.keyboardController.keyReleasedActions.Add(
                Keyboard.Key.W, new Action(() =>
                    this.mover.SetDirectionMove(Mover.EDirection.Up, false)));
        this.keyboardController.keyReleasedActions.Add(
                Keyboard.Key.S, new Action(() =>
                    this.mover.SetDirectionMove(Mover.EDirection.Down, false)));
        */

        this.collider = AddComponent<RectCollider>();
        this.collider.SetSprite(this.renderer.SpriteSheet.Sprite);
        this.collider.IMove = this.mover;

        this.collisionRender = AddComponent<CollisionRender>();
        this.collisionRender.shape = this.collider.GetShape();

        this.rigidbody = AddComponent<Rigidbody>();
        this.rigidbody.mover = mover;
        this.rigidbody.mass = 2;
    }    
}
