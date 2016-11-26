using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLFramework.src.Audio;
using SFMLFramework.src.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetData;

namespace SFMLFramework.src.Level
{
    public class Level1 : GameLevel
    {
        LobbyLevel lobby;

        /// <summary>
        /// Evento que instancia os jogadores
        /// </summary>
        public static EventHandler InstantiatePlayers;
        Game game;
        public Player mainPlayer;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="window">referência da janela do Game</param>
        /// <param name="keyboard">referência do teclado de eventos do Game</param>
        /// <param name="game">O game isntanciado</param>
        public Level1(ref RenderWindow window, ref KeyboardInput keyboard, Game game)
        {
            this.game = game;
            sequence = 1;
            this.window = window;
            this.keyboard = keyboard;
            this.gameObjects = new List<GameObject>();
            this.canvas = new GameObject("Canvas");
            this.labelCommands = new UIText(this.canvas);
            this.canvas.Components.Add(labelCommands);
            this.levelMusicController = new MusicController();
            lock (this)
            {
                InstantiatePlayers += CreatePlayer;
            }
        }

        /// <summary>
        /// Cria um player ao receber a mensagem de InstantiatePlayers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePlayer(object sender, EventArgs e)
        {
            Console.WriteLine("Create Player " + this.game.NetClient.Remote.name);
            Player player = null;

            //verifica se o ID da mensagem recebida é igual ao ID do cliente
            if (this.game.NetClient.Remote.clientID.Equals(this.game.NetClient.ID))
            {
                //se for, cria o player e mantém a referência dele no objeto mainPlayer
                player = GameObjectCreator.CreatePlayer(ref this.keyboard, this.game.NetClient.Remote.name);
                player.Position = new Vector2f(this.game.NetClient.Remote.posX, this.game.NetClient.Remote.posY);
                this.mainPlayer = player;
            }
            else
            {
                player = new Player(this.game.NetClient.Remote.name);
                player.Position = new Vector2f(this.game.NetClient.Remote.posX, this.game.NetClient.Remote.posY);
            }

            //guarda na lista de gameobjects
            this.gameObjects.Add(player);

            //verifica se já tem 2 objetos Players na lista (ou seja, se já instanciou todos os jogadores que o server aceita)
            //TODO: melhorar depois tirando esse '2' hardcoded e usando parametros ou mensagem do server... 
            if (this.gameObjects.Where(g => g.GetType()== typeof(Player)).Count()==2)
            {
                //Cria uma mensagem de update do personagem e envia para o servidor
                var remote = new RemoteClient();
                remote.clientID = this.game.NetClient.ID;
                remote.name = this.mainPlayer.name;
                remote.posX = this.mainPlayer.Position.X;
                remote.posY = this.mainPlayer.Position.Y;
                remote.type = MessageType.Update;
                Logger.Log("Sending first Update of " + remote.ToString());
                this.game.NetClient.SendMessageToServer(remote);
            }
        }

        /// <summary>
        /// Inicia o level, carregando o cenário e os GameObjects
        /// </summary>
        /// <param name="lobby">Referência do level de lobby para ter acesso ao TCPClient e trocar mensagens na rede</param>
        public override void Initialize(ref LobbyLevel lobby)
        {
            this.lobby = lobby;
            LoadScenario();
            SetupGameObjects();
        }

        /// <summary>
        /// Carrega o cenário
        /// </summary>
        private void LoadScenario()
        {
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Down, new Vector2f(0, Game.windowSize.Y - 32)));
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Right, new Vector2f(Game.windowSize.X - 33, 29)));
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Left, new Vector2f(0, 32)));
        }

        /// <summary>
        /// Instancia os gameobjects locais do level
        /// </summary>
        private void SetupGameObjects()
        {
            this.levelMusicController.LoadMusic("nature024.wav");
            this.levelMusicController.PlayAudio("nature024");
            this.levelMusicController.ChangeVolume(25);

            this.labelCommands.Display = (v) => this.labelCommands.SetMessage(
            "A = move player esquerda\n" +
            "D = move player direita\n" +
            "Seta esqueda = move cubo1 esquerda\n" +
            "Seta direita = move cubo1 direita\n" +
            "M = ataque magia\n" +
            "P = ataque `punch\n" +
            "K = ataque kick\n");
            this.labelCommands.Display.Invoke(V2.Zero);

            this.canvas.Position = new Vector2f(Game.windowSize.X / 2, 0);
            this.gameObjects.Add(this.canvas);
        }

    }
}
