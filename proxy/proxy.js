const http = require('http');
const request = require('request');

http.createServer(function (req, res) {
    if (req.url.startsWith('/api')) {
        console.log(req.url)
        const apiURL = 'https://universalgames.buk.no/' + req.url;
        res.setHeader('Access-Control-Allow-Origin', '*');
        res.setHeader('Access-Control-Allow-Methods', 'OPTIONS, GET, POST, PUT, UPDATE, DELETE');
        res.setHeader('Access-Control-Max-Age', 2592000); // 30 days
        req.pipe(request(apiURL)).pipe(res);
    } else {
        res.setHeader('Content-type', 'text/html');
        res.write(`URL must start with /api`);
        res.end();
    }
}).listen(3000);


