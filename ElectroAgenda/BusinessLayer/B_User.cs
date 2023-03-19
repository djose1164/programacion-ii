using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityLayer;
using DataLayer;

namespace BusinessLayer
{
    public class B_User
    {
        public bool validateLogin(E_User user)
        {
            return d_user.validate(user);
        }

        public bool registerUser(E_User user)
        {
            return d_user.register(user);
        }

        private D_User d_user = new D_User();
    }
}
