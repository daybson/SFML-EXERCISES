using SFML.Graphics;
using SFML.Window;
using System;
using System.Linq;
using System.Collections.Generic;
using SFML.System;
using SFMLFramework;
using SFMLFramework.src.Helper;
using SFMLFramework.src.Audio;
using SFMLFramework.src.Level;
using System.Threading;
using NetData;
using SFMLFramework.src.Network;

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
    private LobbyLevel lobby;
    private bool isFocused;

    protected NetClient client;
    public NetClient Client { get { return client; } }
    private Thread ioThread;
    private bool isSyncing;

    public Game(string title)
    {
        Logger.Log("Creating Game");

        this.windowTitle = title;
        this.clock = new Clock();
        this.levels = new List<GameLevel>();
        isDebugging = false;
        isRendering = true;
        isFocused = true;
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
        this.window.LostFocus += Window_LostFocus;
        this.window.GainedFocus += Window_GainedFocus;

        this.lobby = new LobbyLevel(ref this.window, this);
        this.levels.Add(lobby);
        LobbyLevel.LobbyIsDone += StartLevel1;
        this.lobby.Initialize(ref this.lobby);

        Run();
    }

    private void Window_GainedFocus(object sender, EventArgs e)
    {
        this.isFocused = true;
        //Console.WriteLine("Janela focada");
    }

    private void Window_LostFocus(object sender, EventArgs e)
    {
        this.isFocused = false;
        //Console.WriteLine("Janela desfocada");
    }

    private void StartLevel1(object sender, EventArgs e)
    {
        Level1 level1 = new Level1(ref this.window, ref this.keyboard, this);
        this.levels.Add(level1);
        this.currentLevel++;
        level1.Initialize(ref this.lobby);
    }

    public void Update(float deltaTime)
    {
        lock (this.levels[currentLevel].GameObjects)
        {
            foreach (var g in this.levels[currentLevel].GameObjects.Reverse<GameObject>())
            {
                g.Update(deltaTime);

                for (var i = 0; i < this.levels[currentLevel].GameObjects.Count; i++)
                {
                    //evita teste de colis�o consigo mesmo
                    if (!g.Equals(this.levels[currentLevel].GameObjects[i]))
                    {
                        var gRigidBody = (ICollisionable)g.GetComponent<Rigidbody>();
                        var gIndexRigidBody = (ICollisionable)this.levels[currentLevel].GameObjects[i].GetComponent<Rigidbody>();
                        CollisionDispatcher.CollisionCheck(ref gRigidBody, ref gIndexRigidBody, deltaTime);
                    }
                }
            }
        }
    }

    protected void Run()
    {
        while (this.window.IsOpen)
        {
            var timer = this.clock.Restart();

            window.DispatchEvents();
            {
                Update(timer.AsSeconds());
                Render();
            }
        }
    }

    protected void Render()
    {
        this.window.Clear(new Color(150, 150, 150));

        foreach (var g in this.levels[currentLevel].GameObjects.Reverse<GameObject>())
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
                g.GetComponent<MusicController>()?.Render(ref this.window);
                this.levels[currentLevel].LevelMusicController.Render(ref this.window);
            }
            g.GetComponent<UIText>()?.Render(ref this.window);
        }

        this.window.Display();
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        Logger.Log("Closing the window");
        Logger.Close();
        this.window.Close();
    }


    #region NET

    public void NetStart()
    {
        this.ioThread = new Thread(() => { this.client = new NetClient(this); });
        ioThread.Start();
    }

    public void NetReady()
    {
        var remoteReady = new RemoteClient();
        remoteReady.type = MessageType.ClientReady;
        this.client.SendMessageToServer(remoteReady);
    }

    public void UpdateFromServer(RemoteClient remote)
    {
        lock (this.levels[currentLevel].GameObjects)
        {
            foreach (var g in this.levels[currentLevel].GameObjects.Reverse<GameObject>())
            {
                if (g.name.Equals(remote.name))
                {
                    Console.WriteLine("Update from server {0}", remote.name);
                    g.Position = new Vector2f(remote.posX, remote.posY);
                    break;
                }
                
                var remoteUpdate = new RemoteClient();
                remoteUpdate.type = MessageType.Update;
                remoteUpdate.name = g.name;
                remoteUpdate.posX = g.Position.X;
                remoteUpdate.posY = g.Position.Y;
                this.client.SendMessageToServer(remoteUpdate);
                Thread.Sleep(50);
            }
        }
        this.isSyncing = true;
    }

    #endregion
}

