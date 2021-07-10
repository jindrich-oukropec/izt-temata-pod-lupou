using System;
using TemataPodLupou.Web.Models;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace TemataPodLupou.Web.ContentFinders
{
    public class SubmissionWidgetHostScriptFinder : ContentFinderByUrl
    {
        private const string ScriptEmbedTemplateAlias = "SubmissionWidgetScriptEmbed";
        
        [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
        public class Composer : IUserComposer
        {
            public void Compose(Composition composition)
            {
                composition.ContentFinders().InsertBefore<ContentFinderByUrl, SubmissionWidgetHostScriptFinder>();
            }
        }

        private readonly ITemplate _template;

        public SubmissionWidgetHostScriptFinder(ILogger logger, IFileService fileService)
            : base(logger)
        {
            _template = fileService.GetTemplate(ScriptEmbedTemplateAlias);
            if (_template == null)
            {
                Logger.Error(typeof(SubmissionWidgetHostScriptFinder), "Cannot find template: '{TemplateAlias}'", ScriptEmbedTemplateAlias);
            }
        }
        
        public override bool TryFindContent(PublishedRequest frequest)
        {
            var path = frequest.Uri.GetAbsolutePathDecoded();

            if (frequest.HasDomain)
                path = DomainUtilities.PathRelativeToDomain(frequest.Domain.Uri, path);

            if (path == "/")
            {
                return false;
            }

            var pos = path.LastIndexOf('/');
            var templateAlias = path.Substring(pos + 1);
            path = pos == 0 ? "/" : path.Substring(0, pos);

            if (!string.Equals(templateAlias, "embed", StringComparison.InvariantCultureIgnoreCase))
                return false;

            var route = frequest.HasDomain ? (frequest.Domain.ContentId + path) : path;
            var node = FindContent(frequest, route);

            if (node == null)
                return false;

            if (node.ContentType.Alias != SubmissionWidget.ModelTypeAlias)
            {
                Logger.Debug<SubmissionWidgetHostScriptFinder, string>("Node is not a SubmissionWidget: '{Route}'", route);
                return false;
            }
            
            frequest.SetTemplate(_template);
            return true;
        }
    }
}