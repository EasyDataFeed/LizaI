using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleRequestsScraper.DataItems
{
    public class GoogleJsonItem
    {
        public General general { get; set; }
        public Organic[] organic { get; set; }
        public Knowledge knowledge { get; set; }
        public Snack_Pack[] snack_pack { get; set; }
        public Pagination pagination { get; set; }
        public Related[] related { get; set; }
        public People_Also_Ask[] people_also_ask { get; set; }
        public List<Top_Ads> top_ads { get; set; }
    }

    public class General
    {
        public string language { get; set; }
        public string location { get; set; }
        public bool mobile { get; set; }
        public bool basic_view { get; set; }
        public string search_type { get; set; }
    }

    public class Knowledge
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string site { get; set; }
        public Social_Media[] social_media { get; set; }
        public string description { get; set; }
        public string description_source { get; set; }
        public string description_link { get; set; }
        public string headquarters { get; set; }
        public string headquarters_link { get; set; }
    }

    public class Social_Media
    {
        public string link { get; set; }
        public string name { get; set; }
    }

    public class Pagination
    {
        public int current_page { get; set; }
        public string next_page_link { get; set; }
        public int next_page_start { get; set; }
        public int next_page { get; set; }
        public Page[] pages { get; set; }
    }

    public class Page
    {
        public int page { get; set; }
        public string link { get; set; }
        public int start { get; set; }
    }

    public class Organic
    {
        public int rank { get; set; }
        public string link { get; set; }
        public string display_link { get; set; }
    }

    public class Snack_Pack
    {
        public int rank { get; set; }
        public string cid { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string work_status { get; set; }
        public string address { get; set; }
        public string maps_link { get; set; }
    }

    public class Related
    {
        public string link { get; set; }
        public string text { get; set; }
    }

    public class People_Also_Ask
    {
        public string question { get; set; }
        public string question_link { get; set; }
        public string answer_source { get; set; }
        public string answer_link { get; set; }
        public string answer_display_link { get; set; }
        public string answer_html { get; set; }
    }

    public class Top_Ads
    {
        public string rank { get; set; }
        public string link { get; set; }
        public string display_link { get; set; }
        public string referral_link { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
    }

}
