using SFML.Graphics;
using SFMLFramework.src.Audio;
using SFMLFramework.src.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src.Level
{
    public abstract class GameLevel
    {
        protected static int sequence;
        public static int Sequence { get { return sequence; } }
        protected string name;
        protected List<GameObject> gameObjects = new List<GameObject>();
        protected MusicController levelMusicController;
        protected UIText labelCommands;
        protected RenderWindow window;
        protected KeyboardInput keyboard;
        protected GameObject canvas;

        public List<GameObject> GameObjects
        {
            get
            {
                return gameObjects;
            }

            set
            {
                gameObjects = value;
            }
        }

        public MusicController LevelMusicController
        {
            get
            {
                return levelMusicController;
            }

            set
            {
                levelMusicController = value;
            }
        }


        public abstract void Initialize(ref LobbyLevel lobby);

    }
}
