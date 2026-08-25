import type { RulesModel } from '../../types/community/RulesModel';

interface CommunityRulesItemProps {
    t: (key: string) => string;
    rules: RulesModel;
    setRules: React.Dispatch<React.SetStateAction<RulesModel>>;
}

const CommunityRulesItem: React.FC<CommunityRulesItemProps> = ({ t, rules, setRules }) => {
    return (
        <ul className="rules">
            <li>
                <div className="rules__title">{t("TypeOfCommunity")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="type-of-community" id="public" value={rules.policy}
                            onChange={() => setRules(prev => ({
                                ...prev,
                                policy: 0
                            }))}
                            checked={rules.policy === 0} />
                        <label className="form-check-label" htmlFor="public">{t("Public")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="type-of-community" id="private" value={rules.policy}
                            onChange={() => setRules(prev => ({
                                ...prev,
                                policy: 1
                            }))}
                            checked={rules.policy === 1}
                        />
                        <label className="form-check-label" htmlFor="private">{t("Private")}</label>
                    </div>
                </div>
            </li>
            <li>
                <div className="rules__title">{t("InviteOtherPeople")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="invite-people" id="invite-people-anyone" value={rules.invite}
                            onChange={() => setRules(prev => ({
                                ...prev,
                                invite: 0
                            }))}
                            checked={rules.invite === 0}
                            disabled />
                        <label className="form-check-label" htmlFor="invite-people-anyone">{t("Anyone")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="invite-people" id="invite-people-special" value={rules.invite}
                            onChange={() => setRules(prev => ({
                                ...prev,
                                invite: 1
                            }))}
                            checked={rules.invite === 1}
                            disabled
                        />
                        <label className="form-check-label" htmlFor="invite-people-special">{t("Owner")}</label>
                    </div>
                </div>
            </li>
            <li>
                <div className="rules__title">{t("RemoveAnotherPeople")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="remove-people" id="remove-people-anyone" value={rules.remove}
                            onChange={() => setRules(prev => ({
                                ...prev,
                                remove: 0
                            }))}
                            checked={rules.remove === 0}
                            disabled/>
                        <label className="form-check-label" htmlFor="remove-people-anyone">{t("Anyone")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="remove-people" id="remove-people-special" value={rules.remove}
                            onChange={() => setRules(prev => ({
                                ...prev,
                                remove: 1
                            }))}
                            checked={rules.remove === 1}
                            disabled
                        />
                        <label className="form-check-label" htmlFor="remove-people-special">{t("Owner")}</label>
                    </div>
                </div>
            </li>
        </ul>
    );
}

export default CommunityRulesItem;