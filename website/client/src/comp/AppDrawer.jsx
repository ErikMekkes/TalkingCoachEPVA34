import React from 'react';
import PropTypes from 'prop-types';
import {withStyles} from '@material-ui/core/styles';
import SwipeableDrawer from '@material-ui/core/SwipeableDrawer';
import ListItem from "@material-ui/core/es/ListItem/ListItem";
import ListItemIcon from "@material-ui/core/es/ListItemIcon/ListItemIcon";
import ListItemText from "@material-ui/core/es/ListItemText/ListItemText";
import LanguageIcon from '@material-ui/icons/Language'
import SelectLanguageModal from "./SelectLanguageModal";

const styles = {
	list: {
		width: 250,
	},
	fullList: {
		width: 'auto',
	},
};

class AppDrawer extends React.Component {

	constructor(props) {
		super(props);
		this.state = {languageSelectOpen: false};
		this.languageSelectModalHandler = this.languageSelectModalHandler.bind(this)
	}

	toggleDrawer = (open) => () => {
		console.log(this.props);
		this.props.handler(open);
	};

	languageSelectModalHandler(open) {
		this.setState({languageSelectOpen: open})
	};

	render() {
		const {classes} = this.props;

		const sideList = (
				<div className={classes.list}>
					<ListItem button
							  onClick={() => {this.languageSelectModalHandler(true)}}
					>
						<ListItemIcon>
							<LanguageIcon/>
						</ListItemIcon>
						<ListItemText primary="Select language"/>
					</ListItem>
				</div>
		);

		return (
				<div>
					<SwipeableDrawer
							open={this.props.open}
							onClose={this.toggleDrawer(false)}
							onOpen={this.toggleDrawer(true)}
					>
						<div
								tabIndex={0}
								role="button"
								onClick={this.toggleDrawer(false)}
								onKeyDown={this.toggleDrawer(false)}
						>
							{sideList}
						</div>
					</SwipeableDrawer>
					<SelectLanguageModal open={this.state.languageSelectOpen} handler={this.languageSelectModalHandler}/>
				</div>
		);
	}
}

AppDrawer.propTypes = {
	classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(AppDrawer);