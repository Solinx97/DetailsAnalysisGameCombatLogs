import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useRemoveCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const CommunityMemberItem = ({ community, comunityUser, customerId, showRemovePeople }) => {
    const { t } = useTranslation("communication/community/communityMemberItem");

    const [showRemovePeopleAlert, setShowRemovePeopleAlert] = useState(false);

    const [removeCommunityUserAsync] = useRemoveCommunityUserAsyncMutation();
    const { data: member, isLoading } = useGetCustomerByIdQuery(comunityUser.customerId);

    const removePeopleAsync = async () => {
        const deletedItemCount = await removeCommunityUserAsync(comunityUser.id);
        if (deletedItemCount.data !== undefined) {
            setShowRemovePeopleAlert(false);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            {showRemovePeopleAlert &&
                <div className="remove-people">
                    <div>{t("RemovePerson")}</div>
                    <div>
                        <div>{t("RemovePeopleAlertStart")} <strong>'{member.username}'</strong> {t("RemovePeopleAlertFinish")} <strong>'{community.name}'</strong>?</div>
                    </div>
                    <div>
                        <button className="btn btn-outline-warning" onClick={async () => await removePeopleAsync()}>{t("Remove")}</button>
                        <button className="btn btn-outline-success" onClick={() => setShowRemovePeopleAlert((item) => !item)}>{t("Cancel")}</button>
                    </div>
                </div>
            }
            <div className="member">
                {(showRemovePeople && member?.id !== community.ownerId && customerId !== member?.id) &&
                    <FontAwesomeIcon
                        icon={faTrash}
                        title={t("Remove")}
                        onClick={() => setShowRemovePeopleAlert((item) => !item)}
                    />
                }
                <div className="member__username">{member?.username}</div>
            </div>
        </>
    );
}

export default CommunityMemberItem;