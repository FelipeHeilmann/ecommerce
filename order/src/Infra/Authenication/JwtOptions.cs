namespace Infra.Authenication;

public record JwtOptions(string Issuer, string Audience, string SecretKey);
