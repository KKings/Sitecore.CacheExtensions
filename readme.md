## Sitecore.CacheExtensions

The Sitecore.CacheExtensions library is a series of extensions to provide configuration based caches, based on the helix design principles.

### Installation
Using nuget: Install-Package Sitecore.CacheExtensions

### Injected Services

The following classes are automatically injected into the Sitecore OTB Service Collection:

* SitecoreCacheManager (ICacheManager) - Adds layer to expose configured caches
* TransientCache (BaseTransientCache) - Adds caching per request within the HttpContext.Items

To grab a reference to either the SitecoreCacheManager or a TransientCache, add a class that extends the IServicesConfigurator and add your classes that you would like to be resolved.

```c#
public class RegisterDependencies : IServicesConfigurator 
{
        public void Configure(IServiceCollection serviceCollection)
        {
            // Add your classes here using the serviceCollection methods
        }
}
```
```xml
<?xml version="1.0"?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <services>
      <configurator type="[Class], [Assembly Name]" />
    </services>
  </sitecore>
</configuration>

```



For more information, see: http://kamsar.net/index.php/2016/08/Dependency-Injection-in-Sitecore-8-2/

### Adding a new cache

Add a new cache by adding a path configuration like so:

```xml 
<?xml version="1.0"?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
    <sitecore>
        <caches>
            <cache name="example-cache" maxSize="100MB" lifespan="60" expirationType="sliding" />
        </caches>
    </sitecore>
</configuration>
```

### Configuring the Cache

All fields are required

#### name

Must be unique and is used to identify the cache with the Cache Manager

#### maxSize

Max size of the cache in string representation

Examples:

  - 100K
  - 10MB
  - 1GB

### lifespan

Lifetime of a cache entry, read in the number of seconds.

Examples:

  - 60 = 1 minute
  - 360 = 6 minutes
  - 3600 = 1 hour

### expirationType

Determines how the cache entry should expire

Supported Expiration Types:

  - Sliding - expires entry if it hasn't been accessed within the lifespan
  - Absolute - expires entry after a set amount of time
  - Sticky (not implemented)
