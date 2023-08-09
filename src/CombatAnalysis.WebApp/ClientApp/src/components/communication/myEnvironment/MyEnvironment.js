import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import Communication from '../Communication';
import Friends from './Friends';
import MyCommunities from './MyCommunities';
import MyFeed from './MyFeed';
import Profile from './Profile';

import '../../../styles/communication/myEnvironment.scss';

const MyEnvironment = ({ openCommunity }) => {
    const { t } = useTranslation("communication/myEnvironment/myEnvironment");

    const [currentMenuItem, setCurrentMenuItem] = useState(0);

    const render = () => {
        return (
            <div className="communication">
                <Communication
                    currentMenuItem={5}
                />
                <div className="communication__content my-environment">
                    <ul className="my-environment__menu">
                        <li>
                            <button type="button" className="btn btn-outline-success" onClick={() => setCurrentMenuItem(1)}>{t("Profile")}</button>
                        </li>
                        <li>
                            <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(0)}>{t("Feed")}</button>
                            <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(2)}>{t("Friends")}</button>
                            <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(3)}>{t("Communities")}</button>
                        </li>
                        <li>
                            <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(6)}>{t("Recomendations")}</button>
                        </li>
                    </ul>
                    <div>
                        {currentMenuItem === 0 && <MyFeed />}
                        {currentMenuItem === 1 && <Profile />}
                        {currentMenuItem === 2 && <Friends />}
                        {currentMenuItem === 3 && <MyCommunities openCommunity={openCommunity} />}
                    </div>
                </div>
            </div>
        );
    }

    return render();
}

export default MyEnvironment;