using Web.University.Domain;
using Microsoft.EntityFrameworkCore;

namespace Web.University;

public class DbConfigure
{
    // Ensure Database Compatibility level is at least 130.
    // EF 8 requires this for LINQ .Contains queries to work. 

    public static void EnsureCompatibilityLevel()
    {
        var db = ServiceLocator.Resolve<UniversityContext>();
        var env = ServiceLocator.Resolve<IWebHostEnvironment>();
        var name = Path.Combine(env.ContentRootPath, "Data\\University.mdf");
        var command = $"ALTER DATABASE \"{name}\" SET COMPATIBILITY_LEVEL = 130";
        db.Database.ExecuteSqlRaw(command);
    }
}

