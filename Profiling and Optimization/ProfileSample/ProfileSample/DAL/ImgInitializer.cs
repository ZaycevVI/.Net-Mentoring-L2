namespace ProfileSample.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;

    namespace ProfileSample.DAL
    {
        public class ImgInitializer : CreateDatabaseIfNotExists<MyImgContext>
        {
            protected override void Seed(MyImgContext context)
            {
                var imgPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Imgs";
                var imgs = Directory.GetFiles(imgPath);
                var imgsCache = new List<ImgSource>();
                var i = 0;
                foreach (var img in imgs)
                {
                    imgsCache.Add(new ImgSource
                    {
                        Id = i,
                        Data = File.ReadAllBytes(img),
                        Name = Path.GetFileName(img)
                    });

                    i++;
                }

                context.BulkInsert(imgsCache);

                context.SaveChanges();
                base.Seed(context);
            }
        }
    }
}