using TemataPodLupou.Web.Models;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace TemataPodLupou.Web.ContentFinders
{
    public class SubmissionsCsvDownloadFinder : AlternativeTemplateContentFinder
    {
        protected override string TemplateAlias => "SubmissionWidgetDataExport";
        protected override string DocTypeAlias => SubmissionWidget.ModelTypeAlias;
        protected override string UrlSlug => "export";

        [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
        public class Composer : IUserComposer
        {
            public void Compose(Composition composition)
            {
                composition.ContentFinders().InsertBefore<ContentFinderByUrl, SubmissionsCsvDownloadFinder>();
            }
        }

        public SubmissionsCsvDownloadFinder(ILogger logger, IFileService fileService)
            : base(logger, fileService) { }
    }
}