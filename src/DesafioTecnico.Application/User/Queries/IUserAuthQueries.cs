using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalChallenge.Application.User.Queries
{
    public interface IUserAuthQueries
    {
        Task<AccessTokenVM> GetAccessToken(AuthModel model);
        Task<FindUserModel> GetUserByUsername(string username);
    }
}
