import { useKrispNoiseFilter } from '@livekit/components-react/krisp';
import { useTranslation } from 'react-i18next';

const NoiseCancellationToggle = () => {
    const { t } = useTranslation('communication/chats/voiceChat');
    
    const krisp = useKrispNoiseFilter();

    return (
        <div className="noise-filter">
            <div className="form-check">
                <input
                    className="form-check-input"
                    type="checkbox"
                    id="flexCheckChecked"
                    checked={krisp.isNoiseFilterEnabled}
                    disabled={krisp.isNoiseFilterPending}
                    onChange={(e) => {
                        void krisp.setNoiseFilterEnabled(e.target.checked);
                    }}
                />
                <label className="form-check-label" htmlFor="flexCheckChecked">
                    {t("NoiseReduction")}
                </label>
            </div>
        </div>
    );
}

export default NoiseCancellationToggle;