using System;
using System.Collections.Generic;
using System.Text;

namespace TechnicalChallenge.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(string username);
    }
}
