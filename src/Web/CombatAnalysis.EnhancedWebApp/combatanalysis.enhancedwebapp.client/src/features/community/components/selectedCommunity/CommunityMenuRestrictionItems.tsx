import { faCircleCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type SetStateAction } from "react";
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { CommunityModel } from '../../types/CommunityModel';

interface CommunityMenuRestrictionItemsProps {
    itemIndex: number;
    seItemIndex: (value: SetStateAction<number>) => void;
    user: AppUserModel;
    community: CommunityModel;
}

const CommunityMenuRestrictionItems: React.FC<CommunityMenuRestrictionItemsProps> = ({ itemIndex, seItemIndex, user, community, }) => {
    const { t } = useTranslation('communication/community/communityMenu');

    const changeMenuItem = (index: number) => {
        seItemIndex(index);
    }

    return (
        <>
            <li className="menu-item" onClick={() => changeMenuItem(0)}>
                {itemIndex === 0 &&
                    <FontAwesomeIcon
                        className="menu-item__passed"
                        icon={faCircleCheck}
                    />
                }
                <div>{t("Main")}</div>
            </li>
            <li className="menu-item" onClick={() => changeMenuItem(1)}>
                {itemIndex === 1 &&
                    <FontAwesomeIcon
                        className="menu-item__passed"
                        icon={faCircleCheck}
                    />
                }
                <div>{t("Members")}</div>
            </li>
            <li className="menu-item" onClick={() => changeMenuItem(2)}>
                {itemIndex === 2 &&
                    <FontAwesomeIcon
                        className="menu-item__passed"
                        icon={faCircleCheck}
                    />
                }
                <div>{t("InvitePeople")}</div>
            </li>
            <li className="menu-item" style={{ opacity: 0.5 }}>
                {itemIndex === 3 &&
                    <FontAwesomeIcon
                        className="menu-item__passed"
                        icon={faCircleCheck}
                    />
                }
                <div>{t("Permisions")}</div>
            </li>
            {community?.appUserId === user?.id &&
                <li className="menu-item" onClick={() => changeMenuItem(4)}>
                    {itemIndex === 4 &&
                        <FontAwesomeIcon
                            className="menu-item__passed"
                            icon={faCircleCheck}
                        />
                    }
                    <div>{t("Rules")}</div>
                </li>
            }
        </>
    );
}

export default CommunityMenuRestrictionItems;