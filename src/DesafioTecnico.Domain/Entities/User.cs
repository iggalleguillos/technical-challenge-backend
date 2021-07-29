using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TechnicalChallenge.Domain.Exceptions;

namespace TechnicalChallenge.Domain.Entities
{
    public class User
    {
        //Se implementan entidades de dominio usando el principio Tell don´t ask
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }

        public User(string userId, string userName, string password, string email)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
        }

        public User()
        {

        }
        public void VerifyEmail()
        {
            if(!Regex.IsMatch(this.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase))
            {
                throw new UserDomainException("Email is not valid");
            }
        }

    }
}
