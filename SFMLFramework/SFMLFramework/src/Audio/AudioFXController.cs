﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFMLFramework.src.Helper;

namespace SFMLFramework.src.Audio
{
    /// <summary>
    /// Define um controlador de audio para efeitos especiais de som
    /// </summary>
    public class AudioFXController : IComponent, IAudioAdapter
    {
        /// <summary>
        /// Dicionário de bufefrs
        /// </summary>
        private Dictionary<string, SoundBuffer> buffer;

        /// <summary>
        /// Dicionário de sons
        /// </summary>
        private Dictionary<string, Sound> sfx;
        public Dictionary<string, Sound> Sfx { get { return sfx; } }

        public bool IsEnabled { get; set; }
        public GameObject Root { get; set; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public AudioFXController()
        {
            this.buffer = new Dictionary<string, SoundBuffer>();
            this.sfx = new Dictionary<string, Sound>();
        }

        /// <summary>
        /// Chama a rotina de serialização de um Sound em Resources, instancia e armazena um novo buffer para o novo Sound e por fim instancia e armazena o novo Sound no dicionário
        /// </summary>
        /// <param name="fileName">Nome do arquivo de som (com extensão). O nome sem extensão será usado como Key no dicionado de Sound</param>
        public void LoadSoundFX(string fileName)
        {
            var buf = Resources.LoadSSoundBuffer(fileName);
            if (buf != null)
            {
                this.buffer.Add(fileName.Replace(".wav", "").ToString(), buf);
                var fx = new Sound(buf);
                fx.Volume = 100;
                fx.Stop();
                this.sfx.Add(fileName.Replace(".wav", "").ToString(), fx);
            }
        }

        public void Update(float deltaTime) { }

        /// <summary>
        /// Executa o som, caso exista
        /// </summary>
        /// <param name="name">key do dicionário de Sound</param>
        public void PlaySound(string name)
        {
            this.sfx[name]?.Play();
            Logger.Log("Playing sound: " + name + " - " + (this.sfx[name] != null));
        }

        /// <summary>
        /// Pausa a execução do som, caso exista
        /// </summary>
        /// <param name="name">key do dicionário de Sound</param>

        public void PauseSound(string name)
        {
            this.sfx[name]?.Pause();
            Logger.Log("Pausing sound: " + name + " - " + (this.sfx[name] != null));
        }

        /// <summary>
        /// Para a execução do som, caso exista
        /// </summary>
        /// <param name="name">key do dicionário de Sound</param>
        public void StopSound(string name)
        {
            this.sfx[name]?.Stop();
            Logger.Log("Stop sound: " + name + " - " + (this.sfx[name] != null));
        }
    }
}
