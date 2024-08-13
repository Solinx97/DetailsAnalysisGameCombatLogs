import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';
import useRTCVoiceChat from '../../../hooks/useRTCVoiceChat';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatAudioDeviceSettings from './VoiceChatAudioDeviceSettings';
import VoiceChatMembers from './VoiceChatMembers';

export const VoiceChat = () => {
    const { t } = useTranslation("communication/chats/groupChat");

    const navigate = useNavigate();

    const me = useSelector((state) => state.user.value);

    const [openVideoSettings, setOpenVideoSettings] = useState(false);
    const [openAudioSettings, setOpenAudioSettings] = useState(false);

    const [turnOnMicrophone, setTurnOnMicrophone] = useState(true);
    const [turnOnMicrophoneActive, setTurnOnMicrophoneActive] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);
    const [screenSharing, setScreenSharing] = useState(false);

    const { roomId, chatName } = useParams();

    const [connection, peerConnection, streamRef, connectToChatAsync, cleanupResources, createOfferAsync, switchMicrophoneStatusAsync, switchCameraStatusAsync] = useRTCVoiceChat(roomId);

    useEffect(() => {
        if (me === undefined) {
            return;
        }

        const connectToChat = async () => {
            const signalingAddress = "/voiceChatHub";
            await connectToChatAsync(signalingAddress);
        };

        connectToChat();

        return () => {
            cleanupResources();
        };
    }, [me]);

    useEffect(() => {
        const switchMicrophoneStatus = async () => {
            await switchMicrophoneStatusAsync(turnOnMicrophone);
        };

        switchMicrophoneStatus();
    }, [connection, peerConnection, turnOnMicrophone]);

    useEffect(() => {
        if (connection === null) {
            return;
        }

        connection.on("Connected", async () => {
            setTurnOnMicrophoneActive(true);
        });
    }, [connection]);

    useEffect(() => {
        if (connection === null) {
            return;
        }

        const switchCameraStatus = async () => {
            await switchCameraStatusAsync(turnOnCamera);
        };

        switchCameraStatus();
    }, [turnOnCamera]);

    const leaveFromCallAsync = async () => {
        cleanupResources();

        navigate("/chats");
    };

    const handleOpenVideoSettings = () => {
        setOpenAudioSettings(false);
        setOpenVideoSettings(!openVideoSettings);
    };

    const handleOpenAudioSettings = () => {
        setOpenVideoSettings(false);
        setOpenAudioSettings(!openAudioSettings);
    };

    return (
        <>
            <CommunicationMenu
                currentMenuItem={1} />
            <div className="voice">
                <div className="voice__title">
                    <div>{chatName}</div>
                    <div className="tools">
                        <div className="device">
                            <FontAwesomeIcon
                                icon={screenSharing ? faDisplay : faLinkSlash}
                                title={screenSharing ? t("TurnOffScreenSharing") : t("TurnOnScreenSharing")}
                                className="device__screen"
                                onClick={() => setScreenSharing(!screenSharing)} />
                        </div>
                        {turnOnMicrophoneActive &&
                            <div className="device">
                                <FontAwesomeIcon
                                    icon={turnOnCamera ? faVideo : faVideoSlash}
                                    title={turnOnCamera ? t("TurnOffCamera") : t("TurnOnCamera")}
                                    className="device__camera"
                                    onClick={() => setTurnOnCamera(!turnOnCamera)} />
                                <FontAwesomeIcon
                                    icon={openVideoSettings ? faAngleDown : faAngleUp}
                                    title={t("Setting")}
                                    className="device__settings"
                                    onClick={handleOpenVideoSettings} />
                                {openVideoSettings &&
                                    <VoiceChatAudioDeviceSettings />}
                            </div>}
                        <div className="device">
                            <FontAwesomeIcon
                                icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
                                title={turnOnMicrophone ? t("TurnOffMicrophone") : t("TurnOnMicrophone")}
                                className="device__microphone"
                                onClick={() => setTurnOnMicrophone(!turnOnMicrophone)} />
                            <FontAwesomeIcon
                                icon={openAudioSettings ? faAngleDown : faAngleUp}
                                title={t("Setting")}
                                className="device__settings"
                                onClick={handleOpenAudioSettings} />
                            {/*{openAudioSettings &&*/}
                            {/*	<VoiceChatAudioDeviceSettings*/}
                            {/*		setMicrophoneDeviceId={voice.func.setMicrophoneDeviceId}*/}
                            {/*		switchMicrophoneDevice={voice.func.switchMicrophoneDevice}*/}
                            {/*		switchAudioOutputDevice={voice.func.switchAudioOutputDevice}*/}
                            {/*		microphoneIsOn={voice.data.turnOnMicrophone}*/}
                            {/*	/>*/}
                            {/*}*/}
                        </div>
                        <div className="btn-shadow" title={t("Leave")} onClick={async () => await leaveFromCallAsync()}>
                            <FontAwesomeIcon
                                icon={faRightFromBracket} />
                            <div>{t("Leave")}</div>
                        </div>
                    </div>
                </div>
                <VoiceChatMembers
                    roomId={roomId}
                    connection={connection}
                    peerConnection={peerConnection}
                    micStatus={turnOnMicrophone}
                    cameraStatus={turnOnCamera}
                    localStream={streamRef.current}
                    createOfferAsync={createOfferAsync}
                    switchCameraStatusAsync={switchCameraStatusAsync} />
            </div>
        </>
    );
};
