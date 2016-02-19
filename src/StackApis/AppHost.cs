using Funq;
using ServiceStack;
using ServiceStack.Admin;
using ServiceStack.Api.Swagger;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using StackApis.ServiceInterface;

namespace StackApis
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("StackApis", typeof(MyServices).Assembly) {}

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            this.Plugins.Add(new SwaggerFeature { UseBootstrapTheme = true });
            this.Plugins.Add(new PostmanFeature());
            this.Plugins.Add(new CorsFeature());

            Plugins.Add(new RazorFormat());
            Plugins.Add(new AutoQueryFeature
            {
                MaxLimit = 100,
                AutoQueryViewerConfig =
                {
                    ServiceDescription = "Search for ServiceStack Questions on StackOverflow",
                    ServiceIconUrl = "/Content/app/logo-76.png",
                    BackgroundColor = "#fc9a24",
                    TextColor = "#fff",
                    LinkColor = "#ffff8d",
                    BrandImageUrl = "/Content/app/brand.png",
                    BrandUrl = "http://stackapis.servicestack.net/",
                    BackgroundImageUrl = "/Content/app/bg.png",
                    IsPublic = true,
                }
            });
            Plugins.Add(new AdminFeature());

            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory("~/App_Data/db.sqlite".MapServerPath(), SqliteDialect.Provider));
        }
    }    
}