# Intro

A work in progress as I learn more about [CosmosDb Apache Gremlin](https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/introduction).

## Getting Started

1. Read *__all__* bullet points before starting to get anything done.
2. This page will help you get your database setup: [Quickstart: Azure Cosmos DB for Apache Gremlin library for .NET](https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/quickstart-dotnet)
2. Set secrets for the following account:

| Step # | Key | Dotnet CLI |
| -- | -- | -- |
| 5 | _gremlinEndpointUrl | dotnet user-secrets set "_gremlinEndpointUrl" "YOUR GREMLIN ENDPOINT URL" |
| 6 | _cosmosDbAuthKey | dotnet user-secrets set "_cosmosDbAuthKey" "YOUR COSMOS DB AUTH KEY" |
| 8 | _databaseName | dotnet user-secrets set "_databaseName" "YOUR COSMOS DATABASE NAME" |
| 9 | _graphName | dotnet user-secrets set "_graphName" "YOUR GRAPH NAME" |

4. Run the `PopulatePeople` unit test.
5. Note that the graph should now be populated.
6. Run the `FindParents` theory unit test.

## Recipes

<https://tinkerpop.apache.org/docs/current/recipes/>
