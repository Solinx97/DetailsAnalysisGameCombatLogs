import { faArrowsLeftRightToLine, faArrowsToCircle, faCheck, faComments, faEnvelopesBulk, faUser, faUserGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import Communication from "../Communication";
import UserPosts from '../UserPosts';
import Friends from '../myEnvironment/Friends';
import SelectedUserProfile from './SelectedUserProfile';

import '../../../styles/communication/people/user.scss';
import SelectedUserCommunities from './SelectedUserCommunities';

const SelectedUser = () => {
    const { t } = useTranslation("communication/people/user");

    const [customerId, setCustomerId] = useState(0);
    const [customer, setCustomer] = useState(null);
    const [currentMenuItem, setMenuItem] = useState(0);
    const [shortMenu, setShortMenu] = useState(false);

    const [getCustomerById] = useLazyGetCustomerByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCustomerId(queryParams.get("id"));
    }, [])

    useEffect(() => {
        if (customerId === 0) {
            return;
        }

        const getCustomer = async () => {
            await getCustomerByIdAsync();
        }

        getCustomer();
    }, [customerId])

    const getCustomerByIdAsync = async () => {
        const customer = await getCustomerById(customerId);
        if (customer.data !== undefined) {
            setCustomer(customer.data);
        }
    }

    return (
        <div className="communication">
            <Communication
                currentMenuItem={7}
            />
            <div className="communication__content user">
                <div className="user-information__username">
                    {customer?.username}
                </div>
                <div className="user__container">
                    <ul className="user__menu">
                        <li className="sub-menu" onClick={() => setMenuItem(0)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 0 ? "_active" : ""}`}
                                title={t("Profile")}
                                icon={faUser}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Profile")}</div>
                                    {currentMenuItem === 0 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        <li className="sub-menu" onClick={() => setMenuItem(1)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 1 ? "_active" : ""}`}
                                title={t("Posts")}
                                icon={faEnvelopesBulk}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Posts")}</div>
                                    {currentMenuItem === 1 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        <li className="sub-menu" onClick={() => setMenuItem(2)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 2 ? "_active" : ""}`}
                                title={t("Friends")}
                                icon={faUserGroup}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Friends")}</div>
                                    {currentMenuItem === 2 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        <li className="sub-menu" onClick={() => setMenuItem(3)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 3 ? "_active" : ""}`}
                                title={t("Communities")}
                                icon={faComments}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Communities")}</div>
                                    {currentMenuItem === 3 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        {shortMenu
                            ? <li className="sub-menu control">
                                <FontAwesomeIcon
                                    icon={faArrowsLeftRightToLine}
                                    title={t("FullMenu")}
                                    onClick={() => setShortMenu((item) => !item)}
                                />
                            </li>
                            : <li className="sub-menu control">
                                <FontAwesomeIcon
                                    icon={faArrowsToCircle}
                                    title={t("ShortMenu")}
                                    onClick={() => setShortMenu((item) => !item)}
                                />
                            </li>
                        }
                    </ul>
                    <div className="user__content">
                        {currentMenuItem === 0 &&
                            <SelectedUserProfile
                                customer={customer}
                            />
                        }
                        {currentMenuItem === 1 &&
                            <ul>
                                <UserPosts
                                    customer={customer}
                                    userId={customer?.id}
                                />
                            </ul>
                        }
                        {currentMenuItem === 2 &&
                            <Friends
                                customer={customer}
                                requestsToConnect={null}
                                allowRemoveFriend={false}
                            />
                        }
                    </div>
                    {currentMenuItem === 3 &&
                        <SelectedUserCommunities
                            customer={customer}
                        />
                    }
                </div>
            </div>
        </div>
    );
}

export default SelectedUser;