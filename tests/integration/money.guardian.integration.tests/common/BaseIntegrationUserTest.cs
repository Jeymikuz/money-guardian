using System.Net.Http.Headers;
using System.Net.Http.Json;
using money.guardian.api.models.auth;

namespace money.guardian.integration.tests.common;

public abstract class BaseIntegrationUserTest(TestWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    protected async Task AuthenticateUser(string username, string password)
    {
        var user = new LoginModel(username, password);

        var result = await HttpClient.PostAsJsonAsync("api/v1/auth/login", user);
        var loginResult = await result.Content.ReadFromJsonAsync<LoginResultModel>();

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);
    }
}