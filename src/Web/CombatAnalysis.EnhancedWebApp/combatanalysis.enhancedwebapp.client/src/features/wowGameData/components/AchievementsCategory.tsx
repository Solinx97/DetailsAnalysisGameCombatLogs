import WoWGameDataContext from '@/context/WoWGameDataContext';
import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useLazyGetAchievementAllCategoryQuery } from '../api/WoWCharacter.api';
import type { AchievementCategoriesModel } from '../types/achievements/AchievementCategoriesModel';
import SubAchievementsCategory from './SubAchievementsCategory';

const AchievementsCategory: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [achievementAllCategory, setAchievementAllCategory] = useState<AchievementCategoriesModel>();
    const [selectedCategoryId, setSelectedCategoryId] = useState<number>(0);
    const [onlyNotCompleted, setOnlyNotCompleted] = useState<boolean>(false);

    const [getAchievements] = useLazyGetAchievementAllCategoryQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedAchievementAllCategory = await getAchievements({ username, serverName, regionName }).unwrap();
                setAchievementAllCategory(receivedAchievementAllCategory);
            } catch (error) {
                console.error("Failed to fetch character mounts:", error);
            }
        }

        loadAsync();
    }, []);

    if (!username || username.trim().length === 0) {
        return (<div>No data</div>);
    }


    if (!achievementAllCategory) {
        return (<div>Loading...</div>);
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