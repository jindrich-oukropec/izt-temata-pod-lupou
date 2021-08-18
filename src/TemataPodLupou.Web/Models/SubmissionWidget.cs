namespace TemataPodLupou.Web.Models
{
    public partial class SubmissionWidget
    {
        public string SubmitButtonHoverColourOrDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(SubmitButtonHoverColour))
                    return SubmitButtonHoverColour;

                return SubmitButtonColour;
            }
        }

        public string SubmitButtonBorderColourOrDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(SubmitButtonBorderColour))
                    return SubmitButtonBorderColour;

                return SelectedInputBorderColour;
            }
        }

        public string SubmitButtonHoverBorderColourOrDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(SubmitButtonHoverBorderColour))
                    return SubmitButtonHoverBorderColour;

                return SubmitButtonBorderColourOrDefault;
            }
        }

        public string CollectionProcessCircleColourOrDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(CollectionProcessCircleColour))
                    return CollectionProcessCircleColour;

                return SubmitButtonColour;
            }
        }
    }
}