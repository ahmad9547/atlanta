using UnityEngine;
using TMPro;

namespace PlayerUIScene.DebugFunctionality
{
    public sealed class FPSIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        private float deltaTime;

        private void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }

        private void LateUpdate()
        {
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = $"{msec:0.0}ms {fps:0.} FPS [ {Time.timeSinceLevelLoad:0} ]";
            _label?.SetText(text);
        }
    }
}