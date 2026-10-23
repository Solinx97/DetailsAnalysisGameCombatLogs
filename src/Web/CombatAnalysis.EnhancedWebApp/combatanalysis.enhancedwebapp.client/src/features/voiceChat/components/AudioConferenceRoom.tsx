import {
    ControlBar,
    LayoutContextProvider,
    ParticipantAudioTile,
    TrackLoop,
    useTracks,
    type AudioConferenceProps,
} from '@livekit/components-react';
import React from 'react';
import NoiseCancellationToggle from './NoiseCancellationToggle';

import { Track } from 'livekit-client';

const AudioConferenceRoom: React.FC = ({ ...props }: AudioConferenceProps) => {
    const audioTracks = useTracks([Track.Source.Microphone]);

    return (
        <LayoutContextProvider>
            <div className="lk-audio-conference" {...props}>
                <div className="lk-audio-conference-stage">
                    <TrackLoop tracks={audioTracks}>
                        <ParticipantAudioTile />
                    </TrackLoop>
                </div>
                <div className="control">
                    <NoiseCancellationToggle />
                    <ControlBar
                        controls={{ microphone: true, screenShare: false, camera: false, chat: false }}
                    />
                </div>
            </div>
        </LayoutContextProvider>
    );
}

export default AudioConferenceRoom;