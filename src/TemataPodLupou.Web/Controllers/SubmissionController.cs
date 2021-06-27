using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TemataPodLupou.Web.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace TemataPodLupou.Web.Controllers
{
    public class SubmissionController : SurfaceController
    {
        private readonly IMediaService _mediaService;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;

        public SubmissionController(IContentTypeBaseServiceProvider contentTypeBaseServiceProvider, IMediaService mediaService)
        {
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
            _mediaService = mediaService;
        }

        public class ViewModel
        {
            [Required]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }
            
            public HttpPostedFileBase File { get; set; }
            
            public string Email { get; set; }
        }
        
        public ActionResult RenderForm()
        {
            var viewModel = new ViewModel();
            return PartialView("_SubmissionForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateUmbracoFormRouteString]
        public ActionResult HandleSubmission(ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //TempData.Add("CustomMessage", "Failure");
                return CurrentUmbracoPage();
            }

            var content = Services.ContentService.Create(model.Title, CurrentPage.Id, Submission.ModelTypeAlias);
            SetValue(content, x => x.Title, model.Title);
            SetValue(content, x => x.Description, model.Description);
            SetValue(content, x => x.Email, model.Email);
            
            if (model.File != null)
            {
                var parentMedia = (CurrentPage as SubmissionForm).MediaStoreFolder.MediaItem;
                var media = _mediaService.CreateMediaWithIdentity(model.Title, parentMedia.Id, "File");
                media.SetValue(_contentTypeBaseServiceProvider, "umbracoFile", model.File.FileName, model.File.InputStream);
                _mediaService.Save(media);
                var publishedMedia = Umbraco.Media(media.Id);
                SetValue(content, x => x.Media, publishedMedia.Url());
            }
            
            Services.ContentService.SaveAndPublish(content);
            
            TempData.Add("Sent", true);
            return RedirectToCurrentUmbracoPage();
        }
        
        private void SetValue<TValue>(IContent content, Expression<Func<Submission, TValue>> selector, object value)
        {
            var propertyAlias = Submission.GetModelPropertyType(selector).Alias;
            content.SetValue(propertyAlias, value);
        }
    }
}