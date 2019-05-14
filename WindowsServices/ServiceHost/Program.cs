using Topshelf;

namespace WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(conf =>
            {
                conf.SetServiceName("ImgToPdfService");
                conf.SetDisplayName("Image To Pdf Service");
                conf.Service<ImageToPdfService>();
            });
        }
    }
}
