using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JiraJWL.Startup))]
namespace JiraJWL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
