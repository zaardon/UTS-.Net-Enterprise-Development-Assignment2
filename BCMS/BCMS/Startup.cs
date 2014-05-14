using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BCMS.Startup))]
namespace BCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
