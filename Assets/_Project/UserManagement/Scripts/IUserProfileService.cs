using Core.ServiceLocator;

namespace UserManagement
{
    public interface IUserProfileService : IService
    {
        string AvatarLink { get; }
        string Nickname { get; }
        bool IsAdmin { get; }
        UserModel LocalUserModel { get; }

        void SetupUserProfile(UserModel userModel);
    }
}