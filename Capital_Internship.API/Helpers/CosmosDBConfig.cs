using Microsoft.Azure.Cosmos;

namespace Capital_Internship.Helpers
{
    public class CosmosDBConfig
    {
        private readonly IConfiguration _configuration;

        public CosmosDBConfig(IConfiguration configuration) {
            _configuration = configuration;
        }
        public  async Task Setup()
        {
            using CosmosClient client = new(
                accountEndpoint: _configuration.GetValue<string>("AccountEndpoint"),
                authKeyOrResourceToken: _configuration.GetValue<string>("AccountKey"));
            Database database = await client.CreateDatabaseIfNotExistsAsync(id: _configuration.GetValue<string>("DatabaseName"), throughput: 400);

            await database.CreateContainerIfNotExistsAsync(
                id: "Program_",
                partitionKeyPath: "/id"
            );  
            
            await database.CreateContainerIfNotExistsAsync(
                id: "ApplicationRequirements",
                partitionKeyPath: "/id"
            ); 
            
            await database.CreateContainerIfNotExistsAsync(
                id: "AdditionalQuestions",
                partitionKeyPath: "/id"
            );

            await database.CreateContainerIfNotExistsAsync(
                id: "QuestionChoices",
                partitionKeyPath: "/id"
            );

            await database.CreateContainerIfNotExistsAsync(
                id: "CandidateApplications",
                partitionKeyPath: "/id"
            ); 

            await database.CreateContainerIfNotExistsAsync(
                id: "QuestionResponses",
                partitionKeyPath: "/id"
            );

            await database.CreateContainerIfNotExistsAsync(
                id: "SelectedMultiChoiceItems",
                partitionKeyPath: "/id"
            );

        }
    }
}
