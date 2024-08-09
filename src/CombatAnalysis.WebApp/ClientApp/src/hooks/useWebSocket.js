import { useRef } from 'react';

const useWebSocket = (turnOnMicrophone) => {
	const audioContextRef = useRef(new (window.AudioContext || window.webkitAudioContext)());
	const audioBufferQueueRef = useRef([]);
	const scriptProcessorRef = useRef(null);
	const socketRef = useRef(null);
	const streamRef = useRef(null);

	const connectToChat = (serverUrl) => {
		try {
			const socket = new WebSocket(serverUrl);
			socketRef.current = socket;

			socket.addEventListener("open", async () => {
				console.log('WebSocket connection established');

				socket.send("JOINED");
			});

			socket.addEventListener("message", async (event) => {
				if (event.data instanceof Blob) {
					const arrayBuffer = await event.data.arrayBuffer();
					const audioBuffer = await audioContextRef.current.decodeAudioData(arrayBuffer);
					audioBufferQueueRef.current.push(audioBuffer);

					playAudioQueue();
				}
			});

			socket.onerror = (error) => {
				console.error('WebSocket error:', error);
			}

			socket.onclose = () => {
				console.log('WebSocket connection closed');
			}
		} catch (error) {
			console.log(error);
		}
	}

	const convertToWav = (buffer) => {
		const numOfChannels = buffer.numberOfChannels;
		const length = buffer.length * numOfChannels * 2 + 44;
		const result = new ArrayBuffer(length);
		const view = new DataView(result);
		const channels = [];
		let offset = 0;
		let pos = 0;

		// Write WAV header
		setUint32(0x46464952); // "RIFF"
		setUint32(length - 8); // file length - 8
		setUint32(0x45564157); // "WAVE"

		setUint32(0x20746d66); // "fmt " chunk
		setUint32(16); // length = 16
		setUint16(1); // PCM (uncompressed)
		setUint16(numOfChannels);
		setUint32(buffer.sampleRate);
		setUint32(buffer.sampleRate * 2 * numOfChannels); // avg. bytes/sec
		setUint16(numOfChannels * 2); // block-align
		setUint16(16); // 16-bit (hardcoded in this demo)

		setUint32(0x61746164); // "data" - chunk
		setUint32(length - pos - 4); // chunk length

		// Write interleaved data
		for (let i = 0; i < buffer.numberOfChannels; i++) {
			channels.push(buffer.getChannelData(i));
		}

		while (pos < length) {
			for (let i = 0; i < numOfChannels; i++) {
				const sample = Math.max(-1, Math.min(1, channels[i][offset])); // clamp
				view.setInt16(pos, sample < 0 ? sample * 0x8000 : sample * 0x7FFF, true); // convert to PCM
				pos += 2;
			}
			offset++;
		}

		return result;

		function setUint16(data) {
			view.setUint16(pos, data, true);
			pos += 2;
		}

		function setUint32(data) {
			view.setUint32(pos, data, true);
			pos += 4;
		}
	}

	const startRecording = (socket, stream) => {
		try {
			const audioContext = audioContextRef.current;
			const source = audioContext.createMediaStreamSource(stream);
			const scriptProcessor = audioContext.createScriptProcessor(4096, 1, 1);

			scriptProcessor.addEventListener("audioprocess", (event) => {
				const inputBuffer = event.inputBuffer;

				// Convert to WAV format
				const wavData = convertToWav(inputBuffer);

				// Send audio data to WebSocket
				if (socket && socket.readyState === WebSocket.OPEN) {
					socket.send(wavData);
				}
			});

			source.connect(scriptProcessor);
			scriptProcessor.connect(audioContext.destination);

			scriptProcessorRef.current = scriptProcessor;
		} catch (error) {
			console.error('Error starting audio recording:', error);
		}
	}

	const playAudioQueue = () => {
		if (audioBufferQueueRef.current.length === 0) {
			return;
		}

		const audioBuffer = audioBufferQueueRef.current.shift();
		const source = audioContextRef.current.createBufferSource();
		source.buffer = audioBuffer;
		source.connect(audioContextRef.current.destination);
		source.start();
		source.onended = playAudioQueue;
	}

	const switchMicrophoneStatusAsync = async () => {
		if (turnOnMicrophone) {
			const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
			streamRef.current = stream;

			startRecording(socketRef.current, stream);

			return;
		}

		if (streamRef.current) {
			streamRef.current.getTracks().forEach(track => track.stop());
			streamRef.current = null;
		}

		if (scriptProcessorRef.current) {
			scriptProcessorRef.current.disconnect();
			scriptProcessorRef.current.onaudioprocess = null;
			scriptProcessorRef.current = null;
		}

		audioBufferQueueRef.current = [];
		audioContextRef.current = new (window.AudioContext || window.webkitAudioContext)();
	}

	const cleanupAudioResources = () => {
		// Stop the scriptProcessor
		if (scriptProcessorRef.current) {
			scriptProcessorRef.current.disconnect();
			scriptProcessorRef.current.onaudioprocess = null;
			scriptProcessorRef.current = null;
		}

		// Stop the media stream tracks
		if (streamRef.current) {
			streamRef.current.getTracks().forEach(track => track.stop());
			streamRef.current = null;
		}

		// Close the audio context
		if (audioContextRef.current && audioContextRef.current.state !== 'closed') {
			audioContextRef.current.close().catch(error => console.error('Error closing audio context:', error));
		}

		// Close the WebSocket connection
		if (socketRef.current) {
			socketRef.current.send("LEAVED");

			socketRef.current.close();
			socketRef.current = null;
		}

		audioBufferQueueRef.current = [];
		audioContextRef.current = new (window.AudioContext || window.webkitAudioContext)();
	}

	return [socketRef, connectToChat, cleanupAudioResources, switchMicrophoneStatusAsync];
}

export default useWebSocket;