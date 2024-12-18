using System;
using System.IdentityModel.Tokens.Jwt;
using Metaverse.ErrorHandling;
using Metaverse.ErrorHandling.Services;
using Microsoft.IdentityModel.Tokens;
using ProjectConfig.General;

namespace Core.JwtManagement.Services
{
    public sealed class JwtDecoder : IJwtDecoderService
    {
        private const string SecretKey = "ikgg2h5t-kkot95q40uw3fy$-qw2_fqbr8(0$%t40snjos=99r";

        private static SymmetricSecurityKey _key = new(System.Text.Encoding.UTF8.GetBytes(SecretKey));

        private static TokenValidationParameters _tokenValidationParameters = new()
        {
            IssuerSigningKey = _key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        public string DecodeToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
                JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(token);
                return securityToken.Payload.SerializeToJson();
            }
            catch (SecurityTokenInvalidSignatureException exception)
            {
                ErrorLogger.LogError(ErrorType.Error, $"Error validating token: {exception.Message}", GeneralSettings.GoBackOnErrorDefaultUrl);
                return null;
            }
            catch (Exception exception)
            {
                ErrorLogger.LogError(ErrorType.Error, $"Error decoding token: {exception.Message}", GeneralSettings.GoBackOnErrorDefaultUrl);
                return null;
            }
        }
    }
}