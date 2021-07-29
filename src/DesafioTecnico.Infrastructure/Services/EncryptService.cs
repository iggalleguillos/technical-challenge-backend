using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechnicalChallenge.Domain.Interfaces;

namespace TechnicalChallenge.Infrastructure.Services
{
    public class EncryptService : IEncryptService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string requestPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(requestPassword, hashedPassword);
        }
    }
}
