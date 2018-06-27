import React from 'react';
import PropTypes from 'prop-types';
import AppBar from "@material-ui/core/AppBar";
import {withStyles} from '@material-ui/core/styles';
import IconButton from "@material-ui/core/IconButton";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import MenuIcon from '@material-ui/icons/Menu';

const styles = {
	root: {
		flexGrow: 1,
	},
	flex: {
		flex: 1,
	},
	menuButton: {
		marginLeft: -12,
		marginRight: 20,
	},
};

class MyAppBar extends React.Component {

	render() {
		const {classes} = this.props;
		return (
				<AppBar position="static">
					<Toolbar>
						<IconButton className={classes.menuButton} color="inherit" aria-label="Menu" onClick={() => {
							console.log("Menu click");
							console.log(this.props);
							this.props.onMenuClick()
						}}>
							<MenuIcon/>
						</IconButton>
						<Typography variant="title" color="inherit" className={classes.flex}>
							TalkingCoach
						</Typography>
					</Toolbar>
				</AppBar>
		);
	}

}

MyAppBar.propTypes = {
	classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(MyAppBar);
