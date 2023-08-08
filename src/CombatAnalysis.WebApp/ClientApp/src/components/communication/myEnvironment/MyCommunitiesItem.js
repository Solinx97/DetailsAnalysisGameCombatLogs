import { useTranslation } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { useGetCommunityByIdQuery } from '../../../store/api/Community.api';

const MyCommunitiesItem = ({ userCommunity, filterContent }) => {
    const { t } = useTranslation("communication/myEnvironment/myCommunitiesItem");

    const { data: myCommunity, isLoading } = useGetCommunityByIdQuery(userCommunity?.communityId);

    if (isLoading) {
        return <></>;
    }

    return (
        myCommunity.name.toLowerCase().startsWith(filterContent.toLowerCase()) &&
        <div className="community">
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{myCommunity?.name}</h5>
                    <p className="card-text">{myCommunity?.description}</p>
                    <NavLink className="card-link" to={`/community?id=${myCommunity?.id}`}>{t("Open")}</NavLink>
                </div>
            </div>
        </div>
    );
}

export default MyCommunitiesItem;