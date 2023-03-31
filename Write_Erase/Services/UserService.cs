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
                    UserRole = user.UserRole
                };
                return true;
            }
            return false;
        }
    }
}
