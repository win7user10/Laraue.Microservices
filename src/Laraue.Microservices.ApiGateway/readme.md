Service responsibility
1. Define routes between this service (HTTP) and others (HTTP/GRPC).
   1. Some routes are just a proxy
   2. Some routes can making API concatenation
2. Make authentication (JWT/cookie etc.) and pass JWT token to the proxy calls
3. Circuit breaker, limiter implementation (redis-based?)
4. Start tracing for requests