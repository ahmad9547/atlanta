using UnityEngine;

namespace ProjectConfig.General
{
    public static class GeneralSettings
    {
        private const string DefaultVersion = "0.0.0";

        private static string _version;
        private static bool _showDebugInfo;
        private static string _contentFolderUrl;
        private static bool _isAmphitheatreWatchersEnabled;
        private static string _addressablesBundlesUrl;
        private static string _goBackOnErrorDefaultUrl;

        public static string Version
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_version))
                {
                    Debug.LogError("SettingsConfig version is null, default version is set");
                    return DefaultVersion;
                }

                return _version;
            }
        }

        public static bool ShowDebugInfo => _showDebugInfo;
        public static string ContentFolderUrl => _contentFolderUrl;
        public static bool IsAmphitheatreWatchersEnabled => _isAmphitheatreWatchersEnabled;
        public static string AddressablesBundlesUrl => _addressablesBundlesUrl;
        public static string GoBackOnErrorDefaultUrl => _goBackOnErrorDefaultUrl;

        public static void SetupGeneralSettings(GeneralSettingsModel generalSettingsModel)
        {
            _version = generalSettingsModel.Version;
            _showDebugInfo = generalSettingsModel.ShowDebugInfo;
            _contentFolderUrl = generalSettingsModel.ContentFolderUrl;
            _isAmphitheatreWatchersEnabled = generalSettingsModel.AmphitheatreWatchersEnabled;
            _addressablesBundlesUrl = generalSettingsModel.AddressablesBundlesUrl;
            _goBackOnErrorDefaultUrl = generalSettingsModel.GoBackOnErrorDefaultUrl;
        }
    }
}
