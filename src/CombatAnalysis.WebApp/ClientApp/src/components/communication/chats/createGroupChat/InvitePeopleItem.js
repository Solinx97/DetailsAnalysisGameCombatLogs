import { useTranslation } from 'react-i18next';

const InvitePeopleItem = ({ createNewGroupChatAsync }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    return (
        <>
            <div>Invite pepople</div>
            <div>
                <input type="button" value={t("Create")} className="btn btn-success" onClick={async () => await createNewGroupChatAsync()} />
                <input type="button" value={t("Close")} className="btn btn-light" />
            </div>
        </>
    );
}

export default InvitePeopleItem;