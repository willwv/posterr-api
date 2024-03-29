﻿using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<IList<Post>> GetAllPostsAsync(int userId, bool onlyMine, DateTime? fromDate, int skip, int take, CancellationToken cancellationToken);
        Task<Post> CreatePostAsync(Post post, CancellationToken cancellationToken);
        Task<bool> VerifyUsersPostsToday(int userId);
        Task<bool> VerifyPostIsRepost(int originalPostId);
        Task<bool> VerifyPostIsQuote(int originalPostId);
    }
}
