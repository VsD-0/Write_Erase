namespace Write_Erase.Services
{
    public class PointOfIssuesService
    {
        private readonly StoreContext _context;
        public PointOfIssuesService(StoreContext context)
        {
            _context = context;
        }
        public async Task<List<Orderpickuppoint>> GetPoints() => await _context.Orderpickuppoints.AsNoTracking().ToListAsync();
    }
}
