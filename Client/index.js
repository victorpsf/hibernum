require('dotenv').config();
const express = require('express')
const http = require('http')
const download = require('./middleware/download')

const auth = require('./proxy/auth')
const hibernum = require('./proxy/hibernum');

const app = express();

app.use((req, res, next) => {
    console.log(`[${req.method}] - ${req.url}`);
    next();
})

app.use((req, res, next) => {
    res.header('Access-Control-Allow-Headers', '*')
    res.header('Access-Control-Allow-Origin', '*')
    res.header('Access-Control-Allow-Methods', 'GET, PUT, POST, DELETE, OPTIONS')
    next();
})

app.use('/', express.static('public/'))

app.use(download);
app.use('/api/v1', auth);
app.use('/api/v2', hibernum);

app.use('*', (req, res, next) => {
    res.redirect('/');
    res.end();
});

const server = http.createServer(app);

server.listen(8080, '0.0.0.0', () => console.log(`server open in http://0.0.0.0:8080`));