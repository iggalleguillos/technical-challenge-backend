using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechnicalChallenge.Domain.Interfaces;

namespace TechnicalChallenge.Application.User.Commands
{
    public class CreateUserCommand : IRequest<CreatedUserDTO>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptService _encryptService;

        public CreateUserCommandHandler(IUserRepository userRepository, IEncryptService encryptService)
        {
            _userRepository = userRepository;
            _encryptService = encryptService;
        }
        public async Task<CreatedUserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var encryptedPassword = _encryptService.HashPassword(request.Password);

            Domain.Entities.User user = new Domain.Entities.User(Guid.NewGuid().ToString(), request.UserName, encryptedPassword, 
                request.Email);

            var result = await _userRepository.AddUserAsync(user);

            return new CreatedUserDTO()
            {
                UserId = result.UserId,
                UserName = result.UserName,
                Email = result.Email
            };
        }
    }

    public class CreatedUserDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
