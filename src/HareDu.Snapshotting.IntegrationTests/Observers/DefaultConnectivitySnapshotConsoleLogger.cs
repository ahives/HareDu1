namespace HareDu.Snapshotting.IntegrationTests.Observers
{
    using System;
    using Core.Extensions;
    using Extensions;
    using Model;

    public class DefaultConnectivitySnapshotConsoleLogger :
        IObserver<SnapshotContext<BrokerConnectivitySnapshot>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SnapshotContext<BrokerConnectivitySnapshot> value)
        {
            var snapshot = value.Select(x => x.Snapshot);

            if (snapshot.IsNull())
                return;

            Console.WriteLine("Summary");

            if (!snapshot.ConnectionsCreated.IsNull() && snapshot.ConnectionsClosed.IsNull())
            {
                Console.WriteLine("Connections => {0} created | {1:0.0}/s, {2} closed | {3:0.0}/s",
                    snapshot.ConnectionsCreated.Total,
                    snapshot.ConnectionsCreated.Rate,
                    snapshot.ConnectionsClosed.Total,
                    snapshot.ConnectionsClosed.Rate);
            }

            var connections = snapshot.Select(x => x.Connections);

            if (connections.IsNull())
                return;
            
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].IsNull())
                    continue;

                Console.WriteLine("Connection => {0}", connections[0].Identifier);
                Console.WriteLine("Channel Limit => {0}", connections[i].OpenChannelsLimit);
                Console.WriteLine("Channels => {0}", connections[i].Channels.Count);

                Console.WriteLine("Network Traffic");
                
                if (!connections[i].NetworkTraffic.IsNull())
                {
                    if (!connections[i].NetworkTraffic.Sent.IsNull())
                    {
                        Console.WriteLine("\tSent: {0} packets | {1} | {2} msg/s",
                            connections[i].NetworkTraffic.Sent.Total,
                            $"{connections[i].NetworkTraffic.Sent.Bytes} bytes ({connections[i].NetworkTraffic.Sent.Bytes.ToByteString()})",
                            connections[i].NetworkTraffic.Sent.Rate);
                    }
                    
                    if (!connections[i].NetworkTraffic.Received.IsNull())
                    {
                        Console.WriteLine("\tReceived: {0} packets | {1} | {2} msg/s",
                            connections[i].NetworkTraffic.Received.Total,
                            $"{connections[i].NetworkTraffic.Received.Bytes} bytes ({connections[i].NetworkTraffic.Received.Bytes.ToByteString()})",
                            connections[i].NetworkTraffic.Received.Rate);
                    }
                }

                Console.WriteLine("Channels");
                for (int j = 0; j < connections[i].Channels.Count; j++)
                {
                    if (connections[i].Channels[j].IsNull())
                        continue;
                    
                    Console.WriteLine("\tChannel => {0}, Consumers => {1}",
                        connections[i].Channels[j].Identifier,
                        connections[i].Channels[j].Consumers);
                }

                Console.WriteLine("****************************");
                Console.WriteLine();
            }
        }
    }
}