import { faCircleArrowLeft, faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';

const ItemConnector = ({ connectorType, previouslyStep = null, nextStep = null, previouslyStepIndex = 0, nextStepIndex = 0 }) => {
    const { t } = useTranslation("communication/create");

    switch (connectorType) {
        case 0:
            return <></>;
        case 1:
            return (
                <div className="item-result">
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                        title={t("NextStep")}
                        onClick={() => nextStep(nextStepIndex)}
                    />
                </div>
            );
        case 2:
            return (
                <div className="item-result">
                    <FontAwesomeIcon
                        icon={faCircleArrowLeft}
                        title={t("PrevioslyStep")}
                        onClick={() => previouslyStep(previouslyStepIndex)}
                    />
                </div>
            );
        case 3:
            return (
                <div className="item-result">
                    <FontAwesomeIcon
                        icon={faCircleArrowLeft}
                        title={t("PrevioslyStep")}
                        onClick={() => previouslyStep(previouslyStepIndex)}
                    />
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                        title={t("NextStep")}
                        onClick={() => nextStep(nextStepIndex)}
                    />
                </div>
            );
        default:
            return <></>;
    }
}

export default ItemConnector;