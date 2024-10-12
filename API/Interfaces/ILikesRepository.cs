using System;
using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
    Task<PagedList<MemberDTO>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<int>> GetCurrentLikeIds(int currentUserId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
    Task<bool> SaveChanges();
}
