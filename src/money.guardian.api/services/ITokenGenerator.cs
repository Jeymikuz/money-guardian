using System.Security.Claims;

namespace money.guardian.api.services;

public interface ITokenGenerator
{
    string Generate(IEnumerable<Claim> claims);
}