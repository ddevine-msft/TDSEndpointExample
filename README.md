# Dataverse TDS Enpoint Connection Example

## Introduction
This is a Basic Proof Of Concept that demonstrates one possible way to connect and query a Dataverse TDS endpoint using .Net Standard. As it's a simple POC error handling and coding best practices are not fully implemented.

## Supportablity Disclaimer 
Currently the [Dataverse TDS Documentation](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/dataverse-sql-query) indicates TDS access via code is in preview. I discourage using this in production until it is out of preview.

## Prerequisites
Confirm you can query using SMSS per the [Dataverse TDS Documentation](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/dataverse-sql-query) when using the authentication method of 'Microsoft Entra Service Principal' 

## Security
I do not recommend hard coding your credentials in your code or config file. Use a secure method to store and retrieve your credentials like Azure Key Vault and Windows Credential Manager.

## Getting Started
Set the following values in the appsettings.json file:
```json
﻿{
  "AppSettings": {
    "SQLConnectionString": "Server=DATAVERSE URL.REGION.dynamics.com,5558;Database=DB FROM SSMS;Encrypt=True",
    "TenantId": "",
    "ClientId": "",
    "ClientSecret": ""
  }
}
```

The SQL query is hard coded in the Program.cs. The code is written to return the first row of the query. If you  modify the query you might need to modfiy the code that reads the results. 
```csharp
string queryString = "select[Total Row Count] = count(*) from account";
```

## Disclaimer 
``` text
# DISCLAIMER
# This software (or sample code) is not supported under any Microsoft standard
# support program or service. The software is provided AS IS without warranty
# of any kind. Microsoft further disclaims all implied warranties including,
# without limitation, any implied warranties of merchantability or of fitness
# for a particular purpose. The entire risk arising out of the use or
# performance of the software and documentation remains with you. In no event
# shall Microsoft, its authors, or anyone else involved in the creation,
# production, or delivery of the software be liable for any damages whatsoever
# (including, without limitation, damages for loss of business profits, business
# interruption, loss of business information, or other pecuniary loss) arising
# out of the use of or inability to use the software or documentation, even if
# Microsoft has been advised of the possibility of such damages.