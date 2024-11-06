using System;

namespace ProjectConfig.General
{
    [Serializable]
    public sealed class GeneralSettingsModel
    {
        public string Version;
        public bool ShowDebugInfo;
        public string ContentFolderUrl;
        public bool AmphitheatreWatchersEnabled;
        public string AddressablesBundlesUrl;
        public string GoBackOnErrorDefaultUrl;
    }
}