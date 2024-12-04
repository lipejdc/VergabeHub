using VergabeHub.Data;
using VergabeHub.Models;

namespace VergabeHub.DatabaseFunctions;

public static class DatabaseOperator
{
    private static VergabeHubDbContext _dbContext = new();


    public static async Task ProcessNoticesAndAddToDatabase(List<Notice> notices, int noticePlatformId)
    {
        foreach (var newNotice in notices)
        {
            var existingNotice = _dbContext.Notices.FirstOrDefault(n => n.Url == newNotice.Url);
            if (existingNotice == null)
            {
                await ProcessNewNotice(newNotice, noticePlatformId);
            }
        }
    }

    private static async Task ProcessNewNotice(Notice newNotice, int noticePlatformId)
    {
        var existingContractingAuthority = FindOrCreateContractingAuthority(newNotice.ContractingAuthority!.Name!);
        var existingNoticeType = FindOrCreateNoticeType(newNotice.Type!.Type!);

        newNotice.NoticePlatformId = noticePlatformId;
        newNotice.ContractingAuthority = existingContractingAuthority;
        newNotice.Type = existingNoticeType;

        _dbContext.Notices.Add(newNotice);
        await _dbContext.SaveChangesAsync();
    }

    private static ContractingAuthority FindOrCreateContractingAuthority(string name)
    {
        var existingContractingAuthority = _dbContext.ContractingAuthorities
            .FirstOrDefault(ca => ca.Name == name);

        if (existingContractingAuthority == null)
        {
            existingContractingAuthority = new ContractingAuthority { Name = name };
            _dbContext.ContractingAuthorities.Add(existingContractingAuthority);
        }

        return existingContractingAuthority;
    }

    private static NoticeType FindOrCreateNoticeType(string type)
    {
        var existingNoticeType = _dbContext.NoticeTypes
            .FirstOrDefault(nt => nt.Type == type);

        if (existingNoticeType == null)
        {
            existingNoticeType = new NoticeType { Type = type };
            _dbContext.NoticeTypes.Add(existingNoticeType);
        }

        return existingNoticeType;
    }
}