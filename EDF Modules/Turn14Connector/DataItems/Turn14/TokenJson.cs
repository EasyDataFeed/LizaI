﻿using System;

namespace Turn14Connector.DataItems.Turn14
{
    class TokenJson
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}