using Core.ServiceLocator;

namespace Core.UrlParametersParsing
{
    public interface IUrlParameterParserService : IService
    {
        string GetUrlParameter(string parameterName);
    }
}