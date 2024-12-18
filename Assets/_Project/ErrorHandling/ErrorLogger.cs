using Core.ServiceLocator;

namespace Metaverse.ErrorHandling.Services
{
    internal static class ErrorLogger
    {
        private static IErrorHandlingService _errorHandlingService;

        private static IErrorHandlingService ErrorHandlingService =>
            _errorHandlingService ??= Service.Instance.Get<IErrorHandlingService>();

        public static void LogError(Error error) => ErrorHandlingService.ForceHandleError(error);

        public static void LogError(ErrorType type, string message, string url = null)
        {
            LogError(new Error(type, message, url));
        }
    }
}