### Adding a new cache

Add a new cache by adding a path configuration like so:

```sh $ 
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