using Autofac;
using PdfPackage.File;
using PdfPackage.Pdf;
using PdfPackage.Validator;
using Topshelf;

namespace WindowsService
{
    class Program
    {
        public const string InDir = "C:\\InDir";
        public const string OutDir = "C:\\OutDir";

        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Register(ctx => LoggingProxy<IValidator>.Create(new ImageNameValidator())).As<IValidator>();
            containerBuilder.Register(ctx => LoggingProxy<IPdfGenerator>.Create(new PdfSharpGenerator())).As<IPdfGenerator>();
            containerBuilder.Register(ctx => LoggingProxy<IPathConverter>.Create(new PathConverter())).As<IPathConverter>();
            containerBuilder.Register(ctx => LoggingProxy<IDirectoryListener>.Create(
                new DirectoryListener(InDir, OutDir, ctx.Resolve<IValidator>(), 
                    ctx.Resolve<IPdfGenerator>(), ctx.Resolve<IPathConverter>()))).As<IDirectoryListener>();
            var container = containerBuilder.Build();

            HostFactory.Run(conf =>
            {
                conf.SetServiceName("ImgToPdfService");
                conf.SetDisplayName("Image To Pdf Service");
                conf.Service(settings => new ImageToPdfService(container.Resolve<IDirectoryListener>()));
            });
        }
    }
}
