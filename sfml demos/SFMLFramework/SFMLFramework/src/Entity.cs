using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Entity
{
    #region Fields

    protected string name;
    protected Vector2f position;
    protected SpriteSheet spriteSheet;
    protected string spritePath;
    protected bool moveLeft;
    protected bool moveRigth;
    protected Vector2f speed;
    protected Vector2f move;
    protected EDirection direction;
    protected RectangleShape collider;
    protected float mass;
    public RectangleShape Collider { get { return collider; } }
    public OnDirectionChange OnChangeDirection { get; set; }
    private bool isGrounded;
    private bool isFaced;

    #endregion

    public Entity(string spritePath)
    {
        this.spritePath = spritePath;
        this.name = "Dragon";
        this.spriteSheet = new SpriteSheet(spritePath);
        this.OnChangeDirection += this.spriteSheet.SetDirection;
        this.mass = 1;
        this.isGrounded = false;
        this.isFaced = false;
        this.collider = new RectangleShape(new Vector2f(this.spriteSheet.Sprite.GetGlobalBounds().Width, this.spriteSheet.Sprite.GetGlobalBounds().Height));
        this.collider.OutlineColor = Color.Magenta;
        this.collider.OutlineThickness = 1;
        this.collider.FillColor = Color.Transparent;
    }


    #region GameLoop

    public void Update(float deltaTime)
    {
        this.spriteSheet.UpdateAnimation(deltaTime, this.direction);
    }

    public void Render(RenderTarget window)
    {
        window.Draw(this.spriteSheet.Sprite);
        window.Draw(this.collider);
    }

    #endregion


    #region Movement

    private void SetDirectionMove(EDirection direction, bool value)
    {
        this.direction = direction;
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

    public void SetPosition(Vector2f position)
    {
        this.position = position;
        this.spriteSheet.Sprite.Position = position;
        this.collider.Position = position;
    }

    #endregion


    #region Collision

    public bool CheckCollisionY(RectangleShape obstacle, out CollisionInfo hitInfo)
    {
        hitInfo = null;

        //cache dos bounds dos shapes de colisão
        var myLeft = this.collider.Position.X;
        var myRight = this.collider.Position.X + this.collider.Size.X;
        var myUpper = this.collider.Position.Y;
        var myBottom = this.collider.Position.Y + this.collider.Size.Y;

        var oLeft = obstacle.Position.X;
        var oRight = obstacle.Position.X + obstacle.Size.X;
        var oUpper = obstacle.Position.Y;
        var oBottom = obstacle.Position.Y + obstacle.Size.Y;

        //teste de colisão        
        if (!(myRight < oLeft || myLeft > oRight || myBottom < oUpper || myUpper > oBottom))
        {
            this.isGrounded = true;
            float dx = 0;
            float dy = 0;

            //indentifica a direção do movimento atual e calcula a devida profundidade da colisão naquele sentido
            EDirection collisionDirection;
            if (myUpper < oUpper)
            {
                dy = myBottom - oUpper;
                collisionDirection = EDirection.Up;
            }
            else
            {
                dy = myUpper - oBottom;
                collisionDirection = EDirection.Down;
            }

            hitInfo = new CollisionInfo(new Vector2f(dx, dy), collisionDirection);
            Console.WriteLine(hitInfo.ToString());
            this.collider.OutlineColor = Color.Red;
            return true;
        }
        else
        {
            this.isGrounded = false;
            this.collider.OutlineColor = Color.Magenta;
            return false;
        }
    }

    public bool CheckCollisionX(RectangleShape obstacle, out CollisionInfo hitInfo)
    {
        hitInfo = null;

        //cache dos bounds dos shapes de colisão
        var myLeft = this.collider.Position.X;
        var myRight = this.collider.Position.X + this.collider.Size.X;
        var myUpper = this.collider.Position.Y;
        var myBottom = this.collider.Position.Y + this.collider.Size.Y;

        var oLeft = obstacle.Position.X;
        var oRight = obstacle.Position.X + obstacle.Size.X;
        var oUpper = obstacle.Position.Y;
        var oBottom = obstacle.Position.Y + obstacle.Size.Y;

        //teste de colisão        
        if (!(myBottom < oUpper || myUpper > oBottom || myRight < oLeft || myLeft > oRight))
        {
            float dx = 0;
            float dy = 0;

            //indentifica a direção do movimento atual e calcula a devida profundidade da colisão naquele sentido
            if (myLeft < oLeft)
                dx = myRight - oLeft;
            else
                dx = myLeft - oRight;

            EDirection collisionDirection = EDirection.None;
            if (myRight > oLeft)
                collisionDirection = EDirection.Right;
            else if (myLeft < oRight)
                collisionDirection = EDirection.Left;

            hitInfo = new CollisionInfo(new Vector2f(dx, dy), collisionDirection);
            Console.WriteLine(hitInfo.ToString());
            this.collider.OutlineColor = Color.Red;
            return true;
        }
        else
        {
            this.collider.OutlineColor = Color.Magenta;
            return false;
        }
    }

    public void SolveCollision(CollisionInfo hitInfo)
    {
        this.position -= hitInfo.Depth;
        SetPosition(this.position);
        this.isGrounded = true;
    }

    #endregion
}
