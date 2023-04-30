using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackerWeb.Entity
{
    public class Account
    {
        public int Id { get; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        public Account(int id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }

        public void EditLogin(string newLogin)
        {
            if (newLogin == null)
                throw new ArgumentNullException("str", "Text string cannot be null");

            Login = newLogin;
        }

        public void EditPassword(string newPassword)
        {
            if (newPassword == null)
                throw new ArgumentNullException("str", "Text string cannot be null");

            Password = newPassword;
        }

        public override string? ToString()
        {
            var res = new StringBuilder();

            res.Append("Login: ").Append(Login).Append("\n")
                .Append("Password: ").Append(Password);

            return res.ToString();
        }
    }
}
