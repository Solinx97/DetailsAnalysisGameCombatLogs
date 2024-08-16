import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useRef, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import VoiceChatAudioDeviceSettings from './VoiceChatAudioDeviceSettings';

const VoiceChatToolsBar = ({ t, properties, methods, screenSharing, setScreenSharing, turnOnCamera, setTurnOnCamera, turnOnMicrophone, setTurnOnMicrophone }) => {
	const [openAudioSettings, setOpenAudioSettings] = useState(false);
	const [cameraExecute, setCameraExecute] = useState(false);

	const audioInputDeviceIdRef = useRef(null);
	const audioOutputDeviceIdRef = useRef(null);

	const navigate = useNavigate();

	useEffect(() => {
		if (!properties.connection) {
			return;
		}

		const switchMicrophoneStatus = async () => {
			await methods.switchMicrophoneStatusAsync(turnOnMicrophone);
		}

		switchMicrophoneStatus();
	}, [properties.connection, properties.stream, turnOnMicrophone]);

	useEffect(() => {
		if (!properties.connection) {
			return;
		}

		const switchCameraStatus = async () => {
			await methods.switchCameraStatusAsync(turnOnCamera, setCameraExecute);
		}

		switchCameraStatus();
	}, [properties.connection, properties.stream, turnOnCamera]);

	const leaveFromCall = () => {
		methods.stopMediaData();

		navigate("/chats");
	}

	const handleOpenAudioSettings = () => {
		setOpenAudioSettings(!openAudioSettings);
	}

    return (
		<div className="tools">
			<div className="device">
				<FontAwesomeIcon
					icon={screenSharing ? faDisplay : faLinkSlash}
					title={screenSharing ? t("TurnOffScreenSharing") : t("TurnOnScreenSharing")}
					className={`device__screen ${screenSharing ? 'device-use' : ''}`}
					onClick={() => setScreenSharing(!screenSharing)}
				/>
			</div>
			<div className="device" style={{ opacity: cameraExecute ? 0.4 : 1 }}>
				<FontAwesomeIcon
					icon={turnOnCamera ? faVideo : faVideoSlash}
					title={turnOnCamera ? t("TurnOffCamera") : t("TurnOnCamera")}
					className={`device__camera ${turnOnCamera ? 'device-use' : ''}`}
					onClick={cameraExecute ? null : () => setTurnOnCamera(!turnOnCamera)}
				/>
			</div>
			<div className="device">
				<FontAwesomeIcon
					icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
					title={turnOnMicrophone ? t("TurnOffMicrophone") : t("TurnOnMicrophone")}
					className={`device__microphone ${turnOnMicrophone ? 'device-use' : ''}`}
					onClick={() => setTurnOnMicrophone(!turnOnMicrophone)}
				/>
				<FontAwesomeIcon
					icon={openAudioSettings ? faAngleUp : faAngleDown}
					title={t("Setting")}
					className="device__settings"
					onClick={handleOpenAudioSettings}
				/>
				{openAudioSettings &&
					<VoiceChatAudioDeviceSettings
						t={t}
						peerConnectionsRef={properties.peerConnectionsRef}
						turnOnMicrophone={turnOnMicrophone}
					stream={properties.stream}
						audioInputDeviceIdRef={audioInputDeviceIdRef}
						audioOutputDeviceIdRef={audioOutputDeviceIdRef}
					/>
				}
			</div>
			<div className="btn-shadow" title={t("Leave")} onClick={leaveFromCall}>
				<FontAwesomeIcon
					icon={faRightFromBracket}
				/>
				<div>{t("Leave")}</div>
			</div>
		</div>
    );
}

export default VoiceChatToolsBar;