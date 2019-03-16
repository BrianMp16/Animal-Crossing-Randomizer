using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACRandomizer
{
    class SetInfo
    {
        public readonly string ItemName;
        public readonly string ItemNumber;
        public readonly string SetName;

        public SetInfo(string ItemName, string ItemNumber, string SetName)
        {
            this.ItemName = ItemName;
            this.ItemNumber = ItemNumber;
            this.SetName = SetName;
        }
    }
}
