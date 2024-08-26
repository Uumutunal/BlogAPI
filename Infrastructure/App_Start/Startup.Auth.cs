using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Twitter;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.App_Start
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Example: Facebook Authentication
            //app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            //{
            //    AppId = "your-app-id",
            //    AppSecret = "your-app-secret"
            //});

            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = "fZ5Gzcg5IOCd42bUTYJ9otzvA",
                ConsumerSecret = "VBAOynNHd773gTVDsMUaeGX4IwMKMWq1XBs2WVKEtYQcLKvPTA",
                BackchannelCertificateValidator = null
            });

            // Add other authentication methods here
        }
    }
}
