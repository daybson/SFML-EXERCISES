//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : SFML Framework
//  @ File Name : Game.cs
//  @ Date : 13/09/2016
//  @ Author : Daybson B. S. Paisante <daybson.paisante@outlook.com>
//
//

using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using SFML.System;
using System.Threading;
using SFMLFramework;
using SFMLFramework.src.Helper;

public class Game
{
    #region Fields

    public static readonly Vector2u windowSize = new Vector2u(800, 600);
    public string windowTitle;
    public Clock clock;
    protected RenderWindow window;
    private KeyboardInput keyboard;

    private bool isDebugging;
    private bool isRendering;

    private Player player;
    public Player Player { get { return player; } }

    private List<GameObject> gameObjects;
    public List<GameObject> GameObjects { get { return gameObjects; } }

    #endregion


    #region Public

    public Game(string title)
    {
        this.windowTitle = title;
        this.clock = new Clock();
        this.keyboard = new KeyboardInput(ref this.window);
        this.gameObjects = new List<GameObject>();
    }

    public void Start()
    {
        isDebugging = true;
        this.window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), windowTitle);
        this.window.SetFramerateLimit(60);
        this.window.KeyPressed += this.keyboard.ProcessKeyboardPressed;
        this.window.KeyReleased += this.keyboard.ProcessKeyboardReleased;

        this.window.KeyReleased += (sender, e) => { if (e.Code == Keyboard.Key.P) isDebugging = !isDebugging; };
        this.window.KeyReleased += (sender, e) => { if (e.Code == Keyboard.Key.R) isRendering = !isRendering; };

        this.player = GameObjectCreator.CreatePlayer(ref this.keyboard);
        this.gameObjects.Add(this.player);

        this.player.Position = V2.Right * 650;
        
        this.gameObjects.Add(GameObjectCreator.CreateInelasticBrick(new Vector2f(150, 350)));
        this.gameObjects.Add(GameObjectCreator.CreateInelasticBrick(new Vector2f(450, 350)));

        this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Down, new Vector2f(0, windowSize.Y - 32)));
        this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Right, new Vector2f(windowSize.X - 33, 29)));
        this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Left, new Vector2f(0, 32)));


        Run();
    }

    public void Update(float deltaTime)
    {
        int x = 0;
        foreach (var g in this.gameObjects)
        {
            g.Update(deltaTime);

            #region Loop de colis�o
            for (var i = 0; i < this.gameObjects.Count; i++)
            {
                //evita self teste 
                if (!g.Equals(this.gameObjects[i]))
                {
                    var gRigidBody = (ICollisionable)g.GetComponent<Rigidbody>();
                    var gIndexRigidBody = (ICollisionable)gameObjects[i].GetComponent<Rigidbody>();
                    CollisionDispatcher.CollisionCheck(ref gRigidBody, ref gIndexRigidBody, deltaTime);
                }
            }
            #endregion
            x++;
        }
    }

    #endregion


    #region Protected

    protected void Run()
    {
        while (this.window.IsOpen)
        {
            var timer = this.clock.Restart();

            window.DispatchEvents();
            Update(timer.AsSeconds());
            Render();
        }
    }

    protected void Render()
    {
        this.window.Clear(new Color(150, 150, 150));

        foreach (var g in this.gameObjects)
        {
            if (isRendering)
                g.GetComponent<Renderer>()?.Render(ref this.window);

            if (isDebugging)
            {
                g.GetComponent<Rigidbody>()?.ColliderBottom.Render(ref this.window);
                g.GetComponent<Rigidbody>()?.ColliderTop.Render(ref this.window);
                g.GetComponent<Rigidbody>()?.ColliderRight.Render(ref this.window);
                g.GetComponent<Rigidbody>()?.ColliderLeft.Render(ref this.window);
            }
        }

        this.window.Display();
    }

    #endregion

}
