﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MPB_BLL.Home;
using MPB_BLL.COMMON;
using MPB_Entities.COMMON;
using MPB_Entities.Home;
using MPB_Entities.Api;
using MPB_PMMS.Helper;
using System.Text;
using Newtonsoft.Json;

namespace MPB_PMMS.Controllers
{
    public class JsonNetController : Controller
    {
        protected override JsonResult Json(object data, string contentType,
                  Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            if (behavior == JsonRequestBehavior.DenyGet
                && string.Equals(this.Request.HttpMethod, "GET",
                                 StringComparison.OrdinalIgnoreCase))
                //Call JsonResult to throw the same exception as JsonResult
                return new JsonResult();

            return new JsonNetResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                
            };
        }
    }

    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
            
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType =
                !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output)
                {
                    Formatting = Formatting.Indented
                };
                
                SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                
                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.NullValueHandling = NullValueHandling.Ignore;
                
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}

