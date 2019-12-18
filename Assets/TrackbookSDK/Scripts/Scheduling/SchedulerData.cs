using System;
using System.Collections.Generic;

namespace Trackbook.Network.Scheduling
{
    [Serializable]
    internal class SchedulerData
    {
        internal List<string> contents;

        internal SchedulerData()
        {
            contents = new List<string>();
        }
    }
}