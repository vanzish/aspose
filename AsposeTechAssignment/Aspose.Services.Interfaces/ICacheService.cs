using System.Runtime.Caching;

namespace Aspose.Services.Interfaces
{
    public interface ICacheService
    {
        MemoryCache GetCache();
    }
}
