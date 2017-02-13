using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Umbraco_DEV.Startup))]
namespace Umbraco_DEV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
