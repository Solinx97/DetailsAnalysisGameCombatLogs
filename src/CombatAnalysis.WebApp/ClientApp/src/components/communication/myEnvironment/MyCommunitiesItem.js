import { faCommentDots } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useGetCommunityByIdQuery } from '../../../store/api/communication/community/Community.api';

const MyCommunitiesItem = ({ userCommunity, filterContent }) => {
    const { t } = useTranslation("communication/myEnvironment/myCommunitiesItem");

    const navigate = useNavigate();

    const { data: myCommunity, isLoading } = useGetCommunityByIdQuery(userCommunity?.communityId);

    if (isLoading) {
        return <></>;
    }

    return (
        myCommunity.name.toLowerCase().startsWith(filterContent.toLowerCase()) &&
        <div className="community">
            <div className="card box-shadow">
                <div className="card-body">
                    <h5 className="card-title">{myCommunity?.name}</h5>
                    <p className="card-text">{myCommunity?.description}</p>
                    <div className="open-community">
                        <div className="btn-shadow" onClick={() => navigate(`/community?id=${myCommunity?.id}`)}>
                            <FontAwesomeIcon
                                icon={faCommentDots}
                            />
                            <div>{t("Open")}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default MyCommunitiesItem;