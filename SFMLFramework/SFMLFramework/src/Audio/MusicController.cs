using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFMLFramework.src.Helper;

namespace SFMLFramework.src.Audio
{
    public class MusicController : IAudioAdapter, IObserver<GameObject>
    {
        private string currentPlaying;

        /// <summary>
        /// Dicionário de músicas
        /// </summary>
        private Dictionary<string, Music> soundtracks;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public MusicController()
        {
            this.soundtracks = new Dictionary<string, Music>();
        }

        /// <summary>
        /// Chama a rotina de serialização de um objeto Music em Resources, instancia e armazena a nova música no dicionário
        /// </summary>
        /// <param name="fileName">Nome do arquivo de som (com extensão). O nome sem extensão será usado como Key no dicionado de Sound</param>
        public void LoadMusic(string fileName)
        {
            var music = Resources.LoadMusic(fileName);
            if (music != null)
            {
                this.soundtracks.Add(fileName.Remove(fileName.Length - 4, 4).ToString(), music);
                music.Volume = 100;
                music.Loop = true;
                music.RelativeToListener = true;
                music.Stop();
            }
        }

        /// <summary>
        /// Chama a rotina de serialização de um objeto Music em Resources, instancia e armazena a nova música no dicionário
        /// </summary>
        /// <param name="fileName">Nome do arquivo de som (com extensão). O nome sem extensão será usado como Key no dicionado de Sound</param>
        /// /// <param name="relativeToListener">Som acompanha o Listener?</param>
        /// /// /// <param name="minDistance">Distância mínima para ouvir a música</param>
        /// /// /// <param name="attenuation">Fator de atenuação da música com a distância</param>
        public void LoadMusic(string fileName, bool relativeToListener, float minDistance, float attenuation)
        {
            var music = Resources.LoadMusic(fileName);
            if (music != null)
            {
                this.soundtracks.Add(fileName.Remove(fileName.Length - 4, 4).ToString(), music);
                music.Volume = 100;
                music.Loop = true;
                music.RelativeToListener = relativeToListener;                
                music.Stop();
            }
        }

        /// <summary>
        /// Executa o Music, caso exista
        /// </summary>
        /// <param name="name">key do dicionário de Music</param>
        public void PlayAudio(string name)
        {
            this.currentPlaying = name;
            this.soundtracks[name]?.Play();
            Logger.Log("Playing sound: " + name + " - " + (this.soundtracks[name] != null));
        }

        /// <summary>
        /// Pausa a execução do Music, caso exista
        /// </summary>
        /// <param name="name">key do dicionário de Music</param>
        public void PauseAudio(string name)
        {
            this.soundtracks[name]?.Pause();
            Logger.Log("Pausing sound: " + name + " - " + (this.soundtracks[name] != null));
        }

        /// <summary>
        /// Para a execução do Music, caso exista
        /// </summary>
        /// <param name="name">key do dicionário de Music</param>
        public void StopAudio(string name)
        {
            this.soundtracks[name]?.Stop();
            Logger.Log("Stop sound: " + name + " - " + (this.soundtracks[name] != null));
        }

        public void OnNext(GameObject value)
        {
            foreach (var k in this.soundtracks)
            {
                if (!k.Value.RelativeToListener)
                {
                    k.Value.Position = new SFML.System.Vector3f(value.Position.X, 0, value.Position.Y);
                    //Logger.Log("Music position: " + k.Value.Position.ToString());
                }
            }
        }

        public void OnError(Exception error) { }

        public void OnCompleted() { }
    }
}
