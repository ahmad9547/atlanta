using System.Runtime.InteropServices;

namespace Core.UrlParametersParsing
{
    public sealed class UrlParameterParser : IUrlParameterParserService
    {
        [DllImport("__Internal")]
        private static extern string GetQueryParameter(string paramName);

        public string GetUrlParameter(string parameterName)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return GetQueryParameter(parameterName);
#endif
            return null;
        }
    }
}