﻿using Microsoft.AspNetCore.Mvc;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public Object Data { get; set; }

    }
}
