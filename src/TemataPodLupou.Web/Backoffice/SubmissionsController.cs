using CsvHelper.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TemataPodLupou.Web.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Net.Http.Headers;
using UrlMode = Umbraco.Core.Models.PublishedContent.UrlMode;

namespace TemataPodLupou.Web.Backoffice
{
    [Umbraco.Web.Mvc.PluginController("Temata")]
    public class SubmissionsController : UmbracoAuthorizedApiController
    {
        public class SubmissionCsvMap : ClassMap<Submission>
        {
            public SubmissionCsvMap()
            {
                Map(m => m.Id).Name("Id");
                Map(m => m.Name).Name("Name");
                Map(m => m.Title).Name("Title");
                Map(m => m.Description).Name("Description");
                Map(m => m.Reason).Name("Reason");
                Map(m => m.Region).Name("Region");
                Map(m => m.Location).Name("Location");
                Map(m => m.Email).Name("Email");
                Map(m => m.Media).Convert(v => string.Join("\r\n", v.Value.Media?.Select(m => m.MediaItem.Url(mode: UrlMode.Absolute)) ?? new string[0]));
            }
        }

        [HttpGet]
        public HttpResponseMessage Download([FromUri]int nodeId)
        {
            var content = UmbracoContext.Content.GetById(nodeId);
            if (content == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var submissions = content.Descendants<Submission>();

            var memoryStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.Context.RegisterClassMap<SubmissionCsvMap>();
                csvWriter.WriteRecords(submissions);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(memoryStream.ToArray())
            };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = content.Name + ".csv"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            return response;
        }
    }
}