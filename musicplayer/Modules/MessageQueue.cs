using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicplayer.Modules
{
    public enum MessageTopic
    {
        QueueUpdate,
        PlayingUpdate,
        StatusUpdate,
        VolumeChange,
        TickUpdate
    }
}
