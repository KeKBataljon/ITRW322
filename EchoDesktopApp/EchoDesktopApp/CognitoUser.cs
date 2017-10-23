using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EchoDesktopApp
{
    public class CognitoUser : IdentityUser
    {
        public string Password { get; set; }
        public UserStatusType Status { get; set; }
    }
}
