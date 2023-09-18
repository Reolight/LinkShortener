const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');
const path = require("path");

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:10491';

const onError = (err, req, resp, target) => {
    console.error(`${err.message}`);
}

const context = [
    { path: '/links' },
    { regx: '^/[a-z0-9=-]+$'}
]

const filter = (pathname, req) => {
    let shouldBeProxied = false;
    for (const pattern of context)
    {
        const pathMatches = (!!pattern.path && pathname.match(`^${pattern.path}`));
        const regxMatches = (!!pattern.regx && pathname.match(pattern.regx));
        const methodMatches = !pattern.method || (pattern.method && pattern.method === req.method);
        
        shouldBeProxied = (pathMatches || regxMatches) && methodMatches;
        
        if (env.DEBUG)
            console.debug(`For path [${pathname}] and case {\n\tpath: ${pattern.path}\n\tregx: ${pattern.regx}\n\tmethod: ${pattern.method}\n} made next checks: `
                + `path matches: ${pathMatches}, regx matches: ${regxMatches}, method matches: ${methodMatches}. Decision to proxy: ${shouldBeProxied}\n`)
        
        if (shouldBeProxied) return shouldBeProxied;
    }
    
    return false;
}

module.exports = function (app) {
  const appProxy = createProxyMiddleware(filter, {
    target: target,
    // Handle errors to prevent the proxy middleware from crashing when
    // the ASP NET Core webserver is unavailable
    onError: onError,
    secure: false,
    // Uncomment this line to add support for proxying websockets
    //ws: true, 
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
