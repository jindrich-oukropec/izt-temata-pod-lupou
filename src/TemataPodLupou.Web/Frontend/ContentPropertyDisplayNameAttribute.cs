using System.ComponentModel;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace TemataPodLupou.Web.Frontend
{
    public class ContentPropertyDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _propertyAlias;

        public ContentPropertyDisplayNameAttribute(string propertyAlias)
        {
            _propertyAlias = propertyAlias;
        }

        public override string DisplayName
        {
            get
            {
                var accessor = Current.Factory.GetInstance<IUmbracoContextAccessor>();
                var context = accessor.UmbracoContext;
                var content = context.PublishedRequest.PublishedContent;
                return content.GetProperty(_propertyAlias).Value<string>();
            }
        }
    }
}