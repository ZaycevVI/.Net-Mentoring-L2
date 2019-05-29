using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new MyImgContext();

            var sources = context.Imgs.Take(20).ToList();
            
            var model = new List<ImageModel>();

            foreach (var src in sources)
            {
                var obj = new ImageModel()
                {
                    Name = src.Name,
                    Data = src.Data
                };

                model.Add(obj);
            } 

            return View(model);
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");
            var imgCache = new List<ImgSource>();
            using (var context = new MyImgContext())
            {
                Parallel.ForEach(files, file =>
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        var buff = new byte[stream.Length];

                        stream.Read(buff, 0, (int)stream.Length);

                        imgCache.Add(new ImgSource
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        });
                    }
                });

                context.BulkInsert(imgCache);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}