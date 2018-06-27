import React from 'react';
import PropTypes from 'prop-types';
import {withStyles} from '@material-ui/core/styles';
import TextField from "@material-ui/core/TextField";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/es/Button/Button";
import {UnityEvent} from "react-unity-webgl";

const styles = theme => ({
	root: {
		flexGrow: 1,
		width: '470px'
	},
	item: {
		padding: theme.spacing.unit * 2,
		textAlign: 'center',
		color: theme.palette.text.secondary,
	},
	button: {
		margin: theme.spacing.unit,
	},
	paper: {
		padding: theme.spacing.unit * 2,
		height: '100%',
		color: theme.palette.text.secondary,
	},
});

class MyAppBar extends React.Component {

	constructor(props) {
		super(props);
		this.state = {value: ''};

		this.handleChange = this.handleChange.bind(this);
		this.sendText = this.sendText.bind(this);
	}

	sendText() {
		let sendText = new UnityEvent("TalkingCoach", "convertToSpeech");
		sendText.emit(this.state.value)
		this.props.handler(false);
	}

	handleChange(event) {
		this.setState({value: event.target.value})
	}

	render() {
		const {classes} = this.props;
		return (
				<div>
					<Grid container spacing={24} justify={"center"} alignItems={"center"} direction={"row"}>
						<Grid item xs={6}>
							<TextField
									id="full-width"
									label="Text"
									InputLabelProps={{
										shrink: true,
									}}
									fullWidth
									margin="normal"
									onChange={this.handleChange}
							/>
						</Grid>
						<Grid item xs={1}>
							<Button variant="contained" color="primary" className={classes.button} onClick={this.sendText}>
								Speak
							</Button>
						</Grid>
					</Grid>
				</div>
		);
	}

}

MyAppBar.propTypes = {
	classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(MyAppBar);
