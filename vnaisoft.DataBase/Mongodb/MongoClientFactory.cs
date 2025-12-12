using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using quan_ly_kho.DataBase.Mongodb;
using System;
using System.Collections.Generic;

public interface IMongoClientFactory
{
    MongoDBContext CreateClientDatabase(string databaseName);
    IMongoDatabase GetDatabase(string tenantId);
}

public class MongoClientFactory : IMongoClientFactory
{
    private readonly IConfiguration _configuration;

    private readonly Dictionary<string, MongoClient> _mongoClients = new();

    public MongoClientFactory(IConfiguration configuration)
    {
        _configuration = configuration;

        // Ví dụ load từ file, DB, hoặc config cứng
        // _tenantMap = LoadTenantMap(); // school_0001 → (mongoA, school_0001)
    }
    //private Dictionary<string, (string ServerKey, string Database)> LoadTenantMap()
    //{
    //    // Có thể load từ Redis, DB, JSON file hoặc cấu hình động
    //    return new Dictionary<string, (string, string)> {
    //        { "school_0001", ("mongoA", "school_0001") },
    //        { "school_5001", ("mongoB", "school_5001") }
    //    };
    //}
    private MongoClient GetClientForServer(string serverKey)
    {
        if (_mongoClients.TryGetValue(serverKey, out var existing))
            return existing;
        var connectionString = serverKey;
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception($"Missing connection string for server: {serverKey}");

        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.LinqProvider = LinqProvider.V3;
        var client = new MongoClient(settings);
        _mongoClients[serverKey] = client;
        return client;
    }
    public MongoDBContext CreateClientDatabase(string databaseName)
    {

        var client = GetClientForServer(_configuration.GetConnectionString("MongoDB"));
        var database = client.GetDatabase(databaseName);
        return new MongoDBContext(database);
    }


    public IMongoDatabase GetDatabase(string tenantId)
    {
        return CreateClientDatabase(tenantId)._database;
    }
}