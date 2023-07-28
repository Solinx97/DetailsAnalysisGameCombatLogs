import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import { useFriendSearchByUserIdQuery } from '../../store/api/UserApi';

import './../../styles/communication/userInformation.scss';

const UserInformation = ({ customer, closeUserInformation }) => {
    const { data: friends, isLoading } = useFriendSearchByUserIdQuery(customer?.id);

    const [friendsCount, setFriendsCount] = useState(0);

    useEffect(() => {
        friends && setFriendsCount(friends?.length);
    }, [friends])

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="user-information">
            <div className="user-information__menu">
                <FontAwesomeIcon icon={faCircleXmark} title="Close" onClick={closeUserInformation} />
            </div>
            <div className="user-information__username">
                {customer.username}
            </div>
            <div className="user-information__information">
                <ul className="user-information__common-information">
                    <li className="user-information-item">
                        <div className="title">First name</div>
                        <div className="content">{customer.firstName}</div>
                    </li>
                    <li className="user-information-item">
                        <div className="title">Last name</div>
                        <div className="content">{customer.lastName}</div>
                    </li>
                    <li className="user-information-item">
                        <div className="title">About me</div>
                        <div className="content">{customer.aboutMe}</div>
                    </li>
                </ul>
                <ul className="user-information__additional-information">
                    <li className="user-information-item">
                        <div className="title">Friends</div>
                        <div className="content">{friendsCount}</div>
                    </li>
                    <li className="user-information-item">
                        <div className="title">Communities</div>
                        <div className="content">{0}</div>
                    </li>
                </ul>
            </div>
            <div className="user-information__more-details">
                <NavLink className="card-link">More details</NavLink>
            </div>
        </div>
    );
}

export default memo(UserInformation);