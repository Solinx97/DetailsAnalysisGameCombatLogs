import { faCircleDown, faCircleUp, faGauge, faHandFist, faLocationCrosshairs, faMeteor, faStopwatch20 } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';

const fixedNumberUntil = 2;

const HealDoneGemeralHelper = ({ generalData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    return generalData?.map((item) => (
        <li key={item.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{item.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faHandFist}
                            className="list-group-item__value"
                            title={t("TotalHealing")}
                        />
                        <div>{item.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faGauge}
                            className="list-group-item__average-value"
                            title={t("AverageValue")}
                        />
                        <div>{item.averageValue.toFixed(fixedNumberUntil)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faStopwatch20}
                            className="list-group-item__damage-per-second"
                            title={t("HealingPerSec")}
                        />
                        <div>{item.healPerSecond.toFixed(fixedNumberUntil)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                            className="list-group-item__cast-number"
                            title={t("CountOfSkills")}
                        />
                        <div>{item.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faMeteor}
                            className="list-group-item__crit-number"
                            title={t("CountOfCrits")}
                        />
                        <div>{item.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faCircleUp}
                            className="list-group-item__max-value"
                            title={t("MaxValue")}
                        />
                        <div>{item.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faCircleDown}
                            className="list-group-item__min-value"
                            title={t("MinValue")}
                        />
                        <div>{item.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>
    ));
}

export default HealDoneGemeralHelper;