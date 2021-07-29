using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalChallenge.Domain.Interfaces
{
    public interface IEncryptService
    {
        string HashPassword(string password);
        bool VerifyPassword(string requestPassword, string hashedPassword);
    }
}
