import { VoiceServiceConsumer } from '../context/VoiceServiceContext';

const WithVoiceContext = (Wrapped) => {
    return (props) => {
        return (
            <VoiceServiceConsumer>
                {
                    (call) => {
                        return (
                            <Wrapped
                                {...props}
                                callMinimazedData={call.callMinimazedData}
                                useMinimaze={call.useMinimaze}
                                setUseMinimaze={call.setUseMinimaze}
                            />
                        );
                    }
                }
            </VoiceServiceConsumer>
        );
    }
}

export default WithVoiceContext;