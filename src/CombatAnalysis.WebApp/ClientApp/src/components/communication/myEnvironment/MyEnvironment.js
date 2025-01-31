import { useState } from 'react';
import { useSelector } from 'react-redux';
import CommunicationMenu from '../CommunicationMenu';
import Friends from './Friends';
import MyCommunities from './MyCommunities';
import MyFeed from './MyFeed';
import Profile from './Profile';
import RequestsToConnect from './RequestsToConnect';

import '../../../styles/communication/myEnvironment/myEnvironment.scss';

const MyEnvironment = ({ openCommunity }) => {
    const [menuItem, setMenuItem] = useState(8);

    const user = useSelector((state) => state.user.value);

    const render = () => {
        return (
            <div className="communication">
                <CommunicationMenu
                    currentMenuItem={menuItem}
                    setMenuItem={setMenuItem}
                />
                <div className="communication-content my-environment">
                    <>
                        {menuItem === 8 &&
                            <MyFeed />
                        }
                        {menuItem === 9 &&
                            <Friends
                                user={user}
                                requestsToConnect={<RequestsToConnect />}
                                allowRemoveFriend={true}
                            />
                        }
                        {menuItem === 10 &&
                            <MyCommunities
                                openCommunity={openCommunity}
                            />
                        }
                        {menuItem === 12 &&
                            <Profile />
                        }
                    </>
                </div>
            </div>
        );
    }

    return render();
}

export default MyEnvironment;