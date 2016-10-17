using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src.Audio
{
    /// <summary>
    /// Interface adaptadora para execução de áudio
    /// </summary>
    public interface IAudioAdapter
    {
        void PlaySound(string name);
        void PauseSound(string name);
        void StopSound(string name);
    }
}
