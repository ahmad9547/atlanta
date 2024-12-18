using System.Runtime.InteropServices;
using Metaverse.GiftShop.Interfaces;

namespace Metaverse.GiftShop.Services
{
    public sealed class CheckoutFrameCreator : ICheckoutFrameCreatorService
    {
        [DllImport("__Internal")]
        private static extern void CreateIFrame(string dataJson);

        public void CreateCheckoutIFrame(string dataJson)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            CreateIFrame(dataJson);
        }
    }
}