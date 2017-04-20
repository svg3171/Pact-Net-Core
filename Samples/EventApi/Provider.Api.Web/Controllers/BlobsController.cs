using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;



namespace Provider.Api.Web.Controllers
{
    public class BlobsController : Controller
    {
        private const string Data = "This is a test";

        [HttpGet]
        [Route("blobs/{id}")]
        public HttpResponseMessage GetById(Guid id)
        {
            var responseContent = new ByteArrayContent(Encoding.UTF8.GetBytes(Data));
            responseContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "text.txt" };
            responseContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = responseContent
            };
        }

        [HttpPost]
        [Route("blobs/{id}")]
        public HttpResponseMessage Post(Guid id)
        {
            var bytes = new Byte[Request.Body.Length];
            Request.Body.ReadAsync(bytes, 0, bytes.Length);
            var requestBody = Encoding.UTF8.GetString(bytes);

            if (requestBody != Data)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}