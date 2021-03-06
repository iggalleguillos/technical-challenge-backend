using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechnicalChallenge.Domain.Entities;

namespace TechnicalChallenge.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<User> FindUserByIdAsync(string userId);
        Task<bool> DeleteUserAsync(string username);
        Task<User> FindUserByUserName(string username);
    }
}
