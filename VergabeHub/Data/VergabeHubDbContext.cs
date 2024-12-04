using Microsoft.EntityFrameworkCore;
using VergabeHub.Models;

namespace VergabeHub.Data;

public class VergabeHubDbContext : DbContext
{

    public VergabeHubDbContext()
    {

    }

    public VergabeHubDbContext(DbContextOptions<DbContext> options) : base(options)
    {

    
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VergabeHub;
            Integrated Security=True;Connect Timeout=30;Encrypt=False;
            Trust Server Certificate=False;Application Intent=ReadWrite;
            Multi Subnet Failover=False"
            );
    }

    public DbSet<ContractingAuthority> ContractingAuthorities { get; set; }
    public DbSet<Notice> Notices { get; set; }
    public DbSet<NoticePlatform> NoticePlatforms { get; set; }
    public DbSet<NoticeType> NoticeTypes { get; set; }
}
