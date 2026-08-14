import logger from '@/utils/Logger';
import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type SetStateAction } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useLeaveCommunityUserMutation } from '../../api/CommunityUser.api';
import type { CommunityModel } from '../../types/CommunityModel';
import CommunityMenuRestrictionItems from './CommunityMenuRestrictionItems';
import CommunityMenuRestrictionContent from './CommunityMenuRestrictionContent';

import './CommunityMenu.scss';

interface CommunityMenuProps {
    setShowMenu: (value: SetStateAction<boolean>) => void;
    user: AppUserModel;
    community: CommunityModel;
    setCommunity: (value: SetStateAction<CommunityModel | null>) => void;
}

const CommunityMenu: React.FC<CommunityMenuProps> = ({ setShowMenu, user, community, setCommunity }) => {
    const { t } = useTranslation('communication/community/communityMenu');

    const navigate = useNavigate();

    const [itemIndex, setItemIndex] = useState(0);
    const [showLeave, setShowLeave] = useState(false);

    const [leaveCommunityUserAsync] = useLeaveCommunityUserMutation();

    const leaveFromCommunityAsync = async () => {
        try {
            await leaveCommunityUserAsync({ appUserId: user.id, communityId: community.id }).unwrap();
            navigate('/communities');
        } catch (e) {
            logger.error("Failed to leave from community", e);
        }
    }

    return (
        <div className="communication-content community-menu box-shadow">
            {showLeave &&
                <div className="leave-from-community">
                    <div className="leave-from-community__title">{t("LeaveAlert")}</div>
                    <div>
                        <div>{t("LeaveConfirm")}?</div>
                    </div>
                    <div className="actions">
                        <button className="btn btn-outline-danger" onClick={leaveFromCommunityAsync}>{t("Leave")}</button>
                        <button className="btn btn-outline-success" onClick={() => setShowLeave((item) => !item)}>{t("Cancel")}</button>
                    </div>
                </div>
            }
            <div className="community-menu__content">
                <ul className="community-menu__menu">
                    {user.id === community.appUserId &&
                        <CommunityMenuRestrictionItems
                            itemIndex={itemIndex}
                            seItemIndex={setItemIndex}
                            user={user}
                            community={community}
                        />
                    }
                    <li className="menu-item__leave">
                        <div className="btn-shadow" onClick={() => setShowLeave((item) => !item)}>{t("Leave")}</div>
                    </li>
                </ul>
                {user.id === community.appUserId &&
                    <>
                        <CommunityMenuRestrictionContent
                            itemIndex={itemIndex}
                            user={user}
                            community={community}
                            setCommunity={setCommunity}
                        />
                        <div className="close">
                            <FontAwesomeIcon
                                icon={faCircleXmark}
                                title={t("Close") || ""}
                                onClick={() => setShowMenu(false)}
                            />
                        </div>
                    </>
                }
            </div>
            <div className="finish-create">
                <div className="btn-shadow" onClick={() => setShowMenu(false)}>{t("Cancel")}</div>
            </div>
        </div>
    );
}

export default CommunityMenu;