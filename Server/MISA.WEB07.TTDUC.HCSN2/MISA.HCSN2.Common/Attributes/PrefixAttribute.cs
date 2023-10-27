using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.Common.Attributes
{
    public class PrefixAttribute : Attribute
    {
        public string Name { get; set; }

        public PrefixAttribute(string name)
        {
            Name = name;
        }
    }
}
