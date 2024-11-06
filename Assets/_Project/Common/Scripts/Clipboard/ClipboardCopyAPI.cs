using System.Runtime.InteropServices;

namespace Common.Clipboard
{
    public static class ClipboardCopyAPI
    {
        [DllImport("__Internal")]
        private static extern void CopyToClipboard(string text);

        public static void CopyTextToClipboard(string text)
        {
#if !UNITY_WEBGL || UNITY_EDITOR            
            return;
#endif
            CopyToClipboard(text);
        }
    }
}
