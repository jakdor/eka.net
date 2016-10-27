using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalRApp1.Startup))]
namespace SignalRApp1
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
