using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memory
{
    internal class MenuOption
    {
        public string Name { get; }
        public Action Selected { get; }

        public MenuOption(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }
}
