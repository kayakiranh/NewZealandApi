﻿using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    /// <summary>
    /// Entity Framework Repository for "WalkRoute" entity
    /// </summary>
    public interface IWalkRouteRepository
    {
        Task<List<WalkRoute>> GetAllAsync(string? queryName = null, bool? isOrderName = null, bool? isLengthOrder = null, int? page = null);

        Task<WalkRoute> GetByIdAsync(Guid id);

        Task<WalkRoute> CreateAsync(WalkRoute model);

        Task<WalkRoute> UpdateAsync(Guid id, WalkRoute model);

        Task<WalkRoute> DeleteAsync(Guid id);
    }
}