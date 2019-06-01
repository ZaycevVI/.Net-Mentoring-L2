using System.Collections.Generic;
using Autofac;
using Autofac.Core;
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

            containerBuilder.RegisterType<ImageNameValidator>().As<IValidator>();
            containerBuilder.RegisterType<PdfSharpGenerator>().As<IPdfGenerator>();
            containerBuilder.RegisterType<DirectoryListener>()
                .As<IDirectoryListener>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("inDir", InDir),
                    new NamedParameter("outDir", OutDir),
                });
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
