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
        protected int sequence;
        protected string name;
        protected List<GameObject> gameObjects;
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


        public abstract void Initialize();

    }
}
