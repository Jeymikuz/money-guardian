using System.ComponentModel.DataAnnotations;

namespace money.guardian.api.settings;

public class IdentitySettings
{
    [Required] public string Key { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
}