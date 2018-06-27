// Unity functions are in c#, web functions are javascript, need method to 
// call c# from javascript : Runtime.dynCall(signature, pointer, args)
// signature = return type and arguments of function to be called.
// pointer = reference of function to be called
// args = array of arguments for function

// example: want a javascript function that calls callbackBoundary, which is a c#
// function that returns void and takes an integer as argument. The javascript function
// should take `event` as input, and call callbackBoundary with event.charIndex as input.
// signature : return type void (v), integer as input (i) = "vi"
// pointer : callbackBoundary
// args : [event.charIndex]
// result : function(event){Runtime.dynCall("vi", callbackBoundary, [event.charIndex])}

var MyPlugin = {
    // Speak javascript function that calls the textToSpeach.speak function 
    // with as arguments (text, voicename, parameters)
    Speak: function(textMessage, voiceType, callbackStart, callbackEnd, callbackBoundary, callbackPause, callbackResume, lang) {
        var s = Pointer_stringify(lang);
        textToSpeach.speak(Pointer_stringify(textMessage), Pointer_stringify(voiceType), {
            onstart: function(event){Runtime.dynCall('vif', callbackStart, [event.charIndex, event.elapsedTime])},
            onend: function(event){Runtime.dynCall('vif', callbackEnd, [event.charIndex, event.elapsedTime])},
            onboundary: function(event){Runtime.dynCall("vif", callbackBoundary, [event.charIndex, event.elapsedTime])},
            onpause: function(event){Runtime.dynCall("vif", callbackPause, [event.charIndex, event.elapsedTime])},
            onresume: function(event){Runtime.dynCall("vif", callbackResume, [event.charIndex, event.elapsedTime])},
            language: s
        });
    },
    
    Stop: function() {
        window.speechSynthesis.cancel();
    },
    
    getSystemVoices: function() {
      console.log(textToSpeach.getSystemVoices())
      //return textToSpeach.getSystemVoices();
    },
    
    // Returns the hostname of the current page as string.
    // Unity requires a UTF8 data type string, so a conversion is required from javascript.
    getHostNameString: function() {
        var protocol = location.protocol;
        var slashes = protocol.concat("//");
        var host = slashes.concat(window.location.hostname);
        var bufferSize = lengthBytesUTF8(host) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(host, buffer, bufferSize);
        return buffer;
    },
		
    LoadUnity: function() {
        textToSpeach.loadUnity();
    },
};
mergeInto(LibraryManager.library, MyPlugin);