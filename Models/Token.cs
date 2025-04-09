using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tennis.Models;

public class Token(string customerUserName, string tokenValue, DateTime expiry)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int TokenID { get; set; }

    [Required]
    public string CustomerUserName { get; set; } = customerUserName;

    [Required]
    public string TokenValue { get; set; } = tokenValue;

    [Required]
    public DateTime Expiry { get; set; } = expiry;
}
