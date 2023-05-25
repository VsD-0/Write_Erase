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

        public async Task<List<string>> GetAllLogin()
        {
            return await _context.Users.Select(u => u.UserLogin).AsNoTracking().ToListAsync();
        }

        public async Task AddNewUser(string UserName, string UserSurname, string UserPatronymic, string UserLogin, string UserPassword)
        {
            try
            {
                Debug.WriteLine($"Values: {UserName}, {UserSurname}, {UserPatronymic}, {UserLogin}, {UserPassword}");
                var user = new User
                {
                    UserName = UserName,
                    UserSurname = UserSurname,
                    UserPatronymic = UserPatronymic,
                    UserLogin = UserLogin,
                    UserPassword = UserPassword,
                    UserRole = 2
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                Debug.WriteLine(user.UserId);
                var current = _context.Users.SingleOrDefault(u => u.UserId == user.UserId);
                Debug.WriteLine(current.UserLogin);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
