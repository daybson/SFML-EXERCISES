﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src.Audio
{
    /// <summary>
    /// Interface adaptadora para execução de áudio
    /// </summary>
    public interface IAudioPlayer
    {
        void PlayAudio(string name);
        void PauseAudio(string name);
        void StopAudio(string name);
        void ChangeVolume(float volume);
    }
}
