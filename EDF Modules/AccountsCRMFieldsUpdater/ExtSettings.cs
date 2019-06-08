using AccountsCRMFieldsUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Databox.Libs.AccountsCRMFieldsUpdater
{
    public class ExtSettings
    {
        public string FilePath { get; set; }
        [XmlIgnore]
        public ActionType ActionType { get; set; }
    }
}
