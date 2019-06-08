using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Databox.Libs.MarksJewelersSuppliersData
{
    public class ExtSettings
    {
        #region JohnHardy

        public string JohnHardyFilePath { get; set; }
        public bool JohnHardyCheck { get; set; }

        #endregion

        #region TagHeuer

        public string TagHeuerFilePath { get; set; }
        public bool TagHeuerCheck { get; set; }

        [XmlIgnore]
        public string TagHeuerFolderPath { get; set; }

        [XmlIgnore]
        public List<string> GeneralImageList { get; set; }

        [XmlIgnore]
        public List<string> GeneralImagePath { get; set; }

        #endregion
    }
}
