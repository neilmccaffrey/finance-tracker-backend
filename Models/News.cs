using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("guardian_news")]
public class GuardianNews
{
    [Key]
    public string Id { get; set; }
    public string WebTitle { get; set; }
    public string WebUrl { get; set; }
}