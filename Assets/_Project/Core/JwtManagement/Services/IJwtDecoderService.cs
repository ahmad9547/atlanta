using Core.ServiceLocator;

namespace Core.JwtManagement.Services
{
    public interface IJwtDecoderService : IService
    {
        string DecodeToken(string token);
    }
}