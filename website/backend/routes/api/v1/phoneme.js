const express = require('express');
const router = express.Router();
const child_process = require('child_process');
const {spawn} = require('child_process');
const PhonemeDictionary = require('./phonemeDictionary');

const dict = new PhonemeDictionary();

let warnings = [];

/**
 * Create point for the router to resolve the following HTTP request
 * GET /api/v1/phoneme
 */
router.get('/', function (req, res, next) {
	let params = req.query;
	warnings = [];

	/* Enable CORS */
	res.header("Access-Control-Allow-Origin", "*");
	res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");

	/* Test if text parameter is defined, return 400 error if not defined */
	if (params.text === undefined) {
		res.status(400).json({
			_request: {
				route: req.baseUrl,
				query: req.query,
				api_ver: "v1",
				error: "malformed query - text not defined"
			}
		});
		return;
	}
	/* Test if lang parameter is defined, return 400 error if not defined */
	if (params.lang === undefined) {
		res.status(400).json({
			_request: {
				route: req.baseUrl,
				query: req.query,
				api_ver: "v1",
				error: "malformed query - lang not defined"
			}
		});
		return;
	}

	// const espeak = spawn('espeak-ng', ['-qx', '-v', `${params.lang}`, '--sep=.', '"' + params.text.trim() + '"']);

	let espeak = child_process.exec(`espeak-ng -qx -v ${params.lang} --sep=. "${params.text}"`);
	let stdout = "";

	// Read stdout of eSpeak command and store it on stdout variable
	espeak.stdout.on('data', (data) => {
		stdout += data;
	});
	// Handle translation of eSpeak output once command finishes
	espeak.on('close', (code) => {
		console.log(code);
		let phonemeString = cleanPhonemeString(stdout, params.lang);
		let phonemeArray = getPhonemeArrayFromString(phonemeString);
		let phonemeArrayArpa = phonemeArrayToArpabet(phonemeArray);

		let jsonResponse = {
			_request: {route: req.baseUrl, query: req.query, api_ver: "v1"},
			phonemes: phonemeArrayArpa
		};

		if (warnings.length > 0) {
			jsonResponse.warnings = warnings;
		}

		res.status(200).json(jsonResponse);
	});
});

/**
 * This function cleans a string of unused characters. Cleanup may vary based on selected language.
 * Future developers should extend this function if they intend to use other languages
 * Throws a soft warning if language is unknown
 *
 * @param messyString The string to trim and clean of unnecessary characters
 * @param language The selected language, cleanup varies per language
 * @returns {string} Cleaned up string
 */
function cleanPhonemeString(messyString, language) {
	let result = messyString;
	console.log(messyString);
	result = result.trim();
	if (language === "nl" || language === "nl-NL") {
		result = result.replace(/[_!',|]/gi, "");
	} else if (language === "en-us" || language === "en-US" || language === "En-US") {
		result = result.replace(/[_:!',|]/gi, "");
	} else {
		result = result.replace(/[_:!',|]/gi, "");
		console.warn(`[WARN] Unknown language: ${language}. Don't know which cleanup to perform!`);
		warnings.push('language not supported by API')
	}
	result = result.trim();
	return result;
}

/**
 * Splits a string of cleaned up phonemes into an array of individual phonemes
 *
 * @param phonemeString String of cleaned up phonemes
 * @returns {*|string[]} Array of individual phonemes as strings in eSpeak notation
 */
function getPhonemeArrayFromString(phonemeString) {
	phonemeString = phonemeString.replace(/[ ]/gi, './.');
	return phonemeString.split(/[. ]/gi);
}

/**
 * Translate eSpeak phoneme notation to ARPABET phoneme notation based on mapping specified in phonemeDictionary.json file
 * @param phonemeArray Array of individual phonemes as strings in eSpeak notation
 * @returns {Array} Array of individual phonemes as strings in ARPABET notation
 */
function phonemeArrayToArpabet(phonemeArray) {
	let result = [];
	console.log(phonemeArray);
	for (let i = 0; i < phonemeArray.length; i++) {
		let phoneme = phonemeArray[i].trim();

		console.log(`eSpeak: ${phoneme}`);

		if (';' === phoneme || '_:' === phoneme || '' === phoneme || '\n' === phoneme) {
			console.log("This should be unreachable code. Firefly.");
			continue;
		}

		if (dict.dictionary.has(phoneme)) {
			dict.dictionary.get(phoneme).forEach((value) => {
				result.push(value);
				console.log(`Phoneme: ${value}`);
			});
		} else {
			console.log("!!! Invalid phoneme !!!")
		}
		console.log();
	}
	return result;
}

module.exports = router;