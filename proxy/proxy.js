var https = require('https');
const request = require('request');

const fs = require('fs');

const options = {
    key: fs.readFileSync('key.pem'),
    cert: fs.readFileSync('cert.pem')
};

const app = function (req, res) {
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
}


https.createServer(options, app).listen(3000);