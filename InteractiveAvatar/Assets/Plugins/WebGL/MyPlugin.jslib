var MyPlugin = {

    Speak: function(textMessage, voiceType, callbackStart, callbackEnd, lang)
    {
        var s = Pointer_stringify(lang);
        textToSpeach.speak(Pointer_stringify(textMessage), Pointer_stringify(voiceType), {onstart: function(){Runtime.dynCall('v', callbackStart, 0)}, onend: function(){Runtime.dynCall('v', callbackEnd, 0)}, language: s});
    },
	Stop: function()
    {
        textToSpeach.cancel();
    },
    getSystemVoices: function()
    {
    	console.log(textToSpeach.getSystemVoices())
    	//return textToSpeach.getSystemVoices();
    },
};
mergeInto(LibraryManager.library, MyPlugin);