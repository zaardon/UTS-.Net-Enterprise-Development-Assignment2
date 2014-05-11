using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlueConsultingManagementSystem.Startup))]
namespace BlueConsultingManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
