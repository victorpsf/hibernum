const axios = require('axios');
const { env } = require('../lib/config')
const { prepareHeaders, agent, withCredentials } = require('./util');

const proxy = function (name, method) {
    const config = env().getProxy(name);

    const readParams = function (req = request) {
        const [url, params] = req.url.split('?')
        const paramSpit = (params || '').split('&').filter(a => !!a).map(a => a.split('='));
    
        const _params_ = {}
        for (const [key, ...values] of paramSpit) {
            _params_[key] = values.map(a => {
                if (!!a) return a;
                return '=';
            }).join('');
        }
    
        return { url, params: _params_};
    }

    return async function (req, res) {
        const { url, params } = readParams(req);
        const toHref = `${config.href}${url}`;

        try {
            const caller = (['POST', 'PUT'].includes(method.toUpperCase())) ?
                    axios[method](
                        toHref, 
                        req.body, 
                        { 
                            params: params, 
                            headers: prepareHeaders(req.headers),
                            withCredentials,
                            httpsAgent: agent
                        }
                    ):
                    axios[method](
                        toHref, 
                        { 
                            params: params, 
                            headers: prepareHeaders(req.headers),
                            withCredentials,
                            httpsAgent: agent
                        }
                    );
            const { data } = await caller;
            console.log(`[${req.method}] - ${toHref} [SUCCESS]\n  - params : ${JSON.stringify(params ?? {})}\n  - body   : ${JSON.stringify(req.body ?? null)}\n  - headers: ${JSON.stringify(req.headers ?? {})} `)
            res.json(data);
            res.end();
        }
    
        catch (ex) {
            console.log(ex);

            if (ex instanceof axios.AxiosError) {
                console.log(`[${req.method}] - ${toHref} [AXIOS ERROR (${ex})]\n  - params : ${JSON.stringify(params ?? {})}\n  - body   : ${JSON.stringify(req.body ?? null)}\n  - headers: ${JSON.stringify(req.headers ?? {})} `)

                res.status(ex.response?.status || (parseInt(ex.code ?? "") || 500))
                res.json(ex.response?.data);
                res.end();
                return;
            }

            console.log(`[${req.method}] - ${toHref} [ERROR (${ex})]\n  - params : ${JSON.stringify(params ?? {})}\n  - body   : ${JSON.stringify(req.body ?? null)}\n  - headers: ${JSON.stringify(req.headers ?? {})} `)
            res.status(500)
            res.end();
        }
    }
}

module.exports = {
    proxy
}