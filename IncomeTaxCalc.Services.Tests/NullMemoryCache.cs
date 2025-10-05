using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.Tests
{
    public sealed class NullMemoryCache : IMemoryCache
    {
        //This stubs for IMemoryCache when doing unit tests. Using Moq Mocks for it causes tests to fail because Set() is an extension method
        //And can't be stubbed from a mock. From this stack overflow answer: https://stackoverflow.com/questions/38318247/mock-imemorycache-in-unit-test
        public ICacheEntry CreateEntry(object key)
        {
            return new NullCacheEntry() { Key = key };
        }

        public void Dispose()
        {
        }

        public void Remove(object key)
        {

        }

        public bool TryGetValue(object key, out object value)
        {
            value = null;
            return false;
        }

        private sealed class NullCacheEntry : ICacheEntry
        {
            public DateTimeOffset? AbsoluteExpiration { get; set; }
            public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

            public IList<IChangeToken> ExpirationTokens { get; set; }

            public object Key { get; set; }

            public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; set; }

            public CacheItemPriority Priority { get; set; }
            public long? Size { get; set; }
            public TimeSpan? SlidingExpiration { get; set; }
            public object Value { get; set; }

            public void Dispose()
            {

            }
        }
    }
}
