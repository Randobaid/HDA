using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HDA.Core.Startup))]
namespace HDA.Core
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}