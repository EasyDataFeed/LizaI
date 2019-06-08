using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountsCRMFieldsUpdater.DataItems
{
    class SCECustomField
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
