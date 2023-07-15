import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { NavLink } from 'react-router-dom';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import Alert from '../../Alert';
import useHttpClientAsync from '../../../hooks/useHttpClientAsync';
import MyCommunities from '../myEnvironment/MyCommunities';

import '../../../styles/communication/communities.scss';

const Communities = ({ openCommunity }) => {
    const [showCommunities, setShowCommunities] = useState(true);
    const [communitiesCards, setCommunitiesCards] = useState(<></>);
    const [userCommunitiesId, setUserCommunitiesId] = useState([]);
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(false);

    const [getAllAsync] = useHttpClientAsync();

    const checkAuthentificationAsync = useAuthentificationAsync();

    useEffect(() => {
        let checkAuthentification = async () => {
            await checkAuthentificationAsync();
        }

        let getCommunities = async () => {
            await getCommunitiesAsync();
        }

        checkAuthentification();
        getCommunities();
    }, [])

    useEffect(() => {
        if (userCommunitiesId.length === 0) {
            return;
        }

        let getCommunities = async () => {
            await getCommunitiesAsync();
        }

        getCommunities();
    }, [userCommunitiesId])

    useEffect(() => {
        communitiesCards.key === undefined || communitiesCards.key === null 
            ? setShowUpdatingAlert(false)
            : setShowUpdatingAlert(true);
    }, [communitiesCards])

    const getCommunitiesAsync = async () => {
        const newCommunities = [];

        setShowUpdatingAlert(true);
        const dataState = await getAllAsync('Community');
        if (dataState.statusCode !== 200) {
            return;
        }

        const communities = dataState.data;
        for (let i = 0; i < communities.length; i++) {
            if (userCommunitiesId.indexOf(communities[i].id) === -1) {
                newCommunities.push(communities[i]);
            }
        }

        fillCommunities(newCommunities);
    }

    const fillCommunities = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createCommunityCard(element))
            : (<div className="community__empty">Didn't found any communities</div>);

        setCommunitiesCards(list);
    }

    const createCommunityCard = (community) => {
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

    const render = () => {
        return (<div className="communities">
            <Alert
                isVisible={showUpdatingAlert}
                content="Updating..."
            />
            <MyCommunities
                openCommunity={openCommunity}
                setUserCommunitiesId={setUserCommunitiesId}
            />
            <div className="communities__list">
                <div className="title">
                    <div className="content">
                        <FontAwesomeIcon
                            icon={faArrowsRotate}
                            title="Refresh"
                            onClick={async () => await getCommunitiesAsync()}
                        />
                        <div>Communities</div>
                        {showCommunities
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title="Hide"
                                onClick={() => setShowCommunities((item) => !item)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title="Show"
                                onClick={() => setShowCommunities((item) => !item)}
                            />
                        }
                    </div>
                </div>
                {showCommunities
                    ? <ul>
                        {communitiesCards}
                    </ul>
                    : null
                }
            </div>
        </div>);
    }

    return render();
}

export default Communities;