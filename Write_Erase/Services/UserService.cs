namespace Write_Erase.Services
{
    public class UserService
    {
        private readonly StoreContext _context;
        public UserService(StoreContext context)
        {
            _context = context;
        }
        public async Task<bool> AuthorizationAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserLogin == username);
            List<Role> roles = await _context.Roles.ToListAsync();
            if (user == null)
                return false;
            if (user.UserPassword.Equals(password))
            {
                Global.CurrentUser = new UserModel
                {
                    Id = user.UserId,
                    UserName = user.UserName,
                    UserSurname = user.UserSurname,
                    UserPatronymic = user.UserPatronymic,
                    UserRole = roles.SingleOrDefault(rl => rl.RoleId == user.UserRole).RoleName,
                };
                return true;
            }
            return false;
        }
    }
}
