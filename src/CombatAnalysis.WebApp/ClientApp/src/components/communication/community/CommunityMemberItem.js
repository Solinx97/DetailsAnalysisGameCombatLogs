import { faPersonWalkingArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useRemoveCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import User from '../User';

const CommunityMemberItem = ({ community, comunityUser, customer, showRemovePeople }) => {
    const { t } = useTranslation("communication/community/communityMemberItem");

    const [showRemovePeopleAlert, setShowRemovePeopleAlert] = useState(false);
    const [userInformation, setUserInformation] = useState(null);

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
                        <div>{t("RemovePeopleAlertStart")} '{member.username}' {t("RemovePeopleAlertFinish")} '{community.name}'?</div>
                    </div>
                    <div className="actions">
                        <button className="btn btn-outline-warning" onClick={async () => await removePeopleAsync()}>{t("Remove")}</button>
                        <button className="btn btn-outline-success" onClick={() => setShowRemovePeopleAlert((item) => !item)}>{t("Cancel")}</button>
                    </div>
                </div>
            }
            <div className="member">
                {(showRemovePeople && member?.id !== community.ownerId && customer?.id !== member?.id) &&
                    <FontAwesomeIcon
                        icon={faPersonWalkingArrowRight}
                        title={t("Remove")}
                        onClick={() => setShowRemovePeopleAlert((item) => !item)}
                    />
                }
                <User
                    userId={member?.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                />
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default CommunityMemberItem;