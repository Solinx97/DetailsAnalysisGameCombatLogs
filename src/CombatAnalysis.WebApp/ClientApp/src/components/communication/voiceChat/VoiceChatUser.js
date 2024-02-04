import { memo, useEffect, useRef, useState } from "react";

const VoiceChatUser = ({ peer, socket }) => {
    const streamRef = useRef(null);

    const [currentStream, setCurrentStream] = useState(null);
    const [cameraTurnOn, setCameraTurnOn] = useState(false);

    useEffect(() => {
        peer.on("stream", stream => {
            setCurrentStream(stream);
        });

        socket.on("cameraSwitched", status => {
            setCameraTurnOn(status.cameraStatus);
        });
    }, []);

    useEffect(() => {
        if (streamRef.current === null || currentStream === null) {
            return;
        }

        streamRef.current.srcObject = currentStream;
        streamRef.current.play();
    }, [currentStream, cameraTurnOn]);

    return (
        <>
            {cameraTurnOn
                ? <video className="another" playsInline ref={streamRef} />
                : <div>STOP</div>
            }
        </>
    );
}

export default memo(VoiceChatUser);