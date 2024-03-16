namespace money.guardian.api.models.auth;

public record LoginResultModel(string Token, bool IsSuccessful, string ErrorMessage);