const express = require('express');
const router = express.Router();

const phonemeRouter = require('./v1/phoneme');

router.get('/', function(req, res, next) {
	res.status(404).json({ _request: {route: req.baseUrl, api_ver: "v1", error: "No endpoint called"} });
});

router.use('/phoneme', phonemeRouter);

module.exports = router;