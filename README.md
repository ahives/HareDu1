# HareDu

![Join the chat at https://gitter.im/HareDu2/Lobby](https://img.shields.io/gitter/room/haredu2/HareDu2?style=flat)
![NuGet downloads](https://img.shields.io/nuget/dt/haredu?style=flat)

HareDu is a .NET library for managing and monitoring RabbitMQ clusters.

HareDu is Apache 2.0 licensed.

### HareDu NuGet Packages

| Package Name |  | .NET Runtime |
|---| --- | --- |
| **API** |  |  |
| [HareDu.Core](https://www.nuget.org/packages/HareDu.Core/) | Configuration API | Framework 4.6.2 |
| [HareDu](https://www.nuget.org/packages/HareDu/) | Broker API | Framework 4.6.2 |
| [HareDu.Snapshotting](https://www.nuget.org/packages/HareDu.Snapshotting/) | Snapshot API | Framework 4.6.2 |
| [HareDu.Diagnostics](https://www.nuget.org/packages/HareDu.Diagnostics/) | Diagnostics API | Framework 4.6.2 |
| **Dependency Injection Containers** | | |
| [HareDu.AutofacIntegration](https://www.nuget.org/packages/HareDu.AutofacIntegration/) | Autofac Integration API | Framework 4.6.2 |
| [HareDu.CoreIntegration](https://www.nuget.org/packages/HareDu.CoreIntegration/) | .NET Core DI Integration API| Framework 4.6.2 |


## Get It
From the Package Manager Console in Visual Studio you can run the following PowerShell script to get the latest version of HareDu...

```
Install-Package HareDu
```

or if you want a specific version of HareDu you can do the following...

```
Install-Package -Version <version> HareDu
```

ex:

```
Install-Package -Version 1.3.0 HareDu
```

The above applies for any NuGet package you wish to install.

## Using HareDu with RabbitMQ

Under the "IntegrationTesting" solution folder you will find two projects of note, HareDu.IntegrationTesting.Publisher and HareDu.IntegrationTesting.Consumer, respectively. These projects use the popular OSS project MassTransit to interact with the RabbitMQ broker for sending and receiving messages. Follow the below steps in order to test the Diagnostic API.

1. Ensure that your RabbitMQ broker has the proper plugins enabled by following the [RabbitMQ documentation](https://www.rabbitmq.com/management.html#clustering) .
2. Create a VirtualHost called "TestVirtualHost"
   Note: This can be done by either using the Broker API or by logging in to the RabbitMQ UI and creating a vhost
3. Bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start a consumer:
   dotnet ~/<your_path_here>/HareDu2/src/Consumer/bin/Debug/netcoreapp2.1/HareDu.IntegrationTesting.Consumer.dll
4. Once the consumer(s) have been started, bring up a command prompt (e.g., Terminal on macOS) and execute the following command to start publishing messages:
    dotnet ~/<your_path_here>/HareDu2/src/Publisher/bin/Debug/netcoreapp2.1/HareDu.IntegrationTesting.Publisher.dll

Note: if you are using JetBrains Rider you can simply configure both projects and run them within the IDE.

**Enough with the talking, go check out the docs [here](https://github.com/ahives/HareDu1/blob/master/docs/README.md)**


# Dependencies
.NET 4.6.2 or above


# Debugging

If you find that making an API call is failing for reasons unknown, HareDu 2 introduces a way to return a text representation of the serialized JSON of the returned ```Result``` or ```Result<T>``` monads. Here is an example,

```c#
string debugText = result.ToJsonString();
```

That's it. So, the resulting output of calling the ```ToJsonString``` extension method might look something like this,

```json
{
  "timestamp": "2018-12-31T18:04:39.511627+00:00",
  "debugInfo": null,
  "errors": [
    {
      "reason": "RabbitMQ server did not recognize the request due to malformed syntax.",
      "timestamp": "2018-12-31T18:04:39.511303+00:00"
    }
  ],
  "hasFaulted": true
}
```

# Tested

|   | Version |
|---| --- |
| Operating System | macOS Catalina 10.15.3 |
| RabbitMQ | 3.7.x, 3.8.2 |
| Erlang OTP | 22.0.4 (x64) |
| .NET Runtime | Framework 4.6.2 |


# Changelist
You can find the changelist [here](https://github.com/ahives/HareDu1/blob/master/docs/changelist.md)
