import { faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import Peer from "simple-peer";
import io from "socket.io-client";
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatUser from "./VoiceChatUser";

import "../../../styles/communication/chats/voice.scss";

const VoiceChat = () => {
	const { t } = useTranslation("communication/chats/groupChat");

	const navigate = useNavigate();

	const me = useSelector((state) => state.customer.value);

	const [peers, setPeers] = useState([]);
	const [turnOnCamera, setTurnOnCamera] = useState(true);
	const [turnOnMicrophone, setTurnOnMicrophone] = useState(true);
	const [joinedUserUsername, setJoinedUserUsername] = useState("");
	const [showJoinedUser, setShowJoinedUser] = useState(false);
	const [showJoinedUserTimeout, setShowJoinedUserTimeout] = useState(null);
	const [myStream, setMyStream] = useState(null);
	const [roomId, setRoomId] = useState(0);

	const socketRef = useRef(null);
	const peersRef = useRef([]);
	const userVideoRef = useRef(null);

	useEffect(() => {
		socketRef.current = io.connect("192.168.0.161:2000");

		const queryParams = new URLSearchParams(window.location.search);
		const targetRoomId = +queryParams.get("roomId");

		setRoomId(targetRoomId);
	}, [])

	useEffect(() => {
		if (me === null || roomId === 0) {
			return;
		}

		joinToRoomAsync();
	}, [me, roomId])

	useEffect(() => {
		if (userVideoRef.current === null) {
			return;
		}

		userVideoRef.current.srcObject = myStream;
	}, [myStream])

	const createPeer = (userToSignal, callerId, stream) => {
		const peer = new Peer({
			initiator: true,
			trickle: false,
			stream,
		});

		peer.on("signal", (signal) => {
			socketRef.current.emit("sendingSignal", { userToSignal, callerId, signal })
		});

		return peer;
	}

	const addPeer = (incomingSignal, callerId, stream) => {
		const peer = new Peer({
			initiator: false,
			trickle: false,
			stream,
		});

		peer.on("signal", signal => {
			socketRef.current.emit("returningSignal", { signal, callerId })
		});

		peer.signal(incomingSignal);

		return peer;
	}

	const switchCameraAsync = async (cameraStatus) => {
		setTurnOnCamera(cameraStatus);

		if (cameraStatus === false || (cameraStatus === false && turnOnMicrophone === false)) {
			myStream.getVideoTracks().forEach(track => {
				track.stop();
			});

			return;
		}

		const newStream = await navigator.mediaDevices.getUserMedia({ video: cameraStatus, audio: turnOnMicrophone });
		setMyStream(newStream);
	}

	const switchMicrophoneAsync = async (microphoneStatus) => {
		setTurnOnMicrophone(microphoneStatus);

		if (microphoneStatus === false || (turnOnCamera === false && microphoneStatus === false)) {
			myStream.getAudioTracks().forEach(track => {
				track.stop();
			});

			return;
		}

		const newStream = await navigator.mediaDevices.getUserMedia({ video: turnOnCamera, audio: microphoneStatus });
		setMyStream(newStream);
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

	const joinToRoomAsync = async () => {
		const stream = await navigator.mediaDevices.getUserMedia({ video: turnOnCamera, audio: turnOnMicrophone });

		setTurnOnCamera(stream.getVideoTracks().length > 0);
		setMyStream(stream);

		socketRef.current.emit("joinToRoom", { roomId: roomId, userId: me?.id, username: me?.username });

		socketRef.current.on("allUsers", users => {
			const peers = [];
			users.forEach(user => {
				const peer = createPeer(user, socketRef.current.id, stream);
				peersRef.current.push({
					peerId: user.socketId,
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
				peer,
			})

			setPeers(users => [...users, peer]);
		});

		socketRef.current.on("receivingReturnedSignal", payload => {
			const item = peersRef.current.find(p => p.peerId === payload.id);
			item.peer.signal(payload.signal);
		});
	}

	const leave = () => {
		socketRef.current.emit("leaveFromRoom", { roomId: roomId, username: me?.username });

		socketRef.current.on("userLeft", () => {
			myStream.getVideoTracks().forEach(track => {
				track.stop();
			});

			myStream.getAudioTracks().forEach(track => {
				track.stop();
			});

			socketRef.current.destroy();
			navigate(`/chats`);
		});
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
					<div>Title</div>
					<div className="tools">
						{turnOnCamera
							? <FontAwesomeIcon
								icon={faVideo}
								title={t("TurnOffCamera")}
								className="camera"
								onClick={async () => await switchCameraAsync(!turnOnCamera)}
							/>
							: <FontAwesomeIcon
								icon={faVideoSlash}
								title={t("TurnOnCamera")}
								className="camera"
								onClick={async () => await switchCameraAsync(!turnOnCamera)}
							/>
						}
						{turnOnMicrophone
							? <FontAwesomeIcon
								icon={faMicrophone}
								title={t("TurnOffMicrophone")}
								className="microphone"
								onClick={async () => await switchMicrophoneAsync(!turnOnMicrophone)}
							/>
							: <FontAwesomeIcon
								icon={faMicrophoneSlash}
								title={t("TurnOnMicrophone")}
								className="microphone"
								onClick={async () => await switchMicrophoneAsync(!turnOnMicrophone)}
							/>
						}
						<div className="btn-shadow" title={t("Leave")} onClick={leave}>
							<FontAwesomeIcon
								icon={faRightFromBracket}
							/>
							<div>{t("Leave")}</div>
						</div>
					</div>
				</div>
				<div className="voice__content">
					{turnOnCamera
						? <video className="me" playsInline ref={userVideoRef} autoPlay />
						: <div>{me?.username}</div>
					}
					<ul className="video-container">
						{peers.map((peer, index) =>
							<li key={index} className="video">
								<VoiceChatUser
									peer={peer}
								/>
							</li>
						)}
					</ul>
				</div>
			</div>
		</>
	)
}

export default memo(VoiceChat);