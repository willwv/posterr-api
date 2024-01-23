using Domain.Utils;

namespace WebApi.Controllers.Requests
{
    public record CreatePostRequest 
    {
        public string PostContent { get; set; }
        public string? InvalidRequestMessage { get; private set; }
        public bool IsValidRequest() 
        {
            bool isValidRequest = PostContent.Length <= Constants.POST_CONTENT_CHAR_LIMIT;

            if(!isValidRequest)
            {
                InvalidRequestMessage = Constants.POST_CONTENT_CHAR_LIMIT_MESSAGE;

            }

            return isValidRequest;
        }
     };
}
