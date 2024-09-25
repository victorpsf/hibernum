const Router = require("express");
const router = Router();
const { proxy } = require('./proxy');

router.get('*', proxy('AUTH', 'get'));
router.post('*', proxy('AUTH', 'post'));
router.put('*', proxy('AUTH', 'put'));
router.delete('*', proxy('AUTH', 'delete'));

module.exports = router;