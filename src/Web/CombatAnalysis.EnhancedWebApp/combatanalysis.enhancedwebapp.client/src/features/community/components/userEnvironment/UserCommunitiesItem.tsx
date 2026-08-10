import { faCommentDots } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CommunityModel } from '../../types/CommunityModel';

interface UserCommunitiesItemProps {
    myCommunity: CommunityModel;
    filterContent: string;
}

const UserCommunitiesItem: React.FC<UserCommunitiesItemProps> = ({ myCommunity, filterContent }) => {
    const { t } = useTranslation('communication/myEnvironment/myCommunitiesItem');

    const navigate = useNavigate();

    if (!myCommunity.name.toLowerCase().startsWith(filterContent.toLowerCase())) {
        return <></>;
    }

    return (
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
    );
}

export default UserCommunitiesItem;