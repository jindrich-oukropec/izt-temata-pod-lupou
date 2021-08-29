using System;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Routing;

namespace TemataPodLupou.Web.ContentFinders
{
    public abstract class AlternativeTemplateContentFinder : ContentFinderByUrl
    {
        protected abstract string TemplateAlias { get; }
        protected abstract string DocTypeAlias { get; }
        protected abstract string UrlSlug { get; }

        private readonly ITemplate _template;

        public AlternativeTemplateContentFinder(ILogger logger, IFileService fileService)
            : base(logger)
        {
            _template = fileService.GetTemplate(TemplateAlias);
            if (_template == null)
            {
                Logger.Error<AlternativeTemplateContentFinder>("Cannot find template: {TemplateAlias}", TemplateAlias);
            }
        }

        public override bool TryFindContent(PublishedRequest request)
        {
            var path = request.Uri.GetAbsolutePathDecoded();

            if (request.HasDomain)
                path = DomainUtilities.PathRelativeToDomain(request.Domain.Uri, path);

            if (path == "/")
            {
                return false;
            }

            var lastSlashPosition = path.LastIndexOf('/');
            var lastUrlSegment = path.Substring(lastSlashPosition + 1);

            if (!string.Equals(lastUrlSegment, UrlSlug, StringComparison.InvariantCultureIgnoreCase))
                return false;

            var parentRoute = lastSlashPosition == 0 ? "/" : path.Substring(0, lastSlashPosition);
            if (request.HasDomain)
                parentRoute = request.Domain.ContentId + parentRoute;
            var node = FindContent(request, parentRoute);

            if (node == null)
                return false;

            if (node.ContentType.Alias != DocTypeAlias)
            {
                Logger.Debug<AlternativeTemplateContentFinder>(
                    "Node at {Route} is not a {ExpectedDocType} but it is {ActualDocType}",
                    parentRoute, DocTypeAlias, node.ContentType.Alias);
                return false;
            }

            request.SetTemplate(_template);
            return true;
        }
    }
}