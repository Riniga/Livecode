using Microsoft.Azure.Cosmos;
namespace Livecode.Library
{
    public class CosmosUserHelper
    {
        private static string partitionKeyPath = "/fingerprint";
        private static CosmosData data = new CosmosData();

        public static async Task CreateOrUpdateUserInCosmosDb(User user)
        {
            var userFromDatabase = await ReadUserFromCosmosDb(user.Fingerprint);
            if (userFromDatabase != null) await DeleteUserFromCosmosDb(userFromDatabase.Fingerprint);
                        
            Console.WriteLine("Create user with fingerprint: " + user.Fingerprint);
            user.Timestamp = DateTime.UtcNow;
            var container = await GetContainer();
            await container.CreateItemAsync<User>(user, new PartitionKey(user.Fingerprint.ToString()));
        }
        public static async Task<List<User>> ReadAllUserFromCosmosDb()
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Users");
            var container = await GetContainer();
            var queryResultSetIterator = container.GetItemQueryIterator<User>(queryDefinition);
            var users = new List<User>();
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                users.AddRange(currentResultSet);
            }
            return users;
        }
        public static async Task<User> ReadUserFromCosmosDb(string fingerprint)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Users WHERE Users.fingerprint='" + fingerprint + "'");
            var container = await GetContainer();
            var queryResultSetIterator = container.GetItemQueryIterator<User>(queryDefinition);
            List<User> users = new List<User>();
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                users.AddRange(currentResultSet);
            }
            return users.FirstOrDefault();
        }
        internal static async Task DeleteUserFromCosmosDb(string fingerprint)
        {
            Console.WriteLine("Delete user with fingerprint: " + fingerprint);
            var container = await GetContainer();
            await container.DeleteItemAsync<User>(fingerprint, new PartitionKey(fingerprint));
        }
        private static async Task<Container> GetContainer()
        {
            var cosmosClient = new CosmosClient(data.EndpointUrl, data.PrimaryKey);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(data.databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(data.usersContainerId, partitionKeyPath);
            return container;
        }
    }
}
