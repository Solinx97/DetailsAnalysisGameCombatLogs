import { memo, useEffect, useRef, useState } from "react";

const VoiceChatUser = ({ peer, socket }) => {
    const streamRef = useRef(null);

    const [currentStream, setCurrentStream] = useState(null);
    const [cameraTurnOn, setCameraTurnOn] = useState(true);

    useEffect(() => {
        peer.on("stream", stream => {
            streamRef.current.srcObject = stream;
            streamRef.current.play();

            setCurrentStream(stream);
        });

        socket.on("cameraSwitched", status => {
            setCameraTurnOn(status);
        });
    }, []);

    useEffect(() => {
        if (streamRef.current === null || currentStream === null) {
            return;
        }

        streamRef.current.srcObject = currentStream;
        streamRef.current.play();
    }, [cameraTurnOn]);

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