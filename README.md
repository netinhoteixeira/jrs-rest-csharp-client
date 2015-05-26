Rest Client for JasperReports Server [![Build Status](https://travis-ci.org/netinhoteixeira/jrs-rest-csharp-client.svg?branch=master)](https://travis-ci.org/netinhoteixeira/jrs-rest-csharp-client) [![Coverage Status](https://coveralls.io/repos/netinhoteixeira/jrs-rest-csharp-client/badge.png?branch=master)](https://coveralls.io/r/netinhoteixeira/jrs-rest-csharp-client?branch=master)
=========================================

With this library you can easily write C# applications which can interact with one or more JasperReports servers simultaneously in a very simple way. Library provides very friendly API for user, it minimizes possibility of building wrong requests.

Introduction
-------------
With this library you can easily write C# applications which can interact with one or more JasperReports servers simultaneously in a very simple way. Library provides very friendly API for user, it minimizes possibility of building wrong requests. To use library in your application you need just to add NuGet with command
```
$ Install-Package JasperServer.Client
```

Report services
===============
After you've configured the client you can easily use any of available services. Here's the code:
```csharp
JasperserverRestClient jasperserverRestClient = new JasperserverRestClient("username", "password", "http://localhost:8080/jasperserver/rest_v2/reports");
```

####Running a report:
There are two approaches to run a report - in synchronous and asynchronous modes.
To run report in synchronous mode you can use the code below:
```csharp
Stream stream = jasperserverRestClient.Get("/reports/anotherfolder/ReportId.pdf"); // could be xls, html, ...
```

To run report in asynchronous mode you can use the code below:
```csharp
await Task.Run(() => Stream stream = jasperserverRestClient.Get("/reports/anotherfolder/ReportId.pdf"));
```

####Passing parameters:
If you want to pass parameters you need to use the following code:
```csharp
Dictionary<string, string> parameters = new Dictionary<string, string>();
parameters.Add("PARAM1", "VALUE1");
parameters.Add("PARAM2", "VALUE2");

Stream stream = jasperserverRestClient.Get("/reports/anotherfolder/ReportId.pdf", parameters);
```

####Saving directly to file:
You could save directly to file using this method:
```csharp
jasperserverRestClient.Get("/reports/anotherfolder/ReportId.pdf", "path/ReportId.pdf");
````

Or passing parameters:
```csharp
jasperserverRestClient.Get("/reports/anotherfolder/ReportId.pdf", parameters, "path/ReportId.pdf");
````
