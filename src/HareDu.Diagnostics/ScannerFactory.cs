namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Probes;
    using Scanners;
    using Snapshotting;

    /// <summary>
    /// This class should be instantiate once since construction can be expensive.
    /// </summary>
    public class ScannerFactory :
        IScannerFactory
    {
        readonly IKnowledgeBaseProvider _kb;
        readonly ConcurrentDictionary<string, object> _scannerCache;
        readonly ConcurrentDictionary<string, DiagnosticProbe> _probeCache;
        readonly IList<IDisposable> _observers;
        readonly HareDuConfig _config;

        public IReadOnlyDictionary<string, DiagnosticProbe> Probes => _probeCache;

        public IReadOnlyDictionary<string, object> Scanners => _scannerCache;

        public ScannerFactory(HareDuConfig config, IKnowledgeBaseProvider kb)
        {
            _config = !config.IsNull() ? config : throw new HareDuDiagnosticsException($"{nameof(config)} argument missing.");
            _kb = !kb.IsNull() ? kb : throw new HareDuDiagnosticsException($"{nameof(kb)} argument missing.");
            _scannerCache = new ConcurrentDictionary<string, object>();
            _probeCache = new ConcurrentDictionary<string, DiagnosticProbe>();
            _observers = new List<IDisposable>();
            
            if (!TryRegisterAllProbes())
                throw new HareDuDiagnosticsException("Could not register diagnostic probes.");
            
            if (!TryRegisterAllScanners())
                throw new HareDuDiagnosticsException("Could not register diagnostic scanners.");
        }

        public bool TryGet<T>(out DiagnosticScanner<T> scanner)
            where T : Snapshot
        {
            Type type = typeof(T);
            
            if (type.IsNull() || !_scannerCache.ContainsKey(type.FullName))
            {
                scanner = new NoOpScanner<T>();
                return false;
            }
            
            scanner = (DiagnosticScanner<T>) _scannerCache[type.FullName];
            return true;
        }

        public void RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers)
        {
            var probes = _probeCache.Values.ToList();
            
            for (int i = 0; i < observers.Count; i++)
            {
                if (observers[i].IsNull())
                    continue;
                
                for (int j = 0; j < probes.Count; j++)
                    _observers.Add(probes[j].Subscribe(observers[i]));
            }
        }

        public void RegisterObserver(IObserver<ProbeContext> observer)
        {
            if (observer.IsNull())
                return;
            
            var probes = _probeCache.Values.ToList();
            
            for (int j = 0; j < probes.Count; j++)
                _observers.Add(probes[j].Subscribe(observer));
        }

        public bool RegisterProbe<T>(T probe)
            where T : DiagnosticProbe
        {
            if (probe.IsNull())
                return false;
            
            return _probeCache.TryAdd(typeof(T).FullName, probe);
        }

        public bool RegisterScanner<T>(DiagnosticScanner<T> scanner)
            where T : Snapshot
        {
            if (scanner.IsNull())
                return false;

            return _scannerCache.TryAdd(typeof(T).FullName, scanner);
        }

        public bool TryRegisterProbe<T>(T probe)
            where T : DiagnosticProbe
        {
            bool added = _probeCache.TryAdd(typeof(T).FullName, probe);

            if (probe.IsNull() || !added)
                return false;
            
            foreach (var scanner in _scannerCache)
            {
                var method = scanner.Value
                    .GetType()
                    .GetMethod("Configure");
                
                method.Invoke(scanner.Value, new[] {_probeCache.Values});
            }

            return added;
        }

        public bool TryRegisterScanner<T>(DiagnosticScanner<T> scanner)
            where T : Snapshot =>
            scanner.IsNotNull() && _scannerCache.TryAdd(typeof(T).FullName, scanner);

        public bool TryRegisterAllProbes()
        {
            var typeMap = GetProbeTypeMap(GetType());
            bool registered = true;

            foreach (var type in typeMap)
            {
                if (_probeCache.ContainsKey(type.Key))
                    continue;
                
                registered = RegisterProbeInstance(type.Value, type.Key) & registered;
            }

            if (!registered)
                _probeCache.Clear();

            return registered;
        }

        public bool TryRegisterAllScanners()
        {
            var typeMap = GetScannerTypeMap(GetType());
            bool registered = true;

            foreach (var type in typeMap)
            {
                if (_scannerCache.ContainsKey(type.Key))
                    continue;
                
                registered = RegisterScannerInstance(type.Value, type.Key) & registered;
            }

            if (!registered)
                _scannerCache.Clear();

            return registered;
        }

        protected virtual bool RegisterScannerInstance(Type type, string key)
        {
            try
            {
                var instance = CreateScannerInstance(type);

                if (instance.IsNull())
                    return false;

                return _scannerCache.TryAdd(key, instance);
            }
            catch
            {
                return false;
            }
        }

        protected virtual object CreateScannerInstance(Type type)
        {
            var instance = Activator.CreateInstance(type, _probeCache.Values.ToList());

            return instance;
        }
        
        protected virtual IDictionary<string, Type> GetScannerTypeMap(Type findType)
        {
            var types = findType
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && !x.IsGenericType)
                .ToList();
            var typeMap = new Dictionary<string, Type>();

            foreach (var type in types)
            {
                if (type.IsNull())
                    continue;

                if (!type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(DiagnosticScanner<>)))
                    continue;

                var genericType = type
                    .GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(DiagnosticScanner<>));

                if (genericType.IsNull())
                    continue;

                string key = genericType.GetGenericArguments()[0].FullName;

                if (typeMap.ContainsKey(key))
                    continue;

                typeMap.Add(key, type);
            }

            return typeMap;
        }

        protected virtual IDictionary<string, Type> GetProbeTypeMap(Type type)
        {
            var types = type
                .Assembly
                .GetTypes()
                .Where(x => typeof(DiagnosticProbe).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            var typeMap = new Dictionary<string, Type>();

            for (int i = 0; i < types.Count; i++)
            {
                if (types[i].IsNull() || typeMap.ContainsKey(types[i].FullName))
                    continue;
                
                typeMap.Add(types[i].FullName, types[i]);
            }

            return typeMap;
        }

        protected virtual bool RegisterProbeInstance(Type type, string key)
        {
            try
            {
                var instance = CreateProbeInstance(type);

                if (instance.IsNull())
                    return false;

                return _probeCache.TryAdd(key, instance);
            }
            catch
            {
                return false;
            }
        }

        protected virtual DiagnosticProbe CreateProbeInstance(Type type)
        {
            var instance = type.GetConstructors()[0].GetParameters()[0].ParameterType == typeof(DiagnosticsConfig)
                && type.GetConstructors()[0].GetParameters()[1].ParameterType == typeof(IKnowledgeBaseProvider)
                    ? (DiagnosticProbe) Activator.CreateInstance(type, _config.Diagnostics, _kb)
                    : (DiagnosticProbe) Activator.CreateInstance(type, _kb);
            
            return instance;
        }
    }
}