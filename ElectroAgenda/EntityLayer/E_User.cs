using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class E_User
    {
        public E_User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { set; get; }
        public string Password { set; get; }
    }
}
