using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using money.guardian.api.settings;

namespace money.guardian.api.services;

public class TokenGenerator : ITokenGenerator
{
    private readonly IdentitySettings _settings;
    private readonly SymmetricSecurityKey _key;
    private readonly JwtSecurityTokenHandler _tokenHandler = new ();


    public TokenGenerator(IOptions<IdentitySettings> settings)
    {
        _settings = settings.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
    }

    public string Generate(IEnumerable<Claim> claims)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(8)),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);

        return _tokenHandler.WriteToken(token);
    }
}