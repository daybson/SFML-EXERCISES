using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using SFML.System;
using SFMLFramework;
using SFMLFramework.src.Helper;
using SFMLFramework.src.Audio;

public class Game
{
    #region Fields

    public static readonly Vector2u windowSize = new Vector2u(800, 600);
    public string windowTitle;
    public Clock clock;
    protected RenderWindow window;
    private KeyboardInput keyboard;

    private GameObject canvas;
    private UIText labelCommands;

    private bool isDebugging;
    private bool isRendering;

    private Player player;
    public Player Player { get { return player; } }

    private List<GameObject> gameObjects;
    public List<GameObject> GameObjects { get { return gameObjects; } }

    private MusicController musicController;

    #endregion


    #region Public

    public Game(string title)
    {
        Logger.Log("Creating Game");

        this.windowTitle = title;
        this.clock = new Clock();

        this.gameObjects = new List<GameObject>();
        this.musicController = new MusicController();

        this.canvas = new GameObject("Canvas");
        this.labelCommands = new UIText(this.canvas);
        this.canvas.Components.Add(labelCommands);

        isDebugging = false;
        isRendering = false;
    }

    public void Start()
    {
        try
        {
            Logger.Log("Starting Game");

            this.musicController.LoadMusic("nature024.wav");

            this.window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), windowTitle);
            this.window.SetFramerateLimit(120);
            this.keyboard = new KeyboardInput(ref this.window);

            this.window.Closed += OnGameOver;

            this.labelCommands.Display = (v) => this.labelCommands.SetMessage(
                "A = move player esquerda\n" +
                "D = move player direita\n" +
                "Seta esqueda = move cubo1 esquerda\n" +
                "Seta direita = move cubo1 direita\n");

            this.canvas.Position = new Vector2f(windowSize.X / 2, 0);
            this.gameObjects.Add(this.canvas);

            this.player = GameObjectCreator.CreatePlayer(ref this.keyboard);
            this.gameObjects.Add(this.player);
            this.player.Position = V2.Right * 650;

            /*
            var parcialInelastic = GameObjectCreator.CreatePartialInelasticBrick(new Vector2f(300, 400));
            this.gameObjects.Add(parcialInelastic);
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Right) parcialInelastic.GetComponent<Rigidbody>().AddForce(V2.Right * 200); };
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Left) parcialInelastic.GetComponent<Rigidbody>().AddForce(V2.Left * 200); };
            */

            /*
            var elastic = GameObjectCreator.CreateElasticBrick(new Vector2f(500, 400));
            this.gameObjects.Add(elastic);
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Right) elastic.GetComponent<Rigidbody>().AddForce(V2.Right * 200); };
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Left) elastic.GetComponent<Rigidbody>().AddForce(V2.Left * 200); };
            */


            var inelastic = GameObjectCreator.CreateInelasticBrick(new Vector2f(300, 400));
            this.gameObjects.Add(inelastic);
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Right) inelastic.GetComponent<Rigidbody>().AddForce(V2.Right * 200); };
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Left) inelastic.GetComponent<Rigidbody>().AddForce(V2.Left * 200); };


            /*
            var right = GameObjectCreator.CreateElasticBrick2(new Vector2f(300, 0));
            this.gameObjects.Add(right);
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Numpad6) right.GetComponent<Rigidbody>().AddForce(V2.Right * 200); };
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Numpad4) right.GetComponent<Rigidbody>().AddForce(V2.Left * 200); };
            */

            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Down, new Vector2f(0, windowSize.Y - 32)));
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Right, new Vector2f(windowSize.X - 33, 29)));
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Left, new Vector2f(0, 32)));

            this.window.KeyReleased += (sender, e) => { if (e.Code == Keyboard.Key.F) isDebugging = !isDebugging; };
            this.window.KeyReleased += (sender, e) => { if (e.Code == Keyboard.Key.R) isRendering = !isRendering; };

            this.labelCommands.Display.Invoke(V2.Zero);

            this.musicController.PlayAudio("nature024");

            Run();
        }
        catch (Exception e)
        {
            Logger.Log(e.ToString());
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var g in this.gameObjects)
        {
            g.Update(deltaTime);

            #region Loop de colisão
            for (var i = 0; i < this.gameObjects.Count; i++)
            {
                //evita teste de colisão consigo mesmo
                if (!g.Equals(this.gameObjects[i]))
                {
                    var gRigidBody = (ICollisionable)g.GetComponent<Rigidbody>();
                    var gIndexRigidBody = (ICollisionable)gameObjects[i].GetComponent<Rigidbody>();
                    CollisionDispatcher.CollisionCheck(ref gRigidBody, ref gIndexRigidBody, deltaTime);
                }
            }
            #endregion
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
                g.GetComponent<UIText>()?.Render(ref this.window);
            }
        }

        this.window.Display();
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        Logger.Log("Closing the window");
        Logger.Close();
        this.window.Close();
    }

    #endregion
}
