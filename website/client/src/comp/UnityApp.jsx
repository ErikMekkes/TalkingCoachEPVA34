import React, {Component} from 'react';
import Unity from 'react-unity-webgl';

class UnityApp extends Component {

	render() {
		return (
				<div className="unity-wrapper">
					<Unity className="unity-container"
						   src="unity/Build/Build.json"
						   loader="unity/Build/UnityLoader.js"
					/>
				</div>
		);
	}
}

export default UnityApp;
