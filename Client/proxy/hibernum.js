const Router = require("express");
const router = Router();
const { proxy } = require('./proxy');

router.get('*', proxy('HIBERNUM', 'get'));
router.post('*', proxy('HIBERNUM', 'post'));
router.put('*', proxy('HIBERNUM', 'put'));
router.delete('*', proxy('HIBERNUM', 'delete'));

module.exports = router;