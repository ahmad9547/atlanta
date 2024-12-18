using System.Collections.Generic;

namespace ProjectConfig.AdminCredentials
{
    public static class AdminUsersConfig
    {
        private static List<AdminUserModel> _adminUsers;

        public static IEnumerable<AdminUserModel> AdminCredentials => _adminUsers;

        public static void SetupAdminUsers(AdminUsersModel adminUsersModel)
        {
            _adminUsers = adminUsersModel.AdminUsers;
        }
    }
}