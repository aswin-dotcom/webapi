﻿using System.Net;

namespace webapi.Models
{
    public class APIresponsecs
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessage { get; set; }
        public object Result { get; set; }

    }
}
