import { faArrowsRotate, faCircleCheck, faCircleXmark, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import { NavLink } from 'react-router-dom';
import CreateCommunity from './CreateCommunity';

import '../../../styles/communication/communities.scss';

const MyCommunities = ({ openCommunity, setMyCommunitiesId = () => { return; } }) => {
    const customer = useSelector((state) => state.customer.value);

    const [showCreateCommunity, setShowCreateCommunity] = useState(false);
    const [showMyCommunities, setShowMyCommunities] = useState(true);
    const [myCommunities, setMyCommunities] = useState(<></>);
    const [invitesToCommunity, setInvitesToCommunity] = useState(<></>);

    useEffect(() => {
        let getMyCommunities = async () => {
            await getInvitesToCommunityAsync();
            await getUserCommunitiesAsync();
        }

        getMyCommunities();
    }, [])

    const getInvitesToCommunityAsync = async () => {
        const response = await fetch(`api/v1/InviteToCommunity/searchByUserId/${customer.id}`);

        if (response.status !== 200) {
            return;
        }

        const invitesToCommunity = await response.json();
        for (let i = 0; i < invitesToCommunity.length; i++) {
            const customer = await getCustomerByIdAsync(invitesToCommunity[i].ownerId);
            invitesToCommunity[i].username = customer.username;

            const comunity = await getCommunityByIdAsync(invitesToCommunity[i].communityId);
            invitesToCommunity[i].community = comunity.name;
        }

        fillInvitesToCommunity(invitesToCommunity);
    }

    const getCustomerByIdAsync = async (customerId) => {
        const response = await fetch(`api/v1/Customer/${customerId}`);

        if (response.status !== 200) {
            return null;
        }

        const customer = await response.json();
        return customer;
    }

    const getCommunityByIdAsync = async (communityId) => {
        const response = await fetch(`api/v1/Community/${communityId}`);

        if (response.status !== 200) {
            return null;
        }

        const community = await response.json();
        return community;
    }

    const getUserCommunitiesAsync = async () => {
        const myCommunities = [];
        const myCommunitiesId = [];

        const response = await fetch(`api/v1/CommunityUser/searchByUserId/${customer.id}`);
        if (response.status !== 200) {
            return;
        }

        const userCommunities = await response.json();
        for (var i = 0; i < userCommunities.length; i++) {
            const myCommunity = await getMyCommunityAsync(userCommunities[i].communityId);
            myCommunities.push(myCommunity);
            myCommunitiesId.push(userCommunities[i].communityId);
        }

        setMyCommunitiesId(myCommunitiesId);
        fillMyCommunities(myCommunities);
    }

    const getMyCommunityAsync = async (communityId) => {
        const response = await fetch(`api/v1/Community/${communityId}`);

        if (response.status === 200) {
            const myCommunity = await response.json();
            return myCommunity;
        }

        return [];
    }

    const fillMyCommunities = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createMyCommunityCard(element))
            : (<div className="community__empty">You don't have any communities</div>);

        setMyCommunities(list);
    }

    const fillInvitesToCommunity = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createInviteToCommunityCard(element))
            : (<div className="community__empty">You don't have any invites</div>);

        setInvitesToCommunity(list);
    }

    const createMyCommunityCard = (community) => {
        return (<li key={community.id} className="community">
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{community.name}</h5>
                    <p className="card-text">{community.description}</p>
                    <NavLink className="card-link" onClick={() => openCommunity(community)}>Open</NavLink>
                    <NavLink className="card-link">More details</NavLink>
                </div>
            </div>
        </li>);
    }

    const acceptRequestAsync = async (inviteToCommunity) => {
        const newCommunityUser = {
            id: 0,
            communityId: inviteToCommunity.communityId,
            customerId: customer.id
        };

        let response = await fetch("/api/v1/CommunityUser", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newCommunityUser)
        });

        if (response.status !== 200) {
            return;
        }

        response = await fetch(`/api/v1/InviteToCommunity/${inviteToCommunity.id}`, {
            method: 'DELETE',
        });

        if (response.status !== 200) {
            return;
        }

        await getInvitesToCommunityAsync();
    }

    const rejectRequestAsync = async (inviteToCommunity) => {
        const response = await fetch(`/api/v1/InviteToCommunity/${inviteToCommunity.id}`, {
            method: 'DELETE',
        });

        if (response.status !== 200) {
            return;
        }

        await getInvitesToCommunityAsync();
    }

    const createInviteToCommunityCard = (inviteToCommunity) => {
        return (<li key={inviteToCommunity.id} className="request-to-connect">
            <div>'{inviteToCommunity.username}' sent you invite to '{inviteToCommunity.community}'</div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title="Accept"
                    onClick={async () => await acceptRequestAsync(inviteToCommunity)} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title="Reject"
                    onClick={async () => await rejectRequestAsync(inviteToCommunity)} /></div>
            </div>
        </li>);
    }

    const handleCreateCommunityVisibility = () => {
        setShowCreateCommunity(!showCreateCommunity);
    }

    const handleCommunityVisibility = () => {
        setShowMyCommunities(!showMyCommunities);
    }

    const render = () => {
        return (<div>
            <div>
                <div>Invites to community</div>
                <ul>
                    {invitesToCommunity}
                </ul>
            </div>
            <div className="communities__list">
                <div className="title">
                    <button type="button" className="btn btn-success" onClick={handleCreateCommunityVisibility}>Create</button>
                    <div className="content">
                        <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" onClick={async () => await getUserCommunitiesAsync()} />
                        <div>My communities</div>
                        {showMyCommunities
                            ? <FontAwesomeIcon icon={faEye} title="Hide" onClick={handleCommunityVisibility} />
                            : <FontAwesomeIcon icon={faEyeSlash} title="Show" onClick={handleCommunityVisibility} />
                        }
                    </div>
                </div>
                {showMyCommunities
                    ? <ul>
                        {myCommunities}
                    </ul>
                    : null
                }
            </div>
            {showCreateCommunity &&
                <CreateCommunity setShowCreateCommunity={setShowCreateCommunity} />
            }
        </div>);
    }

    return render();
}

export default MyCommunities;