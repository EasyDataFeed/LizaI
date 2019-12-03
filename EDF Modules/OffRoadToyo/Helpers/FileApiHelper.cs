using Newtonsoft.Json;
using OffRoadToyo.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace OffRoadToyo.Helpers
{
    public class FileApiHelper
    {
        public static TiresJson GetTiresList(string tiresFilePath, out string errorTires)
        {
            try
            {
                errorTires = "";
                TiresJson resp = new TiresJson();

                using (StreamReader r = new StreamReader(tiresFilePath))
                {
                    string jsonString = r.ReadToEnd();
                    var jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = 50000000;
                    resp = jss.Deserialize<TiresJson>(jsonString);

                    return resp;
                }
            }
            catch (Exception e)
            {
                errorTires = e.Message;
                return null;
            }
        }

        public static Rootobject GetTirepattern(string tirepatternJson, out string errorTirepattern)
        {
            try
            {
                errorTirepattern = "";
                Rootobject resp = new Rootobject();
                var jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 50000000;

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                resp = JsonConvert.DeserializeObject<Rootobject>(new WebClient().DownloadString(tirepatternJson));

                return resp;
            }
            catch (Exception e)
            {
                errorTirepattern = e.Message;
                return null;
            }
        }
    }
}
