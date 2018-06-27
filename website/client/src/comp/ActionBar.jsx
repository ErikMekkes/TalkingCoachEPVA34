import React from 'react';
import PropTypes from 'prop-types';
import {withStyles} from '@material-ui/core/styles';
import {UnityEvent} from "react-unity-webgl";
import PlayIcon from '@material-ui/icons/PlayArrow'
import PauseIcon from '@material-ui/icons/Pause'
import StopIcon from '@material-ui/icons/Stop'
import IconButton from "@material-ui/core/IconButton";

const styles = theme => ({
	root: {
		width: '100%',
		display: 'flex',
		'justify-content': "center"
	}
});

class ActionBar extends React.Component {

	constructor(props) {
		super(props);
		this.state = {value: ''};

		this.sendPauseSpeechEvent = this.sendPauseSpeechEvent.bind(this);
		this.sendResumeSpeechEvent = this.sendResumeSpeechEvent.bind(this);
		this.sendStopSpeechEvent = this.sendStopSpeechEvent.bind(this);
	}

	sendPauseSpeechEvent() {
		console.log("[ActionBar] Send 'pauseSpeech' to Unity");
		let sendText = new UnityEvent("TalkingCoach", "pauseSpeech");
		sendText.emit(this.state.value);
		this.props.handler(true);
	}

	sendResumeSpeechEvent() {
		console.log("[ActionBar] Send 'resumeSpeech' to Unity");
		let sendText = new UnityEvent("TalkingCoach", "resumeSpeech");
		sendText.emit(this.state.value);
		this.props.handler(false);
	}

	sendStopSpeechEvent() {
		console.log("[ActionBar] Send 'stopSpeech' to Unity");
		let sendText = new UnityEvent("TalkingCoach", "stopSpeech");
		sendText.emit(this.state.value);
	}

	render() {
		const {classes} = this.props;
		return (
				<div className={classes.root}>
					<div>
						<IconButton color="inherit" onClick={() => {
							this.sendStopSpeechEvent();
						}}>
							<StopIcon/>
						</IconButton>
						<IconButton disabled={!this.props.paused} color="inherit" onClick={() => {
							this.sendResumeSpeechEvent();
						}}>
							<PlayIcon/>
						</IconButton>
						<IconButton disabled={this.props.paused} color="inherit" onClick={() => {
							this.sendPauseSpeechEvent();
						}}>
							<PauseIcon/>
						</IconButton>
					</div>
				</div>
		);
	}

}

ActionBar.propTypes = {
	classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(ActionBar);
