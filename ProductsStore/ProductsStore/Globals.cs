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

        static public string getConnectedUser(ISession session) {
            return session.GetString(USER_SESSION_KEY);
        }

        static public bool isUserConnected(ISession session)
        {
            return !String.IsNullOrEmpty(getConnectedUser(session));
        }
         

        static public bool isAdminConnected(ISession session)
        {
            return !String.IsNullOrEmpty(session.GetString(ADMIN_SESSION_KEY));
        }
    }
}
