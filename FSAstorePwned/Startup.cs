using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FSAstorePwned.Startup))]
namespace FSAstorePwned
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
