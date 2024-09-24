using BestHB.Domain.Entities;
using System.Collections.Concurrent;

namespace BestHB.Repository.InMemory;

internal static class InMemoryIntrumentInfoRepository
{
    private static readonly ConcurrentDictionary<string, InstrumentInfo> _instrumentsInfo = new();
    private static readonly object _readLock = new();
    private static readonly object _writeLock = new();

    public static void Add(InstrumentInfo instrumentInfo)
    {
        if (Get(instrumentInfo.Symbol) == null)
        {
            lock (_writeLock)
            {
                _instrumentsInfo.TryAdd(instrumentInfo.Symbol, instrumentInfo);
            }
        }
    }

    public static InstrumentInfo Get(string symbol)
    {
        lock (_readLock)
        {
            if (_instrumentsInfo.ContainsKey(symbol))
            {
                return _instrumentsInfo[symbol];
            }
            else
            {
                return null;
            }
        }
    }
}
