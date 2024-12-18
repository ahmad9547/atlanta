using Metaverse.PresentationModule.Slides;
using UnityEditor;
using UnityEngine;

namespace Metaverse.PresentationModule
{
    [CustomEditor(typeof(Presentation))]
    public sealed class PresentationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Presentation presentation = (Presentation)target;

            if (GUILayout.Button("Add Image slide"))
            {
                presentation.PresentationSlides.Add(new ImageSlide());
            }

            if (GUILayout.Button("Add Video slide"))
            {
                presentation.PresentationSlides.Add(new VideoSlide());
            }
        }
    }
}
