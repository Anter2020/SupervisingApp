using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NajmDefault.Startup))]
namespace NajmDefault
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
