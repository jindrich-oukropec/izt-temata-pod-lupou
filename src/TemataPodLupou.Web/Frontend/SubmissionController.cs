using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using TemataPodLupou.Web.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace TemataPodLupou.Web.Frontend
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
            [ContentPropertyDisplayName(nameof(SubmissionWidget.TitleFieldCaption))]
            public string Title { get; set; }

            [Required]
            [ContentPropertyDisplayName(nameof(SubmissionWidget.ReasonFieldCaption))]
            public string Reason { get; set; }

            [Required]
            [ContentPropertyDisplayName(nameof(SubmissionWidget.DescriptionFieldCaption))]
            public string Description { get; set; }

            [Required]
            [ContentPropertyDisplayName(nameof(SubmissionWidget.RegionFieldCaption))]
            public string Region { get; set; }

            public string Location { get; set; }

            public HttpPostedFileBase[] Files { get; set; }

            [Required]
            [ContentPropertyDisplayName(nameof(SubmissionWidget.EmailFieldCaption))]
            public string Email { get; set; }
        }

        public ActionResult RenderForm()
        {
            var viewModel = new ViewModel();
            return PartialView("_SubmissionForm", viewModel);
        }

        [HttpPost]
        [ValidateUmbracoFormRouteString]
        [ValidateRecaptcha]
        public ActionResult HandleSubmission(ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var content = Services.ContentService.Create(model.Title, CurrentPage.Id, Submission.ModelTypeAlias);
            SetValue(content, x => x.Title, model.Title);
            SetValue(content, x => x.Reason, model.Reason);
            SetValue(content, x => x.Description, model.Description);
            SetValue(content, x => x.Region, model.Region);
            SetValue(content, x => x.Location, model.Location);
            SetValue(content, x => x.Email, model.Email);

            if (model.Files != null)
            {
                var parentMedia = (CurrentPage as SubmissionWidget).MediaStoreFolder.MediaItem;

                var pickerValue = new List<Dictionary<string, string>>(model.Files.Length);
                foreach (var file in model.Files)
                {
                    if (file == null)
                        continue;

                    var media = _mediaService.CreateMedia(model.Title, parentMedia.Id, "File");
                    media.SetValue(_contentTypeBaseServiceProvider, Constants.Conventions.Media.File, file.FileName, file.InputStream);
                    _mediaService.Save(media);

                    pickerValue.Add(new Dictionary<string, string> {
                        {"key", Guid.NewGuid().ToString()},
                        {"mediaKey", media.Key.ToString()},
                        {"crops", null},
                        {"focalPoint", null}
                    });
                }
                var json = JsonConvert.SerializeObject(pickerValue);
                SetValue(content, x => x.Media, json);
            }

            Services.ContentService.SaveAndPublish(content);

            TempData.Add("Sent", true);
            return RedirectToCurrentUmbracoPage();
        }

        private static void SetValue<TValue>(IContentBase content, Expression<Func<Submission, TValue>> selector, object value)
        {
            var propertyAlias = Submission.GetModelPropertyType(selector).Alias;
            content.SetValue(propertyAlias, value);
        }
    }
}