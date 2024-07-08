using Cassandra;

namespace EWallet.API.Services;

public class ReportService
{
    private readonly IScyllaSession _session;

    public ReportService(IScyllaSession session)
    {
        _session = session;
    }

    public async Task<int> GetActiveUsersCountAsync(DateTime start, DateTime end)
    {
        var result = await _session.ExecuteAsync(new SimpleStatement(
            "SELECT COUNT(DISTINCT user_id) as count FROM active_users WHERE date >= ? AND date <= ?",
            start.Date, end.Date));
        return result.First().GetValue<int>("count");
    }

    public async Task<int> GetTransactionCountAsync(DateTime start, DateTime end)
    {
        var result = await _session.ExecuteAsync(new SimpleStatement(
            "SELECT SUM(count) as total FROM transaction_counts WHERE date >= ? AND date <= ?",
            start.Date, end.Date));
        return result.First().GetValue<int>("total");
    }

    public async Task<decimal> GetTransactionVolumeAsync(DateTime start, DateTime end)
    {
        var result = await _session.ExecuteAsync(new SimpleStatement(
            "SELECT SUM(volume) as total FROM transaction_volumes WHERE date >= ? AND date <= ?",
            start.Date, end.Date));
        return result.First().GetValue<decimal>("total");
    }
}