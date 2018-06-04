function startTalk() {
	var text = document.getElementById("textForSpeech").value;
	SendMessage('TalkingCoach', 'ConvertToSpeech', text);
}

function toggleLanguage(checkbox) {
    var checkboxes = document.getElementsByClassName("checkbox");
    for (i = 0; i < checkboxes.length; i++){
        checkboxes[i].checked = false;
    }
    checkbox.checked = true;
    SendMessage('TalkingCoach', 'setLanguage', checkbox.value);
}

function stopTalk() {
	SendMessage('TalkingCoach', 'StopSpeech');
}

function changeBackground() {
	SendMessage('TalkingCoach', 'ChangeBackground');
}

function changeCoach() {
	SendMessage('TalkingCoach', 'ChangeCoach');
}

function zoomIn() {
	SendMessage('TalkingCoach', 'Zoom', -5);
}

function zoomOut() {
	SendMessage('TalkingCoach', 'Zoom', 5);
}

function moveAvatarLeft() {
	SendMessage('TalkingCoach', 'MoveAvatarHorizontal', 5);
}

function moveAvatarRight() {
	SendMessage('TalkingCoach', 'MoveAvatarHorizontal', -5);
}

function moveAvatarUp() {
	SendMessage('TalkingCoach', 'MoveAvatarVertical', 5);
}

function moveAvatarDown() {
	SendMessage('TalkingCoach', 'MoveAvatarVertical', -5);
}

function startTypingTextInput() {
	console.log("starting keyboard input");
	SendMessage('TalkingCoach', 'StartTypingTextInput');
}

function stopTypingTextInput() {
	console.log("stopping keyboard input");
	SendMessage('TalkingCoach', 'StopTypingTextInput');
}

function resetPosition() {
    SendMessage('TalkingCoach', 'ResetPosition');
}

window.onkeydown = function (ev) {
    var key = ev.keyCode;
    if (key == 13) { // enter key
        togglePlay();
    }
};

function tag_clicked(button) {
    var text_field = document.getElementById("textForSpeech")
    var old_text = text_field.value;
    var cursor_index = text_field.selectionStart;
    var tag = ' {' + button.value + '} ';
    var new_text = old_text.substring(0, cursor_index) + tag
    			+ old_text.substring(cursor_index, old_text.length);

    text_field.value = new_text;
    
    // the cursor must be placed behind the added tag
    var new_cursor_index = cursor_index + tag.length;
    text_field.selectionStart = new_cursor_index;
    text_field.selectionEnd = new_cursor_index;

    text_field.focus();
}

var togglePlay = function() {
  var button = document.getElementById('playButton').firstChild;
  var text = document.getElementById("textForSpeech").value.trim();
   if (button.data == "Play" && text != "" && checkForText(text)) 
   {
       startTalk();
       button.data = "Stop";
   }
   else 
   {
       stopTalk();
       button.data = "Play";
   }
}

/**
 * Checks wether there is text that needs to be spoken instead of only tags.
 * @param text
 * @returns {boolean}
 */
function checkForText(text) {

    var words = text.replace(/\s\s+/g, ' ').split(' ');

    for (var i = 0; i < words.length; i++) {
        if (words[i][0] != "{" || words[i][words[i].length-1] != "}"){
            return true;
        } 
    }
    
    return false;
}
