import { memo, useEffect, useState } from "react";

const VoiceChatAudioDeviceSettings = ({ t, peerConnectionsRef, turnOnMicrophone, stream, audioInputDeviceId, setAudioInputDeviceId, audioOutputDeviceId, setAudioOutputDeviceId }) => {
	const [audioInputDevices, setAudioInputDevices] = useState([]);
	const [audioOutputDevices, setAudioOutputDevices] = useState([]);

	useEffect(() => {
		const fetchDevices = async () => {
			const { audioInputs, audioOutputs } = await getAvailableAudioDevicesAsync();

			setAudioInputDeviceId(audioInputDeviceId || audioInputs[0]?.deviceId);
			setAudioOutputDeviceId(audioOutputDeviceId || audioOutputs[0]?.deviceId);

			setAudioInputDevices(audioInputs);
			setAudioOutputDevices(audioOutputs);
		};

		fetchDevices();
	}, []);

	const getAvailableAudioDevicesAsync = async () => {
		const devices = await navigator.mediaDevices.enumerateDevices();
		const audioInputs = devices.filter(device => device.kind === "audioinput" && (device.deviceId !== "communications" || device.deviceId === "default"));
		const audioOutputs = devices.filter(device => device.kind === "audiooutput" && (device.deviceId !== "communications" || device.deviceId === "default"));

		return { audioInputs, audioOutputs };
	}

	const switchAudioInputDevice = async (deviceId) => {
		if (!stream) {
			return;
		}

		const newStream = await navigator.mediaDevices.getUserMedia({ audio: { deviceId: { exact: deviceId } } });
		const newAudioTrack = newStream.getAudioTracks()[0];
		newAudioTrack.enabled = turnOnMicrophone;

		const oldAudioTrack = stream.getAudioTracks()[0];
		stream.removeTrack(oldAudioTrack);
		stream.addTrack(newAudioTrack);

		for (const peerConnection of peerConnectionsRef.current.values()) {
			const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "audio");
			if (sender) sender.replaceTrack(newAudioTrack);
		}

		setAudioInputDeviceId(deviceId);
	}

	const switchAudioOutputDevice = (deviceId) => {
		setAudioOutputDeviceId(deviceId);
	}

	const renderDeviceList = (devices, currentDeviceId, switchDeviceFunction) => (
		<select
			className="form-select"
			value={currentDeviceId}
			onChange={async (e) => await switchDeviceFunction(e.target.value)}
		>
			{devices.map((device, index) => (
				<option key={index} value={device.deviceId}>
					{device.label.split("-")[0]}
				</option>
			))}
		</select>
	)

	return (
		<div className="device-toolbar">
			<div className="device-toolbar__title">{t("SoundSettings")}</div>
			{renderDeviceList(audioOutputDevices, audioOutputDeviceId, switchAudioOutputDevice)}
			<div className="device-toolbar__title">{t("MicrophoneSettings")}</div>
			{renderDeviceList(audioInputDevices, audioInputDeviceId, switchAudioInputDevice)}
		</div>
	);
}

export default memo(VoiceChatAudioDeviceSettings);