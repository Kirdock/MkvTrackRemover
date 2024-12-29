using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkvTrackRemover.interfaces
{
    interface IAudioConfig
    {
        bool KeepGerman { get; set; }
        bool KeepJapanese { get; set; }
    }

    internal class AudioConfig : IAudioConfig
    {
        public bool KeepGerman { get; set; }
        public bool KeepJapanese { get; set; }

        public AudioConfig(bool keepGerman = false, bool keepJapanese = false)
        {
            KeepGerman = keepGerman;
            KeepJapanese = keepJapanese;
        }
    }
}
