function startTalk() {
    var text = document.getElementById("textForSpeech").value;
    SendMessage('TalkingCoach', 'ConvertToSpeech', text);
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
    SendMessage('TalkingCoach', 'StartTypingTextInput');
}

function stopTypingTextInput() {
    SendMessage('TalkingCoach', 'StopTypingTextInput');
}

function resetPosition() {
    document.getElementById("speedSlider").value = 50;
    document.getElementById("speedMul").textContent = 100;
    document.getElementById("textForSpeech").value = "";
    SendMessage('TalkingCoach', 'ResetPosition');
}

function handleListening() {
    var currentvalue = document.getElementById("listenbutton").value;
    var newvalue = "Toggle listening";
    if (currentvalue === "Listening") {
        listening();
        newvalue = "Not listening";
    }
    if (currentvalue === "Not listening") {
        notListening();
        newvalue = "Listening";
    }
    document.getElementById("listenbutton").value = newvalue;
}

function listening() {
    SendMessage('TalkingCoach', 'Listening');
}

function notListening() {
    SendMessage('TalkingCoach', 'NotListening');
}

function handleWalking() {
    var currentvalue = document.getElementById("walkbutton").value;
    var newvalue = "Toggle walk";
    if (currentvalue === "Walk away") {
        walkAway();
        newvalue = "Walk back";
    }
    if (currentvalue === "Walk back") {
        walkBack();
        newvalue = "Walk away";
    }
    document.getElementById("walkbutton").value = newvalue;
}

function walkAway() {
    SendMessage('TalkingCoach', 'WalkAway');
}

function walkBack() {
    SendMessage('TalkingCoach', 'WalkBack');
}

window.onkeydown = function (ev) {
    var key = ev.keyCode;
    if (key == 13) { // enter key
        togglePlay();
    }
};

var togglePlay = function () {
    var button = document.getElementById('playButton').firstChild;
    var text = document.getElementById("textForSpeech").value.trim();
    if (button.data == "Play" && text != "" && checkForText(text) && isWalkedAway()) {
        startTalk();
        button.data = "Stop";
    } else {
        stopTalk();
        button.data = "Play";
    }
}

/**
 * Returns true if the avatar can speak and false if the avatar is walked away.
 * @returns {boolean}
 */
function isWalkedAway() {
    var buttonName = document.getElementById("walkbutton");
    if (buttonName.value === "Walk away") {
        return true;
    }
    return false;
}
