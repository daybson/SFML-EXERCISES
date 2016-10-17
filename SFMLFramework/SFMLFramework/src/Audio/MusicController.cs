using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFMLFramework.src.Helper;

namespace SFMLFramework.src.Audio
{
    public class MusicController : IAudioAdapter
    {
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
                music.Stop();
            }
        }

        /// <summary>
        /// Executa o Music, caso exista
        /// </summary>
        /// <param name="name">key do dicionário de Music</param>
        public void PlayAudio(string name)
        {
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
    }
}
