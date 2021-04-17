# Changelist

| Version | | Description | Type | Breaking Change? |
| --- | --- | --- | --- | --- |
| **1.3.0** | 1 | Fixed issue with create exchange not serializing request properly for type property | Bug Fix | No |
| | 2 | Changed AppliedTo property on OperatorPolicyInfo to be an enumeration | Enhancement | Yes |
| | 3 | Removed JSON.NET dependency and replaced with System.Text.Json parser introduced in C# 8 | Enhancement | No |
| | 4 | Introduced new object in Broker API called Shovel for creating, viewing, and deleting dynamic shovels that are used to move messages between exchanges and queues | Enhancement | No |
| | 5 | Changed method signatures for Broker API objects to make them less verbose | Enhancement | Yes |
| | 6 | Introduced extension methods for Broker API objects to call methods directly off of IBrokerObjectFactory | Enhancement | No |
| | 7 | Added support for configuring HareDu APIs in JSON | Enhancement | No |
| | 8 | Deprecated Peek method on Queue object in Broker API | Deprecated | Yes |
| | 9 | Changed SystemOverview object in Broker API to BrokerSystem | Enhancement | Yes |
| | 10 | Introduced new object in Broker API called OperatorPolicy | New | No |
| | 11 | Introduced new method overload on User object in Broker API called Delete and DeleteUsers extension method to perform bulk delete of users | New | No |
| | 12 | Introduced new method on Connection object in Broker API called Delete and DeleteConnection extension method to delete a connection to the RabbitMQ broker | New | No |
| | 13 | Deprecated method overloads for RegisterObserver and RegisterObservers in IScannerFactory | Deprecated | Yes |
| | 14 | Added more developer documentation (e.g., API intellisense) | Enhancement | No |
| | 15 | Fixed issue with QueueInfo.BackingQueueStatus.TargetTotalMessagesInRAM property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 16 | Fixed issue with QueueInfo.BackingQueueStatus.BackingQueueMode property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 17 | Fixed issue with QueueInfo.State property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 18 | Fixed issue with ChannelInfo.State property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 19 | Fixed issue with NodeInfo.RatesMode property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 20 | Fixed issue with ConnectionInfo.Type property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 21 | Marked API methods that are obsolete in developer documentation | New | No |
