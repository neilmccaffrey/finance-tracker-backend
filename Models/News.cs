using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("guardian_news")]
public class GuardianNews
{
    [Key]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
}