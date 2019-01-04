using Aspose.Services.Interfaces;
using System.Runtime.Caching;

namespace Aspose.Services
{
    public class CacheService : ICacheService
    {
        public MemoryCache GetCache()
        {
            return MemoryCache.Default;
        }
    }
}
