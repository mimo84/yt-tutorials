import * as pulumi from "@pulumi/pulumi";
import * as resources from "@pulumi/azure-native/resources";
import * as storage from "@pulumi/azure-native/storage";
import * as servicebus from "@pulumi/azure-native/servicebus";
import * as web from "@pulumi/azure-native/web";
import * as sql from "@pulumi/azure-native/sql";

// Create an Azure Resource Group
const resourceGroup = new resources.ResourceGroup("resourceGroup");

// Create an Azure resource (Storage Account)
const storageAccount = new storage.StorageAccount("sa", {
  resourceGroupName: resourceGroup.name,
  sku: {
    name: storage.SkuName.Standard_LRS,
  },
  kind: storage.Kind.StorageV2,
});

// Export the primary key of the Storage Account
const storageAccountKeys = storage.listStorageAccountKeysOutput({
  resourceGroupName: resourceGroup.name,
  accountName: storageAccount.name,
});

const serviceBus = new servicebus.Namespace("servicebus", {
  location: "Australia East",
  namespaceName: "mauriziopulumisb",
  resourceGroupName: resourceGroup.name,
  sku: {
    name: servicebus.SkuName.Standard,
    tier: servicebus.SkuTier.Standard,
  },
});

const mySampleQueue = new storage.Queue("queue", {
  accountName: storageAccount.name,
  queueName: "myqueue",
  resourceGroupName: resourceGroup.name,
});

const namespaceAuthorizationRule = new servicebus.NamespaceAuthorizationRule(
  "namespaceAuthorizationRule",
  {
    authorizationRuleName: "sdk-authrule-send-listen",
    namespaceName: serviceBus.name,
    resourceGroupName: resourceGroup.name,
    rights: [servicebus.AccessRights.Listen, servicebus.AccessRights.Send],
  }
);

// servicebus.listNamespaceKeys({
//   resourceGroupName: resourceGroup.name,
//   namespaceName: serviceBus.name,
//   authorizationRuleName: namespaceAuthorizationRule.name,
// });

const keys = pulumi
  .all([resourceGroup.name, serviceBus.name, namespaceAuthorizationRule.name])
  .apply(([resourceGroupName, namespaceName, authorizationRuleName]) =>
    servicebus.listNamespaceKeys({
      resourceGroupName,
      namespaceName,
      authorizationRuleName,
    })
  );

const appServicePlan = new web.AppServicePlan("asp", {
  resourceGroupName: resourceGroup.name,
  kind: "App",
  sku: {
    name: "B1",
    tier: "Basic",
  },
});

const username = "mydbuser";

// Get the password to use for SQL from config.
const config = new pulumi.Config();
const pwd = config.require("sqlPassword");

const sqlServer = new sql.Server("sqlserver", {
  resourceGroupName: resourceGroup.name,
  administratorLogin: username,
  administratorLoginPassword: pwd,
  version: "12.0",
});

const database = new sql.Database("db", {
  resourceGroupName: resourceGroup.name,
  serverName: sqlServer.name,
  sku: {
    name: "S0",
  },
});

const storageContainer = new storage.BlobContainer("container", {
  resourceGroupName: resourceGroup.name,
  accountName: storageAccount.name,
  publicAccess: storage.PublicAccess.None,
});

const blob = new storage.Blob("blob", {
  resourceGroupName: resourceGroup.name,
  accountName: storageAccount.name,
  containerName: storageContainer.name,
  source: new pulumi.asset.FileArchive("wwwroot"),
});

const blobSAS = storage.listStorageAccountServiceSASOutput({
  accountName: storageAccount.name,
  protocols: storage.HttpProtocol.Https,
  sharedAccessStartTime: "2021-01-01",
  sharedAccessExpiryTime: "2030-01-01",
  resource: storage.SignedResource.C,
  resourceGroupName: resourceGroup.name,
  permissions: storage.Permissions.R,
  canonicalizedResource: pulumi.interpolate`/blob/${storageAccount.name}/${storageContainer.name}`,
  contentType: "application/json",
  cacheControl: "max-age=5",
  contentDisposition: "inline",
  contentEncoding: "deflate",
});
const token = blobSAS.apply((x) => x.serviceSasToken);

export const codeBlobUrl = pulumi.interpolate`https://${storageAccount.name}.blob.core.windows.net/${storageContainer.name}/${blob.name}?${token}`;

export const connectionString = keys.primaryConnectionString;

const app = new web.WebApp("webapp", {
  resourceGroupName: resourceGroup.name,
  serverFarmId: appServicePlan.id,
  siteConfig: {
    appSettings: [
      {
        name: "WEBSITE_RUN_FROM_PACKAGE",
        value: codeBlobUrl,
      },
    ],
    connectionStrings: [
      {
        name: "db",
        connectionString: pulumi
          .all([sqlServer.name, database.name])
          .apply(
            ([server, db]) =>
              `Server=tcp:${server}.database.windows.net;initial catalog=${db};user ID=${username};password=${pwd};Min Pool Size=0;Max Pool Size=30;Persist Security Info=true;`
          ),
        type: web.ConnectionStringType.SQLAzure,
      },
    ],
  },
});

export const endpoint = pulumi.interpolate`https://${app.defaultHostName}`;
