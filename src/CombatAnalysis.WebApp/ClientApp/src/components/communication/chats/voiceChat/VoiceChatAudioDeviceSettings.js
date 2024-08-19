import { useEffect, useState } from "react";

const VoiceChatAudioDeviceSettings = ({ t, peerConnectionsRef, turnOnMicrophone, stream, audioInputDeviceIdRef, audioOutputDeviceId, setAudioOutputDeviceId }) => {
	const [audioInputDevices, setAudioInputDevices] = useState([]);
	const [audioOutputDevices, setAudioOutputDevices] = useState([]);

	useEffect(() => {
		const fetchDevices = async () => {
			const { audioInputs, audioOutputs } = await getAvailableAudioDevicesAsync();

			audioInputDeviceIdRef.current = !audioInputDeviceIdRef.current
				? audioInputs[0].deviceId
				: audioInputDeviceIdRef.current;
			setAudioOutputDeviceId(!audioOutputDeviceId
				? audioOutputs[0].deviceId
				: audioOutputDeviceId);

			setAudioInputDevices(audioInputs);
			setAudioOutputDevices(audioOutputs);
		}

		fetchDevices();
	}, [stream]);

	const getAvailableAudioDevicesAsync = async () => {
		const devices = await navigator.mediaDevices.enumerateDevices();
		const audioInputs = devices.filter(device => (device.kind === "audioinput" && device.deviceId !== "communications")
			|| (device.kind === "audioinput" && device.deviceId === "default"));
		const audioOutputs = devices.filter(device => (device.kind === "audiooutput" && device.deviceId !== "communications")
			|| (device.kind === "audiooutput" && device.deviceId === "default"));

		return { audioInputs, audioOutputs };
	}

	const switchAudioInputDevice = async (deviceId) => {
		if (!stream) {
			return;
		}

		const newStream = await navigator.mediaDevices.getUserMedia({
			audio: { deviceId: { exact: deviceId } }
		});

		const newAudioTrack = newStream.getAudioTracks()[0];
		newAudioTrack.enabled = turnOnMicrophone;

		// Replace the audio track in the existing stream
		const oldAudioTrack = stream.getAudioTracks()[0];
		stream.removeTrack(oldAudioTrack);
		stream.addTrack(newAudioTrack);

		for (const peerConnection of peerConnectionsRef.current.values()) {
			const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "audio");
			if (sender) {
				sender.replaceTrack(newAudioTrack);
			}
		}

		audioInputDeviceIdRef.current = deviceId;
	}

	const switchAudioOutputDevice = async (deviceId) => {
		setAudioOutputDeviceId(deviceId);
	}

	return (
		<div className="device-toolbar">
			<div className="device-toolbar__title">{t("SoundSettings")}</div>
			<ul>
				{audioOutputDevices.map((device, index) =>
					<li key={index} className="form-check">
						<input className="form-check-input" type="radio" name="soundDevice" id={`soundDevice${index}`}
							defaultChecked={device.deviceId === audioOutputDeviceId} onClick={() => switchAudioOutputDevice(device.deviceId)} />
						<label className="form-check-label" htmlFor={`soundDevice${index}`}>
							{device.label.split("-")[0]}
						</label>
					</li>
				)}
			</ul>
			<div className="device-toolbar__title">{t("MicrophoneSettings")}</div>
			<ul>
				{audioInputDevices.map((device, index) =>
					<li key={index} className="form-check">
						<input className="form-check-input" type="radio" name="microphoneDevice" id={`microphoneDevice${index}`}
							defaultChecked={device.deviceId === audioInputDeviceIdRef.current} onClick={() => switchAudioInputDevice(device.deviceId)} />
						<label className="form-check-label" htmlFor={`microphoneDevice${index}`}>
							{device.label.split("-")[0]}
						</label>
					</li>
				)}
			</ul>
		</div>
    );
}

export default VoiceChatAudioDeviceSettings;