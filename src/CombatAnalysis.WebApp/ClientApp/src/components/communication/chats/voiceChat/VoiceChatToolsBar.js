import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import VoiceChatAudioDeviceSettings from './VoiceChatAudioDeviceSettings';

const VoiceChatToolsBar = ({ t, properties, methods, screenSharing, setScreenSharing, turnOnCamera, setTurnOnCamera, turnOnMicrophone, setTurnOnMicrophone, audioOutputDeviceId, setAudioOutputDeviceId }) => {
	const [openAudioSettings, setOpenAudioSettings] = useState(false);
	const [cameraExecute, setCameraExecute] = useState(false);
	const [showWarning, setShowWarning] = useState(false);
	const [audioInputDeviceId, setAudioInputDeviceId] = useState("");

	const navigate = useNavigate();

	useEffect(() => {
		if (!properties.hubConnection) {
			return;
		}

		const switchMicrophoneStatus = async () => {
			await methods.switchMicrophoneStatusAsync(turnOnMicrophone);
		}

		switchMicrophoneStatus();
	}, [properties.hubConnection, turnOnMicrophone]);

	useEffect(() => {
		if (!properties.hubConnection) {
			return;
		}

		const switchCameraStatus = async () => {
			setCameraExecute(true);

			await methods.switchCameraStatusAsync(turnOnCamera);

			if (turnOnCamera) {
				setScreenSharing(false);
			}

			setCameraExecute(false);
		}

		switchCameraStatus();
	}, [properties.hubConnection, turnOnCamera]);

	const leaveFromCallAsync = () => {
		methods.stopMediaData();

		navigate("/chats");
	}

	const handleOpenAudioSettings = () => {
		setOpenAudioSettings(!openAudioSettings);
	}

	const handleSwicthScreenSharing = () => {
		setScreenSharing(!screenSharing);
	}

    return (
		<div className="tools">
			<div className={`device ${turnOnCamera ? 'disabled' : ''}`}>
				{(screenSharing && showWarning) &&
					<div className="warning"></div>
				}
				<FontAwesomeIcon
					icon={screenSharing ? faDisplay : faLinkSlash}
					title={screenSharing ? t("TurnOffScreenSharing") : t("TurnOnScreenSharing")}
					className={`device__screen-sharing ${turnOnCamera ? 'busy' : ''} ${screenSharing ? 'device-use' : ''}`}
					onClick={turnOnCamera ? null : handleSwicthScreenSharing}
					onMouseEnter={turnOnCamera ? () => setShowWarning(true) : null}
					onMouseLeave={turnOnCamera ? () => setShowWarning(false) : null}
				/>
			</div>
			<div className={`device ${cameraExecute || screenSharing ? 'disabled' : ''}`}>
				{(turnOnCamera && showWarning) && 
					<div className="warning"></div>
				}
				<FontAwesomeIcon
					icon={turnOnCamera ? faVideo : faVideoSlash}
					title={turnOnCamera ? t("TurnOffCamera") : t("TurnOnCamera")}
					className={`device__camera ${cameraExecute || screenSharing ? 'busy' : ''} ${turnOnCamera ? 'device-use' : ''}`}
					onClick={cameraExecute || screenSharing ? null : () => setTurnOnCamera(!turnOnCamera)}
					onMouseEnter={screenSharing ? () => setShowWarning(true) : null}
					onMouseLeave={screenSharing ? () => setShowWarning(false) : null}
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
						audioInputDeviceId={audioInputDeviceId}
						setAudioInputDeviceId={setAudioInputDeviceId}
						audioOutputDeviceId={audioOutputDeviceId}
						setAudioOutputDeviceId={setAudioOutputDeviceId}
					/>
				}
			</div>
			<div className="btn-shadow" title={t("Leave")} onClick={leaveFromCallAsync}>
				<FontAwesomeIcon
					icon={faRightFromBracket}
				/>
				<div>{t("Leave")}</div>
			</div>
		</div>
    );
}

export default memo(VoiceChatToolsBar);