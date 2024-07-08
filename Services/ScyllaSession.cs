using Cassandra;
using Cassandra.Mapping;
using ISession = Cassandra.ISession;

namespace EWallet.API.Services;

public interface IScyllaSession
{
    Task<RowSet> ExecuteAsync(IStatement statement);
    PreparedStatement Prepare(string cql);
}

public class ScyllaSession(ISession session) : IScyllaSession
{
    public async Task<RowSet> ExecuteAsync(IStatement statement)
    {
        return await session.ExecuteAsync(statement);
    }

    public PreparedStatement Prepare(string cql)
    {
        return session.Prepare(cql);
    }
}