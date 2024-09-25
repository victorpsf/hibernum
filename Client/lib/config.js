const os = require('os');
const process = require('process');

const getPlatform = () => os.platform();
const filterAddress = ({ address }) => /(\d{1,3}\.){3}\d{1}/g.test(address);

const getAddress = () => {
    switch (getPlatform()) {
        case 'win32':
            return  ((os.networkInterfaces()['Ethernet'].filter(a => filterAddress(a))))[0].address ?? '127.0.0.1'
        case 'linux':
            return (os.networkInterfaces()['enp4s0'].filter(a => filterAddress(a)))[0].address ?? '127.0.0.1'
        default:
            throw new Error('unsupported platform');
    }
}

const env = function () {
    const environment = process.env['ENVIRONMENT'];

    return {
        environment,
        getProxy: function(service = 'AUTH' || 'HIBERNUM') {
            return { href: process.env[`${service}_HREF`] || '' }
        }
    };
}

module.exports = {
    getPlatform,
    getAddress,
    env
}