import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import { useGetAchievementAllCategoryQuery } from '../../api/WoWCharacter.api';
import SubAchievementsCategory from './SubAchievementsCategory';

import './Achievements.scss';

const AchievementsCategory: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [selectedCategoryId, setSelectedCategoryId] = useState<number>(0);
    const [onlyNotCompleted, setOnlyNotCompleted] = useState<boolean>(false);

    const { data: achievementAllCategory, isLoading, error } = useGetAchievementAllCategoryQuery({ username, serverName, regionName });

    if (!achievementAllCategory || isLoading || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!achievementAllCategory || isLoading}
        />);
    }
    return (
        <div className="achievements-category">
            <div className="achievements-category__title">
                <h6>{t("Achievements")}</h6>
                <h6 className="count">{achievementAllCategory.totalQuantity}</h6>
            </div>
            <div className="form-check">
                <input className="form-check-input" type="checkbox" value="" id="checkIndeterminate"
                    defaultChecked={onlyNotCompleted}
                    onChange={() => setOnlyNotCompleted(prev => !prev)} />
                <label className="form-check-label" htmlFor="checkIndeterminate">
                    {t("OnlyNotCompleted")}
                </label>
            </div>
            <ul className="achievements-category__container">
                {achievementAllCategory.rootCategories.map((category, index) => (
                    <li key={index} className="achievements-category__item">
                        <div className="achievements-category__name">
                            <div className={`btn-shadow ${selectedCategoryId === category.id ? 'selected' : ''}`}
                                onClick={() => setSelectedCategoryId(prev => prev === 0 ? category.id : 0)}>
                                <FontAwesomeIcon
                                    icon={selectedCategoryId === category.id ? faLocationCrosshairs : faPlus}
                                />
                                <div>{category.name} [{category.quantity}]</div>
                            </div>
                        </div>
                        {selectedCategoryId === category.id &&
                            <SubAchievementsCategory
                                categoryId={category.id}
                                onlyNotCompleted={onlyNotCompleted}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default AchievementsCategory;