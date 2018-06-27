var gameInstance = UnityLoader.instantiate("gameContainer", "Build/mybuild.json", {onProgress: UnityProgress});

function startTalk() {
    var text = document.getElementById("textForSpeech").value;
    gameInstance.SendMessage('TalkingCoach', 'ConvertToSpeech', text);
}

function stopTalk() {
    gameInstance.SendMessage('TalkingCoach', 'StopSpeech');
}

function changeBackground() {
    gameInstance.SendMessage('TalkingCoach', 'ChangeBackground');
}

function changeCoach() {
    gameInstance.SendMessage('TalkingCoach', 'ChangeCoach');
}

function zoomIn() {
    gameInstance.SendMessage('TalkingCoach', 'Zoom', -5);
}

function zoomOut() {
    gameInstance.SendMessage('TalkingCoach', 'Zoom', 5);
}

function moveAvatarLeft() {
    gameInstance.SendMessage('TalkingCoach', 'MoveAvatarHorizontal', 5);
}

function moveAvatarRight() {
    gameInstance.SendMessage('TalkingCoach', 'MoveAvatarHorizontal', -5);
}

function moveAvatarUp() {
    gameInstance.SendMessage('TalkingCoach', 'MoveAvatarVertical', 5);
}

function moveAvatarDown() {
    gameInstance.SendMessage('TalkingCoach', 'MoveAvatarVertical', -5);
}

function startTypingTextInput() {
    gameInstance.SendMessage('TalkingCoach', 'StartTypingTextInput');
}

function stopTypingTextInput() {
    gameInstance.SendMessage('TalkingCoach', 'StopTypingTextInput');
}

function resetPosition() {
    document.getElementById("speedSlider").value = 50;
    document.getElementById("speedMul").textContent = 100;
    document.getElementById("textForSpeech").value = "";
    gameInstance.SendMessage('TalkingCoach', 'ResetPosition');
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
    gameInstance.SendMessage('TalkingCoach', 'Listening');
}

function notListening() {
    gameInstance.SendMessage('TalkingCoach', 'NotListening');
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
    gameInstance.SendMessage('TalkingCoach', 'WalkAway');
}

function walkBack() {
    gameInstance.SendMessage('TalkingCoach', 'WalkBack');
}

window.onkeydown = function (ev) {
    var key = ev.keyCode;
    if (key == 13) { // enter key
        togglePlay();
    }
};

function togglePlay() {
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
