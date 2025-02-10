import { RecomendationsProps } from "../../types/components/communication/RecomendationsProps";

const Recomendations: React.FC<RecomendationsProps> = ({ t }) => {
    return (
        <div className="communication-recomendations">
            <div className="title">{t("Recomendations")}</div>
            <div className="recomendations-list">
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="communication-reason-2" id="communication-reason-2" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="communication-reason-2">{t("RecomendationsByTags")}</label>
                </div>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" disabled />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("UseRecomendations")}</label>
            </div>
        </div>
    );
}

export default Recomendations;