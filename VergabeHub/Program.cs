using System.Diagnostics;
using VergabeHub.Data;
using VergabeHub.Models;


VergabeHubDbContext dbContext = new();

 if (dbContext.NoticePlatforms.Any(np => np.IsActive))
{
    foreach (NoticePlatform noticePlatform in dbContext.NoticePlatforms.Where(platform => platform.IsActive))
    {
        //Starts a new process for each active notice platform
        Process.Start(noticePlatform.ExecutableName!);
    }
}
else
{
    Console.Clear();
    Console.WriteLine("Nothing to do. There is no Notice Platform active in the database!");
}
