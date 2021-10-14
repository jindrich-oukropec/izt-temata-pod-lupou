using TemataPodLupou.Web.Models;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web.Trees;

namespace TemataPodLupou.Web.Backoffice
{

    public class DownloadCsvTreeMenuItemComponent : IComponent
    {
        public class DownloadCsvTreeMenuItemComposer : IUserComposer
        {
            public void Compose(Composition composition)
            {
                // Append our component to the collection of Components
                // It will be the last one to be run
                composition.Components().Append<DownloadCsvTreeMenuItemComponent>();
            }
        }

        // register the event listener with a component:
        public void Initialize()
        {
            TreeControllerBase.MenuRendering += TreeControllerBase_MenuRendering;
        }

        public void Terminate()
        {
            // unsubscribe on shutdown
            TreeControllerBase.MenuRendering -= TreeControllerBase_MenuRendering;
        }

        // the event listener method:
        private void TreeControllerBase_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content")
                return;

            var nodeId = int.Parse(e.NodeId);
            var content = sender.Services.ContentService.GetById(nodeId);
            if (content.ContentType.Alias != SubmissionWidget.ModelTypeAlias)
                return;

            var i = new Umbraco.Web.Models.Trees.MenuItem("download", "Download Submissions");
            i.AdditionalData.Add("actionView", "../App_Plugins/Temata/dialogs/content/download-submissions.html");
            i.Icon = "download-alt";
            i.SeparatorBefore = true;
            e.Menu.Items.Add(i);
        }
    }
}