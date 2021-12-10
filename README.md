# BDD based testing on API Management APIs

## basic idea (not yet a concept)

Define API tests using Gherkin, specifying an API operation to be called, inputs to be used, product subscription to be used and then examine API behaviour by capturing the call on a to be injected mocked backend and inspecting the HTTP headers and HTTP payloads passed through.

Proposed syntax

```text
Feature: Order processing
 
Prepare and reformat order for backend system.
 
Scenario: Valid orders
Pass through a valid orders
Given I pass in an order from file <Filename>
And I inject backend with named value order-processing-backend
When I use API Order processing operation POST Order
Then status code returned should be <Result>
Examples:
| Filename       | Result |
| order1.json    | 200    |
| order2.json    | 200    |
```

## configuration

az ad sp create-for-rbac -n "APIM testing" --role "API Management Service Contributor" --scopes /subscriptions/$(az account show --query id -o tsv)

note values and put into environment variables

```shell
export subscriptionId="..."
export tenantId="..."
export clientId="..."
export clientSecret="..."
export resourceGroupName="..."
export serviceName="..."
```
