using Metaverse.PresentationModule.Slides;
using System.Collections.Generic;
using UnityEngine;

namespace Metaverse.PresentationModule
{
    [CreateAssetMenu(fileName = "Presentation", menuName = "ScriptableObjects/PresentationAsset")]
    public sealed class Presentation : ScriptableObject
    {
        [SerializeReference] private List<Slide> _presentationSlides;

        public List<Slide> PresentationSlides => _presentationSlides;
    }
}
