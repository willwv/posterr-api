using Domain.Utils;

namespace WebApi.Controllers.Requests
{
    public record CreatePostRequest 
    {
        public string? PostContent { get; set; }
        public bool IsRepost { get; set; }
        public bool IsQUote { get; set; }
        public string? Quote { get; set; }
        public int? OriginalPostId { get; set; }
        public IList<string>? InvalidRequestMessage { get; private set; }

        public bool IsValidRequest() 
        {
            InvalidRequestMessage = new List<string>();
            bool isValidPost = IsValidPost(InvalidRequestMessage);
            bool isValidQuote = IsValidQuote(InvalidRequestMessage);
            bool isValidRepost = IsValidRepost(InvalidRequestMessage);

            return isValidPost ^ isValidQuote ^ isValidRepost;
        }

        private bool IsValidPost(IList<string> InvalidRequestMessage)
        {
            if (IsQUote)
            {
                InvalidRequestMessage.Add("Param IsQuote must be false to create a post.");
            }

            if (IsRepost)
            {
                InvalidRequestMessage.Add("Param IsRepost must be false to create a post.");
            }

            if (Quote != null)
            {
                InvalidRequestMessage.Add("Param Quote must be null to create a post.");
            }

            if (OriginalPostId != null)
            {
                InvalidRequestMessage.Add("Param OriginalPostId must be null to create a post.");
            }

            if(PostContent != null && PostContent.Length > Constants.POST_CONTENT_CHAR_LIMIT) 
            {
                InvalidRequestMessage.Add("Posts can have a maximum of 777 characters.");
            }

            return IsQUote == false && IsRepost == false && Quote == null && OriginalPostId == null && PostContent != null && PostContent.Length <= Constants.POST_CONTENT_CHAR_LIMIT;
        }

        private bool IsValidRepost(IList<string> InvalidRequestMessage)
        {
            if (IsQUote)
            {
                InvalidRequestMessage.Add("Param IsQuote must be false when IsRepost is true.");
            }

            if (Quote != null)
            {
                InvalidRequestMessage.Add("Param Quote must be null when IsRepost is true.");
            }

            if(OriginalPostId == null)
            {
                InvalidRequestMessage.Add("Param OriginalPostId must be defined when IsRepost is true.");
            }

            return IsRepost && OriginalPostId != null && Quote == null && IsQUote == false;
        }

        private bool IsValidQuote(IList<string> InvalidRequestMessage)
        {
            if (IsRepost)
            {
                InvalidRequestMessage.Add("Param IsRepost must be false when IsQUote is true.");
            }

            if (Quote == null)
            {
                InvalidRequestMessage.Add("Param Quote must be defined when IsQUote is true.");
            }

            if (OriginalPostId == null)
            {
                InvalidRequestMessage.Add("Param OriginalPostId must be defined when IsQUote is true.");
            }

            return IsQUote && OriginalPostId != null && Quote != null && IsRepost == false;
        } 
     };
}
