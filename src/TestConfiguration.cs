using Microsoft.Azure.Management.ApiManagement;
using Microsoft.Azure.Management.ApiManagement.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace Apim.Testing;
public class TestConfiguration : IConfigureTest
{
    private string? resourceGroupName = Environment.GetEnvironmentVariable(nameof(resourceGroupName));
    private string? serviceName = Environment.GetEnvironmentVariable(nameof(serviceName));
    private string? subscriptionId = Environment.GetEnvironmentVariable(nameof(subscriptionId));
    private string? tenantId = Environment.GetEnvironmentVariable(nameof(tenantId));
    private string? clientId = Environment.GetEnvironmentVariable(nameof(clientId));
    private string? clientSecret = Environment.GetEnvironmentVariable(nameof(clientSecret));
    
    readonly ApiManagementClient client;
    readonly ApiManagementServiceResource service;

    public TestConfiguration()
    {
        client = ConfigureClient();
        service = ConfigureService();
    }

    public IConfigureTest ConfigureNamedValue(string name, string value)
    {
        var nv = client.NamedValue.Get(resourceGroupName, serviceName, name);

        client.NamedValue.Update(resourceGroupName, serviceName, name, new NamedValueUpdateParameters()
        {
            Value = value,
        }, nv.Id);

        return this;
    }

    private ApiManagementClient ConfigureClient()
    {
        var context = new AuthenticationContext("https://login.windows.net/" + tenantId);
        ClientCredential cc = new ClientCredential(clientId, clientSecret);
        AuthenticationResult result = context.AcquireTokenAsync("https://management.azure.com/", cc).Result;
        ServiceClientCredentials cred = new TokenCredentials(result.AccessToken);

        return new ApiManagementClient(cred)
        {
            SubscriptionId = subscriptionId
        };
    }

    private ApiManagementServiceResource ConfigureService()
    {
        return client.ApiManagementService.Get(resourceGroupName, serviceName);
    }
}