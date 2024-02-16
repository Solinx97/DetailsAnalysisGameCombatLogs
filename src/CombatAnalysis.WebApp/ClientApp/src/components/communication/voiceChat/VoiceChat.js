import { faAngleDown, faAngleUp, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import useVoice from '../../../hooks/useVoice';
import { updateCall } from '../../../store/slicers/CallSlice';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatDeviceSettings from './VoiceChatDeviceSettings';
import VoiceChatUser from './VoiceChatUser';

import '../../../styles/communication/chats/voice.scss';

const VoiceChat = ({ callMinimazedData }) => {
	const { t } = useTranslation("communication/chats/groupChat");

	const navigate = useNavigate();

	const storeCallData = useSelector((state) => state.call.value);
	const me = useSelector((state) => state.customer.value);

	const dispatch = useDispatch();

	const [openVideoSettings, setOpenVideoSettings] = useState(false);
	const [openAudioSettings, setOpenAudioSettings] = useState(false);
	const [microphoneDeviceId, setMicrophoneDeviceId] = useState("");

	const audioRef = useRef(null);

	const voice = useVoice(me, callMinimazedData, microphoneDeviceId);

	useEffect(() => {
		if (!isCallStarted()) {
			navigate(`/chats`);
		}

		voice.func.initConnection();
	}, []);

	useEffect(() => {
		if (me === null || voice.data.roomId === 0) {
			return;
		}

		if (storeCallData.socketId === "") {
			voice.func.joinToRoom();
		}

		const beforeunload = (event) => {
			removeCookie();
		}

		window.addEventListener("beforeunload", beforeunload);

		return () => {
			window.removeEventListener("beforeunload", beforeunload);

			dispatch(updateCall(voice.data.callData));
		}
	}, [me, voice.data.roomId]);

	const isCallStarted = () => {
		const allCokie = document.cookie.split(";");
		const calStartedCookie = allCokie.filter(cookie => cookie.includes("callAlreadyStarted"));
		return calStartedCookie.length > 0;
	}

	const removeCookie = () => {
		document.cookie = "callAlreadyStarted=true;expires=Thu, 01 Jan 1970 00:00:00 UTC";
	}

	const handleOpenVideoSettings = () => {
		setOpenAudioSettings(false);
		setOpenVideoSettings(!openVideoSettings);
	}

	const handleOpenAudioSettings = () => {
		setOpenVideoSettings(false);
		setOpenAudioSettings(!openAudioSettings);
	}

	return (
		<>
			<CommunicationMenu
				currentMenuItem={1}
			/>
			<div className="voice">
				<div className="voice__title">
					<div>{voice.data.chatName}</div>
					<div className="tools">
						{voice.data.turnOnCamera
							? <div className="device">
								<FontAwesomeIcon
									icon={faVideo}
									title={t("TurnOffCamera")}
									className="device__camera"
									onClick={() => voice.func.switchCamera(!voice.data.turnOnCamera)}
								/>
								{openVideoSettings
									? <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenVideoSettings}
									/>
									: <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenVideoSettings}
									/>
								}
								{openVideoSettings &&
									<VoiceChatDeviceSettings />
								}
							</div>
							: <div className="device">
								<FontAwesomeIcon
									icon={faVideoSlash}
									title={t("TurnOnCamera")}
									className="device__camera"
									onClick={() => voice.func.switchCamera(!voice.data.turnOnCamera)}
								/>
								{openVideoSettings
									? <FontAwesomeIcon
										icon={faAngleUp}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenVideoSettings}
									/>
									: <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenVideoSettings}
									/>
								}
								{openVideoSettings &&
									<VoiceChatDeviceSettings />
								}
							</div>
						}
						{voice.data.turnOnMicrophone
							? <div className="device">
								<FontAwesomeIcon
									icon={faMicrophone}
									title={t("TurnOffMicrophone")}
									className="device__microphone"
									onClick={() => voice.func.switchMicrophone(!voice.data.turnOnMicrophone)}
								/>
								{openAudioSettings
									? <FontAwesomeIcon
										icon={faAngleUp}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
									: <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
								}
								{openAudioSettings &&
									<VoiceChatDeviceSettings
										isAudio={true}
										setMicrophoneDeviceId={setMicrophoneDeviceId}
										switchMicrophoneDevice={voice.func.switchMicrophoneDevice}
										switchAudioOutputDevice={voice.func.switchAudioOutputDevice}
										microphoneIsOn={voice.data.turnOnMicrophone}
									/>
								}
							</div>
							: <div className="device">
								<FontAwesomeIcon
									icon={faMicrophoneSlash}
									title={t("TurnOnMicrophone")}
									className="device__microphone"
									onClick={() => voice.func.switchMicrophone(!voice.data.turnOnMicrophone)}
								/>
								{openAudioSettings
									? <FontAwesomeIcon
										icon={faAngleUp}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
									: <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
								}
								{openAudioSettings &&
									<VoiceChatDeviceSettings
										isAudio={true}
										setMicrophoneDeviceId={setMicrophoneDeviceId}
										switchMicrophoneDevice={voice.func.switchMicrophoneDevice}
										switchAudioOutputDevice={voice.func.switchAudioOutputDevice}
										microphoneIsOn={voice.data.turnOnMicrophone}
									/>
								}
							</div>
						}
						<div className="btn-shadow" title={t("Leave")} onClick={() => voice.func.leave(true)}>
							<FontAwesomeIcon
								icon={faRightFromBracket}
							/>
							<div>{t("Leave")}</div>
						</div>
					</div>
				</div>
				<audio ref={audioRef} />
				<div className="voice__content">
					{voice.data.turnOnCamera
						? <video className="me" playsInline ref={voice.data.videoRef} autoPlay />
						: <div>{me?.username}</div>
					}
					<ul className="another-user-container">
						{voice.data.peersRef.current?.map((peer, index) =>
							<li key={index} className="user">
								<VoiceChatUser
									peer={peer.peer}
									socket={voice.data.socketRef.current}
									username={peer?.username}
									audio={voice.data.anotherUsersAudio}
									setAudio={voice.data.setAnotherUsersAudio}
									initTurnOnCamera={peer?.turnOnCamera}
									initTurnOnMicrophone={peer?.turnOnMicrophone}
								/>
							</li>
						)}
					</ul>
				</div>
			</div>
		</>
	);
}

export default memo(WithVoiceContext(VoiceChat));