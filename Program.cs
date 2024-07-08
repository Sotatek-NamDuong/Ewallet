using Cassandra;
using EWallet.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IScyllaSession>(provider =>
{
    var cluster = Cluster.Builder()
        .AddContactPoint("your_scylla_host")
        .WithPort(9042) // ScyllaDB default port
        .Build();
    var session = cluster.Connect("your_keyspace");
    return new ScyllaSession(session);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
