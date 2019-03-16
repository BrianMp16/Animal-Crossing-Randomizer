using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACRandomizer
{
    class ModInfo
    {
        public readonly string modName;
        public readonly string modNumber;
        public readonly string modAddress;
        public readonly string modGroup;

        public ModInfo(string modName, string modNumber, string modAddress, string modGroup)
        {
            this.modName = modName;
            this.modNumber = modNumber;
            this.modAddress = modAddress;
            this.modGroup = modGroup;
        }
    }
}
