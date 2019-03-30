using Microsoft.Owin;
using Owin;
using XamarinOfflineSyncWithAzure.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace XamarinOfflineSyncWithAzure.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}