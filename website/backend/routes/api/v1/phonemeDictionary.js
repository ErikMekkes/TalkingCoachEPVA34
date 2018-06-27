const dictionaryJson = require("./phonemeDictionary.json");

class PhonemeDictionary {

	constructor() {
		this.dictionary = PhonemeDictionary.objectToMap(dictionaryJson);
	}

	/**
	 * Get keys and values from JSON object and create a map from them
	 *
	 * @param obj JSON object with key value pairs
	 * @returns {Map<any, any>}
	 */
	static objectToMap(obj) {
		let map = new Map();
		for (let k of Object.keys(obj)) {
			map.set(k, obj[k]);
		}
		return map;
	}

}

module.exports = PhonemeDictionary;