var systemvoices;

//Wait until system voices are ready and trigger the event OnVoiceReady
if (typeof speechSynthesis !== 'undefined') {
    speechSynthesis.onvoiceschanged = function () {
        console.log("called onvoiceschanged");
        systemvoices = window.speechSynthesis.getVoices();
        loadVoices();
    };
}

// Close the dropdown if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('#button-voice')) {

        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

function tag_clicked(button) {
    var text_field = document.getElementById("textForSpeech")
    var old_text = text_field.value;
    var selectionStart = text_field.selectionStart;
    var selectionEnd = text_field.selectionEnd;
    var tag = ' {' + button.value + '} ';
    var new_text = old_text.substring(0, selectionStart) + tag +
        old_text.substring(selectionEnd, old_text.length);

    text_field.value = new_text;

    // the cursor must be placed behind the added tag
    var new_cursor_index = selectionStart + tag.length;
    text_field.selectionStart = new_cursor_index;
    text_field.selectionEnd = new_cursor_index;

    text_field.focus();
}

/**
 * Checks wether there is text that needs to be spoken instead of only tags.
 * @param text
 * @returns {boolean}
 */
function checkForText(text) {
    var words = text.replace(/\s\s+/g, ' ').split(' ');

    for (var i = 0; i < words.length; i++) {
        if (words[i][0] != "{" || words[i][words[i].length - 1] != "}") {
            return true;
        }
    }

    return false;
}

function selectVoice() {
    document.getElementById("dropdownOptions").classList.toggle("show");
}


function voice_clicked(button) {
    document.getElementById("button-voice").value = button.value;
    textToSpeach.setVoiceIndex(button.id);
}


function loadVoices() {
    // First reset all options.
    document.getElementById('dropdownOptions').innerHTML = "";
    // Add new options.
    var max_weight = 0;
    for (var i = 0; i < systemvoices.length; i++) {
        var z = document.createElement("INPUT");
        var name = systemvoices[i].name + " | (" + systemvoices[i].lang + ")";
        z.setAttribute("type", "button");
        z.setAttribute("class", "drop-options");
        z.setAttribute("onclick", "voice_clicked(this)");
        z.setAttribute("value", name);
        z.setAttribute("id", i);
        document.getElementById('dropdownOptions').appendChild(z);
    }
    placeDropdown();
}

function placeDropdown() {
    var width = document.getElementById("dropdownOptions").cientWidth;
    document.getElementById("button-voice").width = width;
    var name = systemvoices[textToSpeach.speakVoiceIndex].name + " | (" + systemvoices[textToSpeach.speakVoiceIndex].lang + ")";
    document.getElementById("button-voice").value = name;
    
}

