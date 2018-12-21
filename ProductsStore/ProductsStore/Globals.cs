using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsStore
{
     static public class Globals
    {
        public const string ADMIN_SESSION_KEY = "Admin";
        public const string USER_SESSION_KEY = "User";
        public const string CART_SESSION_KEY = "Cart";

        static public int getConnectedUser(ISession session) {
            return (session.GetInt32(USER_SESSION_KEY) ?? 0);
        }

        static public bool isUserConnected(ISession session)
        {
            return getConnectedUser(session) != 0;
        }
         

        static public bool isAdminConnected(ISession session)
        {
            return session.GetInt32(ADMIN_SESSION_KEY) == 1;
        }
    }
}
