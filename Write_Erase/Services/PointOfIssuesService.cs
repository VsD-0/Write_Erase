namespace Write_Erase.Services
{
    public class PointOfIssuesService
    {
        private readonly TradeContext _context;
        public PointOfIssuesService(TradeContext context)
        {
            _context = context;
        }
        public async Task<List<Point>> GetPoints() => await _context.Points.AsNoTracking().ToListAsync();
    }
}
