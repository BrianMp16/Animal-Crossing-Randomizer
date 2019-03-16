using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACRandomizer
{
    class EventInfo
    {
        public readonly string eventName;
        public readonly string eventAddress;
        public readonly string eventGroup;
        public readonly int itemPosition;
        public readonly int byteSize;

        public EventInfo(string eventName, string eventAddress, string eventGroup, int itemPosition, int byteSize)
        {
            this.eventName = eventName;
            this.eventAddress = eventAddress;
            this.eventGroup = eventGroup;
            this.itemPosition = itemPosition;
            this.byteSize = byteSize;
        }
    }
}
