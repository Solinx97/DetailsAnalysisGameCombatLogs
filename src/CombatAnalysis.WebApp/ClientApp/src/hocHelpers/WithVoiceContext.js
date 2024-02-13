import { VoiceServiceConsumer } from '../context/VoiceServiceContext';

const WithVoiceContext = (Wrapped) => {
    return (props) => {
        return (
            <VoiceServiceConsumer>
                {
                    (callMinimazedData) => {
                        return (
                            <Wrapped
                                {...props}
                                callMinimazedData={callMinimazedData}
                            />
                        );
                    }
                }
            </VoiceServiceConsumer>
        );
    }
}

export default WithVoiceContext;