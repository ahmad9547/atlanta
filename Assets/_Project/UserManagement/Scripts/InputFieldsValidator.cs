using System.Linq;
using ProjectConfig.AdminCredentials;

namespace UserManagement
{
    public static class InputFieldsValidator
    {
        public static bool ValidateAdminName(string nickname)
        {
            return AdminUsersConfig.AdminCredentials.Any(user => user.UserName == nickname);
        }

        public static bool ValidateAdminNicknameAndPassword(string nickname, string password)
        {
            return AdminUsersConfig.AdminCredentials.Any(user => user.UserName == nickname && user.Password == password);
        }
    }
}
