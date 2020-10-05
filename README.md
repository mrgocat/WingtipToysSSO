# WingtipToysSSO
Single Sign On service using JWT and Azure Cosmos(Mongo DB) and AWS DynamoDB

Configurations in appsettings.json file.
JWT setting information :::
  "jwt": {
    "JwtKey": "JWT_KEY_atleast 16 characters length", 
    "JwtIssuer": "http://wingtiptoys.com", 
    "JwtAudience": "WingtipToysClient", 
    "JwtExpireDays": 15 
  }

Database setting information :::
  "DatabaseSettings": {
    "DBType": "DynamoDB", // DynamoDB or MongoDB
    "DynamoDB": {  // AWS DynamoDB configuration. If you select DynamoDB you have to set these values.
      "ServiceURL": "https://dynamodb.us-east-2.amazonaws.com", 
      "AccessKey": "", // IAM AccessKey for DynamoDB
      "SecretKey": "+Ceq0u56TdH7CoX9YfWjCn" // IAM SecretKey for DynamoDB
    },
    "ConnectionString": " azure connection string here ", // Azure Cosmos DB connection string. If you select MongoDB you have to set this value. 
    "DatabaseName": "wingtipsso" // If you select MongoDB you have to set this value.
  }
