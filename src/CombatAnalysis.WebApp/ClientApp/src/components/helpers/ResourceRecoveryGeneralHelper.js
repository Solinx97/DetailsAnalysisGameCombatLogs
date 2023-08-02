import { faCircleDown, faCircleUp, faGauge, faHandFist, faLocationCrosshairs, faStopwatch20 } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';

const ResourceRecoveryGeneralHelper = ({ generalData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const createGeneralItem = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("TotalResourcesRecovery")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title={t("AverageValue")} />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title={t("ResourcesRecoveryPerSec")} />
                        <div>{element.resourcePerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title={t("CountOfSkills")} />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title={t("MaxValue")} />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title={t("MinValue")} />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const render = () => {
        const list = generalData.map((element) => createGeneralItem(element));

        return list;
    }

    return render();
}

export default ResourceRecoveryGeneralHelper;