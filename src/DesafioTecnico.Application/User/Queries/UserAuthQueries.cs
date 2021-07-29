using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechnicalChallenge.Domain.Exceptions;
using TechnicalChallenge.Domain.Interfaces;

namespace TechnicalChallenge.Application.User.Queries
{
    public class UserAuthQueries : IUserAuthQueries
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptService _encryptService;
        private readonly IJwtService _jwtService;

        public UserAuthQueries(IUserRepository userRepository, IEncryptService encryptService,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _encryptService = encryptService;
            _jwtService = jwtService;
        }

        public async Task<AccessTokenVM> GetAccessToken(AuthModel model)
        {
            var user = await _userRepository.FindUserByUserName(model.UserName);

            if(user == null)
            {
                throw new NotFoundException($"User '{model.UserName}' not found");
            }

            if(!_encryptService.VerifyPassword(model.Password, user.Password))
            {
                throw new UserDomainException("Wrong password. Please try again");
            }

            var result = _jwtService.GenerateAccessToken(user.UserName);

            return new AccessTokenVM()
            {
                access_token = result,
                token_type = "Bearer"
            };
        }

        public async Task<FindUserModel> GetUserByUsername(string username)
        {
            var user = await _userRepository.FindUserByUserName(username);

            if (user == null)
            {
                throw new NotFoundException($"User '{username}' not found");
            }

            return new FindUserModel()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
