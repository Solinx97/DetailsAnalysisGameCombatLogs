import { faAngleDown, faAngleUp, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import io from 'socket.io-client';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatDeviceSettings from './VoiceChatDeviceSettings';
import VoiceChatUser from './VoiceChatUser';

import '../../../styles/communication/chats/voice.scss';

const VoiceChat = () => {
	const { t } = useTranslation("communication/chats/groupChat");

	const navigate = useNavigate();

	const me = useSelector((state) => state.customer.value);

	const [peers, setPeers] = useState([]);
	const [turnOnCamera, setTurnOnCamera] = useState(false);
	const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
	const [joinedUserUsername, setJoinedUserUsername] = useState("");
	const [showJoinedUser, setShowJoinedUser] = useState(false);
	const [showJoinedUserTimeout, setShowJoinedUserTimeout] = useState(null);
	const [myStream, setMyStream] = useState(null);
	const [roomId, setRoomId] = useState(0);
	const [chatName, setChatName] = useState("");
	const [anotherUsersAudio, setAnotherUsersAudio] = useState([]);

	const [openVideoSettings, setOpenVideoSettings] = useState(false);
	const [openAudioSettings, setOpenAudioSettings] = useState(false);
	const [microphoneDeviceId, setMicrophoneDeviceId] = useState("");

	const socketRef = useRef(null);
	const peersRef = useRef([]);
	const videoRef = useRef(null);
	const audioRef = useRef(null);

	useEffect(() => {
		socketRef.current = io.connect("192.168.0.161:2000");

		const queryParams = new URLSearchParams(window.location.search);
		const targetRoomId = +queryParams.get("roomId");
		const targetChatName = queryParams.get("chatName");

		setRoomId(targetRoomId);
		setChatName(targetChatName);
	}, []);

	useEffect(() => {
		if (me === null || roomId === 0) {
			return;
		}

		joinToRoom();

		const beforeunload = (event) => {
			refreshPage();
		}

		window.addEventListener("beforeunload", beforeunload);

		return () => {
			window.removeEventListener("beforeunload", beforeunload);

			leave();
		}
	}, [me, roomId]);

	useEffect(() => {
		if (videoRef.current === null) {
			return;
		}

		videoRef.current.srcObject = myStream;
	}, [myStream]);

	const createPeer = (userToSignal, callerId, stream) => {
		const peer = new window.SimplePeer({
			initiator: true,
			trickle: false,
			stream,
		});

		peer.on("signal", signal => {
			socketRef.current.emit("sendingSignal", { userToSignal, callerId, signal, username: me.username });
		});

		return peer;
	}

	const addPeer = (incomingSignal, callerId, stream) => {
		const peer = new window.SimplePeer({
			initiator: false,
			trickle: false,
			stream,
		});

		peer.on("signal", signal => {
			socketRef.current.emit("returningSignal", { signal, callerId });
		});

		peer.signal(incomingSignal);

		return peer;
	}

	const switchCamera = (cameraStatus) => {
		setTurnOnCamera(cameraStatus);

		if (!cameraStatus) {
			myStream.getVideoTracks()[0].stop();
			socketRef.current.emit("cameraSwitching", { roomId: roomId, cameraStatus: cameraStatus });

			return;
		}

		navigator.mediaDevices.getUserMedia({ video: cameraStatus, audio: turnOnMicrophone }).then(stream => {
			peers.forEach(peer => {
				const peerStream = peer.streams[0];
				peer.replaceTrack(peerStream.getVideoTracks()[0], stream.getVideoTracks()[0], peerStream);
			});

			setMyStream(stream);
		});

		socketRef.current.emit("cameraSwitching", { roomId: roomId, cameraStatus: cameraStatus });
	}

	const switchMicrophone = (microphoneStatus) => {
		setTurnOnMicrophone(microphoneStatus);

		if (!microphoneStatus) {
			myStream.getAudioTracks()[0].stop();
			socketRef.current.emit("microphoneSwitching", { roomId: roomId, microphoneStatus: microphoneStatus });

			return;
		}

		const constraints = {
			video: turnOnCamera,
			audio: microphoneStatus
		};

		if (microphoneDeviceId !== "") {
			constraints.audio = { deviceId: microphoneDeviceId };
		}

		navigator.mediaDevices.getUserMedia(constraints).then(stream => {
			peers.forEach(peer => {
				const peerStream = peer.streams[0];
				peer.replaceTrack(peerStream.getAudioTracks()[0], stream.getAudioTracks()[0], peerStream);
			});

			setMyStream(stream);
		});

		socketRef.current.emit("microphoneSwitching", { roomId: roomId, microphoneStatus: microphoneStatus });
	}

	const switchMicrophoneDevice = (deviceId) => {
		navigator.mediaDevices.getUserMedia({ video: turnOnCamera, audio: { deviceId: deviceId } }).then(stream => {
			peers.forEach(peer => {
				const peerStream = peer.streams[0];
				peer.replaceTrack(peerStream.getAudioTracks()[0], stream.getAudioTracks()[0], peerStream);
			});

			setMyStream(stream);
		});
	}

	const switchAudioOutputDevice = (deviceId) => {
		for (let i = 0; i < anotherUsersAudio.length; i++) {
			anotherUsersAudio[i].setSinkId(deviceId);
        }
	}

	const joinedUser = (username) => {
		if (showJoinedUserTimeout !== null) {
			clearTimeout(showJoinedUserTimeout);
		}

		setShowJoinedUser(true);
		setJoinedUserUsername(username);

		const timeout = setTimeout(() => {
			setShowJoinedUser(false);
		}, 5000);

		setShowJoinedUserTimeout(timeout);
	}

	const joinToRoom = () => {
		const stream = createDummyStream(50, 50);
		setMyStream(stream);

		socketRef.current.emit("joinToRoom", { roomId: roomId, userId: me?.id, username: me.username });

		socketRef.current.on("allUsers", users => {
			const peers = [];

			users.forEach(user => {
				const peer = createPeer(user, socketRef.current.id, stream);
				peersRef.current.push({
					peerId: user.socketId,
					username: user.username,
					peer,
				});

				peers.push(peer);
			});

			setPeers(peers);
		});

		socketRef.current.on("userJoined", payload => {
			joinedUser(payload.username);

			const peer = addPeer(payload.signal, payload.callerId, stream);
			peersRef.current.push({
				peerId: payload.callerId,
				username: payload.username,
				peer,
			})

			setPeers(users => [...users, peer]);
		});

		socketRef.current.on("receivingReturnedSignal", payload => {
			const item = peersRef.current.find(p => p.peerId === payload.id);
			item.peer.signal(payload.signal);
		});

		socketRef.current.on("someUserLeft", socketId => {
			if (peersRef.current.length === 0) {
				return;
			}

			const peerRef = peersRef.current.filter(peer => peer.peerId === socketId)[0];
			peerRef.peer.destroy();

			const peerRefIndex = peersRef.current.indexOf(peerRef);
			peersRef.current.splice(peerRefIndex, 1);
		});
	}

	const createDummyStream = (width, height) => {
		let ctx = new AudioContext(), oscillator = ctx.createOscillator();
		let dst = oscillator.connect(ctx.createMediaStreamDestination());
		oscillator.start();
		const audioTrack = Object.assign(dst.stream.getAudioTracks()[0], { enabled: true });

		let canvas = Object.assign(document.createElement("canvas"), { width: width, height: height });
		canvas.getContext('2d').fillRect(0, 0, width, height);
		let stream = canvas.captureStream();
		const videoTrack = Object.assign(stream.getVideoTracks()[0], { enabled: false });

		const mediaStream = new MediaStream([videoTrack, audioTrack]);

		return mediaStream;
	}

	const leave = () => {
		socketRef.current.emit("leavingFromRoom", { roomId: roomId, username: me?.username });

		socketRef.current.on("userLeft", () => {
			myStream?.getVideoTracks().forEach(track => {
				track.stop();
			});

			myStream?.getAudioTracks().forEach(track => {
				track.stop();
			});

			peersRef.current.forEach(peerRef => {
				peerRef.peer.destroy();;
			});

			socketRef.current.disconnect();
			socketRef.current.destroy();
			navigate(`/chats`);
		});
	}

	const refreshPage = () => {
		socketRef.current.emit("leavingFromRoom", { roomId: roomId, username: me?.username });

		socketRef.current.on("userLeft", () => {
			myStream?.getVideoTracks().forEach(track => {
				track.stop();
			});

			myStream?.getAudioTracks().forEach(track => {
				track.stop();
			});

			socketRef.current.disconnect();
		});
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
			{showJoinedUser &&
				<div className="joined-user-username">
					<div className="username">User {joinedUserUsername} joined</div>
				</div>
			}
			<div className="voice">
				<div className="voice__title">
					<div>{chatName}</div>
					<div className="tools">
						{turnOnCamera
							? <div className="device">
								<FontAwesomeIcon
									icon={faVideo}
									title={t("TurnOffCamera")}
									className="device__camera"
									onClick={() => switchCamera(!turnOnCamera)}
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
									onClick={() => switchCamera(!turnOnCamera)}
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
						{turnOnMicrophone
							? <div className="device">
								<FontAwesomeIcon
									icon={faMicrophone}
									title={t("TurnOffMicrophone")}
									className="device__microphone"
									onClick={() => switchMicrophone(!turnOnMicrophone)}
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
										switchMicrophoneDevice={switchMicrophoneDevice}
										switchAudioOutputDevice={switchAudioOutputDevice}
										microphoneIsOn={turnOnMicrophone}
									/>
								}
							</div>
							: <div className="device">
								<FontAwesomeIcon
									icon={faMicrophoneSlash}
									title={t("TurnOnMicrophone")}
									className="device__microphone"
									onClick={() => switchMicrophone(!turnOnMicrophone)}
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
										switchMicrophoneDevice={switchMicrophoneDevice}
										switchAudioOutputDevice={switchAudioOutputDevice}
										microphoneIsOn={turnOnMicrophone}
									/>
								}
							</div>
						}
						<div className="btn-shadow" title={t("Leave")} onClick={leave}>
							<FontAwesomeIcon
								icon={faRightFromBracket}
							/>
							<div>{t("Leave")}</div>
						</div>
					</div>
				</div>
				<audio ref={audioRef} />
				<div className="voice__content">
					{turnOnCamera
						? <video className="me" playsInline ref={videoRef} autoPlay />
						: <div>{me?.username}</div>
					}
					<ul className="another-user-container">
						{peersRef.current.map((peer, index) =>
							<li key={index} className="user">
								<VoiceChatUser
									peer={peer.peer}
									socket={socketRef.current}
									username={peer.username}
									audio={anotherUsersAudio}
									setAudio={setAnotherUsersAudio}
								/>
							</li>
						)}
					</ul>
				</div>
			</div>
		</>
	);
}

export default memo(VoiceChat);