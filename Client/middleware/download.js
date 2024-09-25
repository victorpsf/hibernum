const { request, response } = require('express')

const toArray = function (buff = Buffer.from([])) {
    const bytes = [];
    for (const byte of buff) bytes.push(byte);
    return bytes;
}

const readData = (req = request) => new Promise((resolve, reject) => {
    let values = [];

    req.on('data', chunk => {
        if (chunk) values = values.concat(toArray(chunk));
    })

    req.on('error', () => {
        reject();
    })

    req.on('end', () => {
        req.buff = Buffer.from(values);
        resolve();
    })
});

module.exports = async function (req = request, res = response, next) {
    try {
        await readData(req);

        switch (req.headers['content-type']) {
            case 'application/json':
                req.body = JSON.parse(req.buff.toString('utf-8'))
                break;
            case 'application/octet-stream':
                req.body = req.buff;
                break;
            default:
                break;
        }

        delete req.buff;
        next();
    }

    catch (ex) {
        res.status(400);
        res.json = { message: 'Unprocessable data input' }
        res.end();
    }
}