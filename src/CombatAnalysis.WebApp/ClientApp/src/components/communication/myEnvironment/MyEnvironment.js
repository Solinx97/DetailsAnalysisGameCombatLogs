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

    const customer = useSelector((state) => state.customer.value);

    const render = () => {
        return (
            <div className="communication">
                <CommunicationMenu
                    currentMenuItem={menuItem}
                    setMenuItem={setMenuItem}
                />
                <div className="communication__content my-environment">
                    <div>
                        {menuItem === 8 &&
                            <MyFeed />
                        }
                        {menuItem === 9 &&
                            <Friends
                                customer={customer}
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
                    </div>
                </div>
            </div>
        );
    }

    return render();
}

export default MyEnvironment;