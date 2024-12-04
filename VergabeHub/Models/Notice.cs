namespace VergabeHub.Models;

public class Notice
{
    public int Id { get; set; }
    public DateTime DateOfPublication { get; set; }
    public DateTime SubmissionDeadline { get; set; }
    public string? ShortDescription { get; set; }
    public NoticeType? Type { get; set; }
    public ContractingAuthority? ContractingAuthority { get; set; }
    public string? Url { get; set; }

    //Navigation properties
    public NoticePlatform? NoticePlatform {  get; set; }
    public int NoticePlatformId { get; set; }

}
