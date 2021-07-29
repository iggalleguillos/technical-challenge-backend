using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalChallenge.Domain.Entities;
using TechnicalChallenge.Domain.Interfaces;

namespace TechnicalChallenge.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BasicAWSCredentials _credentials;

        public UserRepository()
        {
            _credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("DYNAMODB_ACCESS_KEY"),
                Environment.GetEnvironmentVariable("DYNAMODB_SECRET_KEY"));
        }

        public async Task<User> AddUserAsync(User user)
        {
            var client = new AmazonDynamoDBClient(_credentials, RegionEndpoint.USEast2);
            var table = Table.LoadTable(client, "User");

            var context = new DynamoDBContext(client);

            var result = await table.PutItemAsync(context.ToDocument(user));

            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var client = new AmazonDynamoDBClient(_credentials, RegionEndpoint.USEast2);

            var updateItemRequest = new UpdateItemRequest()
            {
                TableName = "User",
                Key = new Dictionary<string, AttributeValue> { { "UserName", new AttributeValue { S = user.UserName } } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":email", new AttributeValue{ S = user.Email }}
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#E", "Email"}
                },
                UpdateExpression = "SET #E = :email"
            };

            await client.UpdateItemAsync(updateItemRequest);

            return true;
        }

        public async Task<User> FindUserByIdAsync(string userId)
        {
            var client = new AmazonDynamoDBClient(_credentials, RegionEndpoint.USEast2);
            var table = Table.LoadTable(client, "User");

            var result = await table.GetItemAsync(userId);

            return MapUserWithPassword(result);
        }

        public async Task<User> FindUserByUserName(string username) 
        {
            var client = new AmazonDynamoDBClient(_credentials, RegionEndpoint.USEast2);

            var query = new QueryRequest
            {
                TableName = "User",
                KeyConditionExpression = "UserName = :username",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":username", new AttributeValue { S = username } } }
            };

            var result = await client.QueryAsync(query);

            if(result.Count == 0)
            {
                throw new Exception("User not exists");
            }

            return MapUser(result.Items.FirstOrDefault());
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var client = new AmazonDynamoDBClient(_credentials, RegionEndpoint.USEast2);

            var deleteRequest = new DeleteItemRequest
            {
                TableName = "User",
                Key = new Dictionary<string, AttributeValue> { { "UserName", new AttributeValue { S = username } } }
            };

            await client.DeleteItemAsync(deleteRequest);

            return true;
        }

        private User MapUser(Document document)
        {
            var user = new User(document["UserId"], document["UserName"], string.Empty, document["Email"]);

            return user;
        }

        private User MapUserWithPassword(Document document)
        {
            var user = new User(document["UserId"], document["UserName"], string.Empty, document["Email"]);

            return user;
        }

        private User MapUser(Dictionary<string, AttributeValue> item)
        {
            var user = new User(item["UserId"].S, item["UserName"].S, item["Password"].S, item["Email"].S);

            return user;
        }
    }
}
