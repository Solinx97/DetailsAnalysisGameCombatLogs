import { useEffect, useState } from "react";

const VoiceChatDeviceSettings = ({ isAudio = false, switchMicrophoneDevice, switchAudioOutputDevice }) => {
	const [audioInputDevices, setAudioInputDevices] = useState([]);
	const [audioOutputDevices, setAudioOutputDevices] = useState([]);
	const [videoDevices, setVideoDevices] = useState([]);

	useEffect(() => {
		getDevices();
	}, []);

	const getDevices = () => {
		navigator.mediaDevices.enumerateDevices().then(devices => {
			const targetAudioInputDevices = devices.filter(device => (device.kind === "audioinput" && device.deviceId !== "communications")
				|| (device.kind === "audioinput" && device.deviceId === "default"));
			const targetAudioOutputDevices = devices.filter(device => (device.kind === "audiooutput" && device.deviceId !== "communications")
				|| (device.kind === "audiooutput" && device.deviceId === "default"));
			const targetVideoDevices = devices.filter(device => (device.kind === "videoinput" && device.deviceId !== "communications")
				|| (device.kind === "videoinput" && device.deviceId === "default"));

			setAudioInputDevices(targetAudioInputDevices);
			setAudioOutputDevices(targetAudioOutputDevices);
			setVideoDevices(targetVideoDevices);
		});
	}

	const switchInputDevice = (index) => {
		switchMicrophoneDevice(audioInputDevices[index].deviceId);
	}

	const switchOutputDevice = (index) => {
		switchAudioOutputDevice(audioOutputDevices[index].deviceId);
	}

	return (
		<div className="device-toolbar">
			{isAudio
				? <>
					<div className="device-toolbar__title">Sound settings:</div>
					<ul>
						{audioOutputDevices.map((device, index) =>
							<li key={index} className="form-check">
								<input className="form-check-input" type="radio" name="soundDevice" id={`soundDevice${index}`}
									defaultChecked={index === 0 ? true : false} disabled={audioOutputDevices.length === 1} onClick={() => switchOutputDevice(index)} />
								<label className="form-check-label" htmlFor={`soundDevice${index}`}>
									{device.label.split("-")[0]}
								</label>
							</li>
						)}
					</ul>
					<div className="device-toolbar__title">Microphone settings:</div>
					<ul>
						{audioInputDevices.map((device, index) =>
							<li key={index} className="form-check">
								<input className="form-check-input" type="radio" name="microphoneDevice" id={`microphoneDevice${index}`}
									defaultChecked={index === 0 ? true : false} disabled={audioInputDevices.length === 1} onClick={() => switchInputDevice(index)} />
								<label className="form-check-label" htmlFor={`microphoneDevice${index}`}>
									{device.label.split("-")[0]}
								</label>
							</li>
						)}
					</ul>
				</>
				: <>
					<div className="device-toolbar__title">Camera settings:</div>
					<ul>
						{videoDevices.map((device, index) =>
							<li key={index} className="form-check">
								<input className="form-check-input" type="radio" name="videoDevice" id={`videoDevice${index}`}
									defaultChecked={index === 0 ? true : false} disabled={videoDevices.length === 1} />
								<label className="form-check-label" htmlFor={`videoDevice${index}`}>
									{device.label.split("-")[0]}
								</label>
							</li>
						)}
					</ul>
				</>
			}
		</div>
    );
}

export default VoiceChatDeviceSettings;