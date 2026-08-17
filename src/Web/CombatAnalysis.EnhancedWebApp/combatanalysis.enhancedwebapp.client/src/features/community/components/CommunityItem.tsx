import logger from '@/utils/Logger';
import { faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type JSX, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import User from '../../user/components/User';
import type { AppUserModel } from '../../user/types/AppUserModel';
import { useCreateCommunityUserMutation } from '../api/CommunityUser.api';
import type { CommunityUserModel } from '../types/CommunityUserModel';
import type { CommunityModel } from '../types/CommunityModel';

interface CommunityItemProps {
    community: CommunityModel;
    targetUser: AppUserModel | null;
}

const CommunityItem: React.FC<CommunityItemProps> = ({ community, targetUser }) => {
    const { t } = useTranslation('communication/community/Communities');
    
    const navigate = useNavigate();

    const [canJoin, setCanJoin] = useState(true);
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const [createCommunityUser] = useCreateCommunityUserMutation();

    const createCommunityUserAsync = async () => {
        try {
            const newCommunityUser: CommunityUserModel = {
                id: crypto.randomUUID(),
                username: targetUser?.username ?? "",
                appUserId: targetUser?.id ?? "",
                communityId: community.id
            };

            await createCommunityUser(newCommunityUser).unwrap();
            navigate(`/community?id=${community.id}`);
        } catch (e) {
            logger.error(`Failed to create community user to community: ${community.id}`, e);
        }
    }

    return (
        <div>
            {community?.policyType !== 0 &&
                <div className="private-container">
                    <div className="private">{t("Private")}</div>
                </div>
            }
            <div className="card box-shadow">
                <div className="card-body">
                    <h5 className="card-title">{community?.name}</h5>
                    <p className="card-text">{community?.description}</p>
                    {community?.policyType === 0 &&
                        <>
                            <div className="links">
                                <div className="open-community">
                                    <div className="btn-shadow" onClick={() => navigate(`/community?id=${community?.id}`)}>
                                        <FontAwesomeIcon
                                            icon={faCircleQuestion}
                                        />
                                        <div>{t("Open")}</div>
                                    </div>
                                </div>
                                {/* {canJoin &&
                                    <div className="join-to-community">
                                        <div className="btn-shadow" onClick={async () => await createCommunityUserAsync()}>
                                            <FontAwesomeIcon
                                                icon={faCirclePlus}
                                            />
                                            <div>{t("Join")}</div>
                                        </div>
                                    </div>
                                } */}
                            </div>
                        </>
                    }
                </div>
            </div>
            <div className="owner-container">
                <div className="owner">
                    <User
                        targetUserId={community?.appUserId ?? ""}
                        setUserInformation={setUserInformation}
                    />
                </div>
            </div>
            {userInformation !== null &&
                <div className="owner-user-information">{userInformation}</div>
            }
        </div>
    );
}

export default CommunityItem;