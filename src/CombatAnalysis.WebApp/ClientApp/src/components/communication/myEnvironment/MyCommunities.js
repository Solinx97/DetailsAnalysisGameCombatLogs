import { faArrowsRotate, faCircleCheck, faCircleXmark, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { NavLink } from 'react-router-dom';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import CommunityService from '../../../services/CommunityService';
import CommunityUserService from '../../../services/CommunityUserService';
import CustomerService from '../../../services/CustomerService';
import InviteToCommunityService from '../../../services/InviteToCommunityService';
import Alert from '../../Alert';
import CreateCommunity from './CreateCommunity';

import '../../../styles/communication/communities.scss';

const MyCommunities = ({ openCommunity, setUserCommunitiesId = null }) => {
    const inviteToCommunityService = new InviteToCommunityService();
    const customerService = new CustomerService();
    const communityService = new CommunityService();
    const communityUserService = new CommunityUserService();

    const [, customer] = useAuthentificationAsync();

    const [showCreateCommunity, setShowCreateCommunity] = useState(false);
    const [showMyCommunities, setShowMyCommunities] = useState(true);
    const [communities, setCommunities] = useState(<></>);
    const [invitesToCommunity, setInvitesToCommunity] = useState(<></>);
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(false);

    useEffect(() => {
        let getMyCommunities = async () => {
            await getInvitesToCommunityAsync();
            await getUserCommunitiesAsync();
        }

        getMyCommunities();
    }, [customer])

    useEffect(() => {
        communities.key === undefined || communities.key === null
            ? setShowUpdatingAlert(false)
            : setShowUpdatingAlert(true);
    }, [communities])

    const getInvitesToCommunityAsync = async () => {
        const invitesToCommunity = await inviteToCommunityService.searchByUserId(customer?.id);
        if (invitesToCommunity === null) {
            return;
        }

        for (let i = 0; i < invitesToCommunity.length; i++) {
            const customer = await getCustomerByIdAsync(invitesToCommunity[i].ownerId);
            invitesToCommunity[i].username = customer.username;

            const comunity = await getCommunityByIdAsync(invitesToCommunity[i].communityId);
            invitesToCommunity[i].community = comunity.name;
        }

        fillInvitesToCommunity(invitesToCommunity);
    }

    const getCustomerByIdAsync = async (customerId) => {
        const customerById = await customerService.getByIdAsync(customerId);
        return customerById;
    }

    const getCommunityByIdAsync = async (communityId) => {
        const community = await communityService.getByIdAsync(communityId);
        return community;
    }

    const getUserCommunitiesAsync = async () => {
        const myCommunities = [];
        const myCommunitiesId = [];

        setShowUpdatingAlert(true);
        const userCommunities = await communityUserService.searchByUserId(customer?.id);
        if (userCommunities === null) {
            return;
        }

        for (let i = 0; i < userCommunities.length; i++) {
            const myCommunity = await getCommunityByIdAsync(userCommunities[i].communityId);
            myCommunities.push(myCommunity);
            myCommunitiesId.push(userCommunities[i].communityId);
        }

        if (setUserCommunitiesId != null) {
            setUserCommunitiesId(myCommunitiesId);
        }

        fillMyCommunities(myCommunities);
    }

    const fillMyCommunities = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createMyCommunityCard(element))
            : (<div className="community__empty">You don't have any communities</div>);

        setCommunities(list);
    }

    const fillInvitesToCommunity = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createInviteToCommunityCard(element))
            : (<div className="community__empty">You don't have any invites</div>);

        setInvitesToCommunity(list);
    }

    const createMyCommunityCard = (community) => {
        return (
            <li key={community.id} className="community">
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{community.name}</h5>
                        <p className="card-text">{community.description}</p>
                        <NavLink className="card-link" onClick={() => openCommunity(community)}>Open</NavLink>
                        <NavLink className="card-link">More details</NavLink>
                    </div>
                </div>
            </li>
        );
    }

    const acceptRequestAsync = async (inviteToCommunity) => {
        const newCommunityUser = {
            communityId: inviteToCommunity.communityId,
            customerId: customer.id
        };

        let createdItem = await communityUserService.createAsync(newCommunityUser);
        if (createdItem === null) {
            return;
        }

        createdItem = await inviteToCommunityService.deleteAsync(inviteToCommunity.id);
        if (createdItem === null) {
            return;
        }

        await getInvitesToCommunityAsync();
    }

    const rejectRequestAsync = async (inviteToCommunity) => {
        const createdItem = await inviteToCommunityService.deleteAsync(inviteToCommunity.id);
        if (createdItem === null) {
            return;
        }

        await getInvitesToCommunityAsync();
    }

    const createInviteToCommunityCard = (inviteToCommunity) => {
        return (
            <li key={inviteToCommunity.id} className="request-to-connect">
                <div>'{inviteToCommunity.username}' sent you invite to '{inviteToCommunity.community}'</div>
                <div className="request-to-connect__answer">
                    <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title="Accept"
                        onClick={async () => await acceptRequestAsync(inviteToCommunity)} /></div>
                    <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title="Reject"
                        onClick={async () => await rejectRequestAsync(inviteToCommunity)} /></div>
                </div>
            </li>
        );
    }

    const render = () => {
        return (
            <div>
                <Alert
                    isVisible={showUpdatingAlert}
                    content="Updating..."
                />
                {invitesToCommunity.length !== undefined
                    ? <div className="invite-to-community">
                        <div>Invites to community</div>
                        <ul>
                            {invitesToCommunity}
                        </ul>
                      </div>
                    : null
                }
                <div className="communities__list">
                    <div className="title">
                        <button type="button" className="btn btn-success" onClick={() => setShowCreateCommunity((item) => !item)}>Create</button>
                        <div className="content">
                            <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" onClick={async () => await getUserCommunitiesAsync()} />
                            <div>My communities</div>
                            {showMyCommunities
                                ? <FontAwesomeIcon icon={faEye} title="Hide" onClick={() => setShowMyCommunities((item) => !item)} />
                                : <FontAwesomeIcon icon={faEyeSlash} title="Show" onClick={() => setShowMyCommunities((item) => !item)} />
                            }
                        </div>
                    </div>
                    {showMyCommunities
                        ? <ul>
                            {communities}
                          </ul>
                        : null
                    }
                </div>
                {showCreateCommunity &&
                    <CreateCommunity setShowCreateCommunity={setShowCreateCommunity} />
                }
            </div>
        );
    }

    return render();
}

export default MyCommunities;