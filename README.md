# Managed Service Authenitcation 


This POC consists of two services. 

DataAPI is an example of a Azure AD enterprise application. The appsetting.json for this file needs application (client) ID of the Azure AD Service Principal.
It can be accessed as. 
```
GET /data/v1/files/{fileid}
```

There are only two fileids in the in-memory database 101 and 202

ClientAPI is also a Restful service which in turns calls the DataAPI using Managed Identity. It can be invoked as 
```
GET /api/v1/Client/{fileid}
```

again use the fileids that are available in DataAPI for a successful test
