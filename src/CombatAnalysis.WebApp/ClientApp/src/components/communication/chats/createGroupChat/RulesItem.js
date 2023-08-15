import { useTranslation } from 'react-i18next';

const RulesItem = () => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    return (
        <>
            <div>Rules</div>
            <div>
                <input type="button" value="Next" className="btn btn-success" />
                <input type="button" value={t("Close")} className="btn btn-light" />
            </div>
        </>
    );
}

export default RulesItem;