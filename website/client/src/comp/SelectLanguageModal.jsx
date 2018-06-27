import React from "react";
import {UnityEvent} from "react-unity-webgl";
import PropTypes from "prop-types";
import {withStyles} from "@material-ui/core/styles";
import Typography from "@material-ui/core/Typography";
import Modal from "@material-ui/core/Modal";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import Menu from "@material-ui/core/Menu";
import MenuItem from "@material-ui/core/MenuItem";

function getModalStyle() {
	const top = 50;
	const left = 50;

	return {
		top: `${top}%`,
		left: `${left}%`,
		transform: `translate(-${top}%, -${left}%)`
	};
}

const styles = theme => ({
	root: {
		width: '100%',
		maxWidth: 360,
		backgroundColor: theme.palette.background.paper,
	},
	paper: {
		position: "absolute",
		width: theme.spacing.unit * 50,
		backgroundColor: theme.palette.background.paper,
		boxShadow: theme.shadows[5],
		padding: theme.spacing.unit * 4
	}
});

const languageOptions = [
	'US English',
	'Dutch',
];

class SelectLanguageModal extends React.Component {
	state = {
		open: false,
		language: 'en-US',
		selectedIndex: 0
	};

	handleOpen = () => {
		this.props.handler(true);
	};

	handleClose = () => {
		this.props.handler(false);
	};

	handleClickListItem = event => {
		this.setState({ anchorEl: event.currentTarget });
	};

	handleMenuItemClick = (event, index) => {
		this.setState({ selectedIndex: index, anchorEl: null });
		if(index === 0)  {
			this.setState({language: 'en-us'}, this.changeLanguage)
		} else if (index === 1) {
			this.setState({language: 'nl'}, this.changeLanguage)
		}
	};

	changeLanguage() {
		console.log(`Changing language to ${this.state.language}`);
		let languageChangeEvent = new UnityEvent('TalkingCoach', 'setLanguage');
		languageChangeEvent.emit(this.state.language)
	}

	handleMenuClose = () => {
		this.setState({ anchorEl: null });

		if(this.state.selectedIndex === 0) {
			this.setState({language: 'en-US'})
		} else if (this.state.selectedIndex === 1) {
			this.setState({language: 'nl'})
		}
		console.log(`MenuClose. New language is ${this.state.selectedIndex}`);
	};

	render() {
		const {classes} = this.props;
		const { anchorEl } = this.state;

		const languageSelectMenu = (
				<div className={classes.root}>
					<List component="nav">
						<ListItem
								button
								aria-haspopup="true"
								aria-controls="lock-menu"
								aria-label="When device is locked"
								onClick={this.handleClickListItem}
						>
							<ListItemText
									primary={languageOptions[this.state.selectedIndex]}
							/>
						</ListItem>
					</List>
					<Menu
							id="lock-menu"
							anchorEl={anchorEl}
							open={Boolean(anchorEl)}
							onClose={this.handleMenuClose}
					>
						{languageOptions.map((option, index) => (
								<MenuItem
										key={option}
										selected={index === this.state.selectedIndex}
										onClick={event => this.handleMenuItemClick(event, index)}
								>
									{option}
								</MenuItem>
						))}
					</Menu>
				</div>
		);

		return (
				<div>
					<Modal
							aria-labelledby="simple-modal-title"
							aria-describedby="simple-modal-description"
							open={this.props.open}
							onClose={this.handleClose}
					>
						<div style={getModalStyle()} className={classes.paper}>
							<Typography variant="title" id="modal-title">
								Select a language
							</Typography>
							<div>{languageSelectMenu}</div>
						</div>
					</Modal>
				</div>
		);
	}


}

SelectLanguageModal.propTypes = {
	classes: PropTypes.object.isRequired
};

export default withStyles(styles)(SelectLanguageModal);
