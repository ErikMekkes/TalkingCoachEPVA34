const express = require('express');
const router = express.Router();

const v1Router = require('./api/v1');

router.get('/', function(req, res, next) {
	res.status(404).json({ error: 404, message: "Must define API version like '/api/v{version}/" });
});

router.use('/v1', v1Router);

module.exports = router;