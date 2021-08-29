using TemataPodLupou.Web.Models;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace TemataPodLupou.Web.ContentFinders
{
    public class EmbeddableScriptContentFinder : AlternativeTemplateContentFinder
    {
        protected override string TemplateAlias => "ScriptEmbed";
        protected override string UrlSlug => "embed";

        [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
        public class Composer : IUserComposer
        {
            public void Compose(Composition composition)
            {
                composition.ContentFinders().InsertBefore<ContentFinderByUrl, EmbeddableScriptContentFinder>();
            }
        }

        public EmbeddableScriptContentFinder(ILogger logger, IFileService fileService)
            : base(logger, fileService) { }

        protected override bool IsTemplateValidForNode(IPublishedContent node)
        {
            return node.ContentType.CompositionAliases.Contains(EmbeddableWidget.ModelTypeAlias);
        }
    }
}