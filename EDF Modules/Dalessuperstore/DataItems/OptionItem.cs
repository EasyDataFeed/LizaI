using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dalessuperstore.DataItems
{
    public class OptionItem
    {
        public string Name { get; set; }
        public List<Values> Values { get; set; }
    }

    public class Values
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
    }
}
