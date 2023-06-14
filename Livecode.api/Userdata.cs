using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Livecode.Library;

namespace Livecode.api
{
    public static class Userdata
    {
        [FunctionName("Store")]
        public static async Task<IActionResult> Store([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function to add or update a user.");
            string userJson = await new StreamReader(req.Body).ReadToEndAsync();

            var user = JsonConvert.DeserializeObject<User>(userJson);
            if (user == null) return new BadRequestObjectResult(401);

            await CosmosUserHelper.CreateOrUpdateUserInCosmosDb(user);
            return new OkObjectResult($"success");
        }

        [FunctionName("Retrieve")]
        public static async Task<IActionResult> Retrieve([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function to retrieve all users.");
            var users = await CosmosUserHelper.ReadAllUserFromCosmosDb();
            return new OkObjectResult(users);
        }
    }
}
