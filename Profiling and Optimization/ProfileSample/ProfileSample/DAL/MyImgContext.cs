using ProfileSample.DAL.ProfileSample.DAL;

namespace ProfileSample.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MyImgContext : DbContext
    {
        // Your context has been configured to use a 'MyImgContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ProfileSample.DAL.MyImgContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MyImgContext' 
        // connection string in the application configuration file.
        public MyImgContext()
            : base("name=MyImgContext")
        {
            Database.SetInitializer(new ImgInitializer());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<ImgSource> Imgs { get; set; }
    }
}