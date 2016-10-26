using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using SFML.System;
using SFMLFramework;
using SFMLFramework.src.Helper;
using SFMLFramework.src.Audio;
using SFMLFramework.src.Level;

public class Game
{
    public static readonly Vector2u windowSize = new Vector2u(800, 600);
    public string windowTitle;
    public Clock clock;
    protected RenderWindow window;
    
    private bool isDebugging;
    private bool isRendering;
    
    private KeyboardInput keyboard;
    private int currentLevel = 0;
    private List<GameLevel> levels;

    public Game(string title)
    {
        Logger.Log("Creating Game");

        this.windowTitle = title;
        this.clock = new Clock();
        this.levels = new List<GameLevel>();
        isDebugging = false;
        isRendering = true;
    }

    public void Start()
    {
        Logger.Log("Starting Game");
        
        this.window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), windowTitle);
        this.window.SetFramerateLimit(120);
        this.keyboard = new KeyboardInput(ref this.window);
        this.window.Closed += OnGameOver;

        this.window.KeyReleased += (sender, e) => { if (e.Code == Keyboard.Key.F) isDebugging = !isDebugging; };
        this.window.KeyReleased += (sender, e) => { if (e.Code == Keyboard.Key.R) isRendering = !isRendering; };

        //this.levels.Add(new Level1(ref window, ref keyboard));
        this.levels.Add(new LobbyLevel());
        this.currentLevel = 0;

        Run();
    }

    public void Update(float deltaTime)
    {
        string netPackage = string.Empty;

        foreach (var g in this.levels[currentLevel].GameObjects)
        {
            g.Update(deltaTime);

            for (var i = 0; i < this.levels[currentLevel].GameObjects.Count; i++)
            {
                //evita teste de colisão consigo mesmo
                if (!g.Equals(this.levels[currentLevel].GameObjects[i]))
                {
                    var gRigidBody = (ICollisionable)g.GetComponent<Rigidbody>();
                    var gIndexRigidBody = (ICollisionable)this.levels[currentLevel].GameObjects[i].GetComponent<Rigidbody>();
                    CollisionDispatcher.CollisionCheck(ref gRigidBody, ref gIndexRigidBody, deltaTime);
                }
            }
        }
    }

    protected void Run()
    {
        this.levels[currentLevel].Initialize();

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

        foreach (var g in this.levels[currentLevel].GameObjects)
        {
            if (isRendering)
                g.GetComponent<Renderer>()?.Render(ref this.window);

            if (isDebugging)
            {
                g.GetComponent<Rigidbody>()?.ColliderBottom.Render(ref this.window);
                g.GetComponent<Rigidbody>()?.ColliderTop.Render(ref this.window);
                g.GetComponent<Rigidbody>()?.ColliderRight.Render(ref this.window);
                g.GetComponent<Rigidbody>()?.ColliderLeft.Render(ref this.window);
                g.GetComponent<AudioListener3D>()?.Render(ref this.window);
                g.GetComponent<UIText>()?.Render(ref this.window);
                g.GetComponent<MusicController>()?.Render(ref this.window);
                this.levels[currentLevel].LevelMusicController.Render(ref this.window);
            }
        }


        this.window.Display();
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        Logger.Log("Stop network");

        Logger.Log("Closing the window");
        Logger.Close();
        this.window.Close();
    }
}

