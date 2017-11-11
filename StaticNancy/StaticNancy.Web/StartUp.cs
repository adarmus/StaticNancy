using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Nancy.Bootstrapper;
using Owin;
using StaticNancy.Logging;

[assembly: OwinStartup(typeof(StaticNancy.Web.StartUp))]

namespace StaticNancy.Web
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            new Log4NetInitialiser("Log4net.config").SetConfigurationFromFile();

            NancyBootstrapperLocator.Bootstrapper = new NancyBootstrapper();
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}