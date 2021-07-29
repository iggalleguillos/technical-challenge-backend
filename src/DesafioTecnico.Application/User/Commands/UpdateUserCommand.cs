using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechnicalChallenge.Domain.Interfaces;

namespace TechnicalChallenge.Application.User.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.Entities.User(string.Empty, request.UserName, string.Empty, request.Email);
            
            var result = await _userRepository.UpdateUserAsync(user);

            return result;
        }
    }

    public class UpdatedUserDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
