const https = require('https');

const agent = new https.Agent({
    rejectUnauthorized: false,
    requestCert: false,
    agent: false,
});

const prepareHeaders = function (headers= {}) {
    if (headers['host']) delete headers['host'];
    if (headers['referer']) delete headers['referer'];
    if (headers['sec-fetch-site']) delete headers['sec-fetch-site'];
    if (headers['sec-fetch-mode']) delete headers['sec-fetch-mode'];
    if (headers['sec-fetch-dest']) delete headers['sec-fetch-dest'];
    if (headers['sec-ch-ua-platform']) delete headers['sec-ch-ua-platform'];
    if (headers['sec-ch-ua-mobile']) delete headers['sec-ch-ua-mobile'];
    if (headers['sec-ch-ua']) delete headers['sec-ch-ua'];

    headers['user-agent'] = "insomnia/9.3.3";
    return headers;
}

module.exports = { prepareHeaders, agent, withCredentials: false }