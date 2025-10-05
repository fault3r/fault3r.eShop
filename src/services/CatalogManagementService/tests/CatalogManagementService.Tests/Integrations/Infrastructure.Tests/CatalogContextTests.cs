
using System;
using CatalogManagementService.Infrastructure.Data.Contexts;
using CatalogManagementService.Infrastructure.Data.Documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogManagementService.Tests.Integrations.Infrastructure.Tests
{
    public class CatalogContextTests
    {
        [Fact]
        public void CatalogContext_CanCommunicateWithMongo()
        {
            string databaseName = "TestDatabase";
            string collectionName = "TestCollection";
            var client = new MongoClient("mongodb://localhost:27017");
            var context = new CatalogContext(client, databaseName, collectionName);
            string expected = "TestItem";
            try
            {
                context.Documents.InsertOne(new ItemDocument { Name = expected });
                var actual = context.Documents.Find(p => p.Name == expected).FirstOrDefault();
 
                Assert.Equal(actual.Name, expected);
                Assert.NotEqual(actual.Id, ObjectId.Empty);

                client.DropDatabase(databaseName);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
        
    }
}