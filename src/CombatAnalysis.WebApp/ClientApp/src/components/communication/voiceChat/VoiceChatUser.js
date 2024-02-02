import { memo, useEffect, useRef } from "react";

const VoiceChatUser = ({ peer }) => {
    const streamRef = useRef(null);

    useEffect(() => {
        peer.on("stream", stream => {
            console.log(stream);
            streamRef.current.srcObject = stream;
        });
    }, []);

    return (
        <video className="another" playsInline ref={streamRef} autoPlay />
    );
}

export default memo(VoiceChatUser);