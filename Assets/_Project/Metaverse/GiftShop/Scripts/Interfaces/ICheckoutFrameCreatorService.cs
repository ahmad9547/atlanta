using Core.ServiceLocator;

namespace Metaverse.GiftShop.Interfaces
{
    public interface ICheckoutFrameCreatorService : IService
    {
        public void CreateCheckoutIFrame(string dataJson);
    }
}
