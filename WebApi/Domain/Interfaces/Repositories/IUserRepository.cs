﻿using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserMetadataDto?> GetUserMetadata(int userId, CancellationToken cancellationToken);
        Task<User?> GetUserById(int userId);
    }
}
