namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting.Model;

    public class FakeBrokerQueuesSnapshot1 :
        BrokerQueuesSnapshot
    {
        public FakeBrokerQueuesSnapshot1(ulong total)
        {
            Churn = new BrokerQueueChurnMetricsImpl(total);
            Queues = GetQueues().ToList();
        }

        public string ClusterName { get; }
        public BrokerQueueChurnMetrics Churn { get; }
        public IReadOnlyList<QueueSnapshot> Queues { get; }

        IEnumerable<QueueSnapshot> GetQueues()
        {
            yield return new FakeQueueSnapshot("FakeQueue1",
                "FakeVirtualHost",
                "FakeNode",
                83,
                33,
                8929,
                62,
                93,
                2,
                1.57M,
                new DateTimeOffset(2019, 8, 20, 8, 0, 55, TimeSpan.Zero));
        }

        
        class BrokerQueueChurnMetricsImpl :
            BrokerQueueChurnMetrics
        {
            public BrokerQueueChurnMetricsImpl(ulong total)
            {
                NotRouted = new QueueDepthImpl(total);
            }

            
            class QueueDepthImpl :
                QueueDepth
            {
                public QueueDepthImpl(ulong total)
                {
                    Total = total;
                }

                public ulong Total { get; }
                public decimal Rate { get; }
            }

            public long Persisted { get; }
            public QueueDepth Incoming { get; }
            public QueueDepth Unacknowledged { get; }
            public QueueDepth Ready { get; }
            public QueueDepth Gets { get; }
            public QueueDepth GetsWithoutAck { get; }
            public QueueDepth Delivered { get; }
            public QueueDepth DeliveredWithoutAck { get; }
            public QueueDepth DeliveredGets { get; }
            public QueueDepth Redelivered { get; }
            public QueueDepth Acknowledged { get; }
            public QueueDepth NotRouted { get; }
            public QueueDepth Broker { get; }
        }

        
        class FakeQueueSnapshot :
            QueueSnapshot
        {
            public FakeQueueSnapshot(string name,
                string virtualHost,
                string node,
                ulong target,
                ulong total,
                ulong bytes,
                ulong unacknowledged,
                ulong ready,
                ulong consumers,
                decimal consumerUtilization,
                DateTimeOffset idleSince)
            {
                Identifier = name;
                VirtualHost = virtualHost;
                Node = node;
                Consumers = consumers;
                ConsumerUtilization = consumerUtilization;
                IdleSince = idleSince;
                Memory = new FakeQueueMemory(target, total, bytes, unacknowledged, ready);
                Messages = new QueueChurnMetricsImpl();
            }

            
            class QueueChurnMetricsImpl :
                QueueChurnMetrics
            {
                public QueueChurnMetricsImpl()
                {
                    Incoming = new QueueDepthImpl(768578, 3845.5M);
                    Unacknowledged = new QueueDepthImpl(8293, 774.5M);
                    Ready = new QueueDepthImpl(8381, 3433.5M);
                    Gets = new QueueDepthImpl(934, 500.5M);
                    GetsWithoutAck = new QueueDepthImpl(0, 0M);
                    Delivered = new QueueDepthImpl(7339, 948.5M);
                    DeliveredWithoutAck = new QueueDepthImpl(34, 5.5M);
                    DeliveredGets = new QueueDepthImpl(0, 0M);
                    Redelivered = new QueueDepthImpl(768578, 3845.5M);
                    Acknowledged = new QueueDepthImpl(9238, 8934.5M);
                    Aggregate = new QueueDepthImpl(823847, 9847.5M);
                }

                
                class QueueDepthImpl :
                    QueueDepth
                {
                    public QueueDepthImpl(ulong total, decimal rate)
                    {
                        Total = total;
                        Rate = rate;
                    }

                    public ulong Total { get; }
                    public decimal Rate { get; }
                }

                public QueueDepth Incoming { get; }
                public QueueDepth Unacknowledged { get; }
                public QueueDepth Ready { get; }
                public QueueDepth Gets { get; }
                public QueueDepth GetsWithoutAck { get; }
                public QueueDepth Delivered { get; }
                public QueueDepth DeliveredWithoutAck { get; }
                public QueueDepth DeliveredGets { get; }
                public QueueDepth Redelivered { get; }
                public QueueDepth Acknowledged { get; }
                public QueueDepth Aggregate { get; }
            }

            public string Identifier { get; }
            public string VirtualHost { get; }
            public string Node { get; }
            public QueueChurnMetrics Messages { get; }
            public QueueMemoryDetails Memory { get; }
            public QueueInternals Internals { get; }
            public ulong Consumers { get; }
            public decimal ConsumerUtilization { get; }
            public DateTimeOffset IdleSince { get; }

            
            class FakeQueueMemory :
                QueueMemoryDetails
            {
                public FakeQueueMemory(ulong target, ulong total, ulong bytes, ulong unacknowledged, ulong ready)
                {
                    RAM = new FakeRAM(target, total, bytes, unacknowledged, ready);
                    PagedOut = new FakePagedOut(3);
                }

                public ulong Total { get; }
                public PagedOut PagedOut { get; }
                public RAM RAM { get; }

                
                class FakePagedOut :
                    PagedOut
                {
                    public FakePagedOut(ulong total)
                    {
                        Total = total;
                    }

                    public ulong Total { get; }
                    public ulong Bytes { get; }
                }
                

                class FakeRAM :
                    RAM
                {
                    public FakeRAM(ulong target, ulong total, ulong bytes, ulong unacknowledged, ulong ready)
                    {
                        Target = target;
                        Total = total;
                        Bytes = bytes;
                        Unacknowledged = unacknowledged;
                        Ready = ready;
                    }

                    public ulong Target { get; }
                    public ulong Total { get; }
                    public ulong Bytes { get; }
                    public ulong Unacknowledged { get; }
                    public ulong Ready { get; }
                }
            }
        }
    }
}