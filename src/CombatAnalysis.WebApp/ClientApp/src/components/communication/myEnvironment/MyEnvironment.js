import { useState } from 'react';
import Communication from '../Communication';
import Friends from './Friends';
import MyCommunities from './MyCommunities';
import MyFeed from './MyFeed';
import Profile from './Profile';

import '../../../styles/communication/myEnvironment/myEnvironment.scss';

const MyEnvironment = ({ openCommunity }) => {
    const [menuItem, setMenuItem] = useState(8);

    const render = () => {
        return (
            <div className="communication">
                <Communication
                    currentMenuItem={menuItem}
                    setMenuItem={setMenuItem}
                />
                <div className="communication__content my-environment">
                    <div>
                        {menuItem === 8 &&
                            <MyFeed />
                        }
                        {menuItem === 9 &&
                            <Friends />
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