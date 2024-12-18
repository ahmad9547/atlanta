using TMPro;
using UnityEngine.UI;

namespace Core.UI
{
    public static class UIExtension
    {
        public static void Set(this TextMeshProUGUI textMesh, string value)
        {
            textMesh.SetText(value);
        }

        public static void Set(this TextMeshProUGUI textMesh, float value)
        {
            textMesh.SetText(value.ToString());
        }

        public static void Set(this TextMeshProUGUI textMesh, int value)
        {
            textMesh.SetText(value.ToString());
        }

        public static void Set(this TMP_InputField inputField, string value)
        {
            inputField.SetTextWithoutNotify(value);
        }

        public static void Set(this Text text, float value)
        {
            text.text = value.ToString();
        }

        public static void Set(this Toggle toggle, bool value)
        {
            toggle.isOn = value;
        }
    }
}