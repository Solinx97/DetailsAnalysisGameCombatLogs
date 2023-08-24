import { faPersonWalkingArrowRight, faQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useRemoveCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import UserInformation from '../UserInformation';

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

    const openUserInformationWithTimeout = (targetCustomer) => {
        setUserInformation(
            <UserInformation
                customer={customer}
                people={targetCustomer}
                closeUserInformation={closeUserInformation}
            />
        );
    }

    const closeUserInformation = () => {
        setUserInformation(null);
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
                <div className="member__username"
                    onClick={() => openUserInformationWithTimeout(member)}>
                    {customer?.id !== member?.id &&
                        <FontAwesomeIcon
                            icon={faQuestion}
                            title={t("Information")}
                            onClick={() => openUserInformationWithTimeout(member)}
                        />
                    }
                    <div>{member?.username}</div>
                </div>
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default CommunityMemberItem;