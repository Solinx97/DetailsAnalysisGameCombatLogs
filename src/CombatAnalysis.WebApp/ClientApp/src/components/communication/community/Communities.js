import { useEffect, useState } from "react";
import { NavLink } from 'react-router-dom';
import CreateCommunity from './CreateCommunity';
import { faArrowRightToBracket, faCircleInfo } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import '../../../styles/communication/communities.scss';

const Communities = () => {
    const [showCreateCommunity, setShowCreateCommunity] = useState(false);
    const [communities, setCommunities] = useState(<></>);
    const [myCommunities, setMyCommunities] = useState(<></>);

    useEffect(() => {
        let getCommunities = async () => {
            await getCommunitiesAsync();
            await getMyCommunitiesAsync();
        }

        getCommunities();
    }, [])

    useEffect(() => {
        let getCommunities = async () => {
            await getCommunitiesAsync();
            await getMyCommunitiesAsync();
        }

        getCommunities();
    }, [showCreateCommunity])

    const getCommunitiesAsync = async () => {
        const response = await fetch('api/v1/Community');

        if (response.status === 200) {
            const myCommunities = await response.json();
            fillCommunities(myCommunities);
        }
    }

    const getMyCommunitiesAsync = async () => {
        const response = await fetch('api/v1/Community');

        if (response.status === 200) {
            const myCommunities = await response.json();
            fillMyCommunities(myCommunities);
        }
    }

    const fillCommunities = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createCommunityCard(element))
            : (<div className="community__empty">You don't have any communities</div>);

        setCommunities(list);
    }

    const fillMyCommunities = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createMyCommunityCard(element))
            : (<div className="community__empty">You don't have any communities</div>);

        setMyCommunities(list);
    }

    const createCommunityCard = (element) => {
        return (<li key={element.id} className="community">
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.name}</h5>
                    <p className="card-text">{element.description}</p>
                    <NavLink className="card-link">Open</NavLink>
                    <NavLink className="card-link">Enter</NavLink>
                    <NavLink className="card-link">More details</NavLink>
                </div>
            </div>
        </li>);
    }

    const createMyCommunityCard = (element) => {
        return (<li key={element.id} className="my-community">
            <div>
                {element.name}
            </div>
            <div>
                <FontAwesomeIcon icon={faArrowRightToBracket} title="Open" />
                <FontAwesomeIcon icon={faCircleInfo} title="Info" />
            </div>
        </li>);
    }

    const render = () => {
        return (<div className="communities">
            <div className="communities__my-list">
                <div className="create">
                    <button type="button" className="btn btn-success" onClick={() => setShowCreateCommunity(!showCreateCommunity)}>Create</button>
                </div>
                <div className="list">
                    <p>My communities</p>
                    <ul>
                        {myCommunities}
                    </ul>
                </div>
            </div>
            <div className="communities__list">
                <div className="title">
                    <p>Communities</p>
                </div>
                <ul>
                    {communities} 
                </ul>
            </div>
            {showCreateCommunity &&
                <CreateCommunity setShowCreateCommunity={setShowCreateCommunity} />
            }
        </div>);
    }

    return render();
}

export default Communities;