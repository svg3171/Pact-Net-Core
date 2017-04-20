using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PactNet.Configuration.Json;
using PactNet.Mocks.MockHttpService.Models;

namespace PactNet.Mocks.MockHttpService.Mappers
{
    internal class HttpBodyContentMapper : IHttpBodyContentMapper
    {
        public HttpBodyContent Convert(dynamic body, IDictionary<string, string> headers)
        {
            if (body == null)
                return null;

            MediaTypeHeaderValue parsedHeaders = ParseContentTypeHeader(headers);
            int ba = Encoding.ASCII.GetBytes(ConvertBodyToString(body));
            
            var bodyContent = new HttpBodyContent(ba, parsedHeaders);

            return bodyContent;
        }



        private string ConvertBodyToString(dynamic body)
        {
            string c = JsonConvert.SerializeObject(body, JsonConfig.ApiSerializerSettings);
            return c;
        }


        public HttpBodyContent Convert(byte[] content, IDictionary<string, string> headers)
        {
            if (content == null)
                return null;

            var bodyContent = new HttpBodyContent(content, ParseContentTypeHeader(headers));

            return bodyContent;
        }


        private MediaTypeHeaderValue ParseContentTypeHeader(IDictionary<string, string> headers)
        {
            string contentType = headers?
                .Where(hdr => hdr.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                .Select(hdr => hdr.Value)
                .FirstOrDefault();

            MediaTypeHeaderValue contentTypeHeader = (contentType == null)
                ? new MediaTypeHeaderValue("text/plain")
                : MediaTypeHeaderValue.Parse(contentType);

            contentTypeHeader.CharSet = contentTypeHeader.CharSet ?? Encoding.UTF8.WebName;

            return contentTypeHeader;
        }
    }
}