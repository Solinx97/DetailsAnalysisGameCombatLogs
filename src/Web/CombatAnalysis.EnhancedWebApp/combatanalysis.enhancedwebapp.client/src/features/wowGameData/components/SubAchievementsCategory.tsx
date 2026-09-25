import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetAchievementsByCategoryQuery } from '../api/WoWCharacter.api';
import { useContext, useEffect, useState } from 'react';
import type { AchievementExtendModel } from '../types/achievements/AchievementExtendModel';
import SelectedAchievemnt from './SelectedAchievemnt';
import WoWGameDataContext from '@/context/WoWGameDataContext';

interface SubAchievementsCategoryProps {
    categoryId: number;
    onlyNotCompleted: boolean;
}

const SubAchievementsCategory: React.FC<SubAchievementsCategoryProps> = ({ categoryId, onlyNotCompleted }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;
    
    const [selectedCategoryId, setSelectedCategoryId] = useState<number>(0);
    const [selectedAchievementId, setSelectedAchievementId] = useState<number>(0);
    const [achievemntCount, setAchievemntCount] = useState<number>(0);
    const [achievemntCompletedCount, setAchievemntCompletedCount] = useState<number>(0);
    const [achievements, setAchievements] = useState<AchievementExtendModel[]>([]);

    const { data: categoryAchievements, isLoading } = useGetAchievementsByCategoryQuery({ username, serverName, categoryId, regionName });

    useEffect(() => {
        if (!categoryAchievements) {
            return;
        }

        setAchievements(categoryAchievements.achievements);
        setAchievemntCount(categoryAchievements.achievements.length);
        setAchievemntCompletedCount(categoryAchievements.achievements.filter(x => x.completedTime !== null).length);
    }, [categoryAchievements]);

    useEffect(() => {
        if (!categoryAchievements) {
            return;
        }

        if (onlyNotCompleted) {
            const onlyNotCompletedAchievements = categoryAchievements.achievements.filter(x => x.completedTime === null);
            setAchievements(onlyNotCompletedAchievements);
        }
        else {
            setAchievements(categoryAchievements.achievements);
        }

    }, [categoryAchievements, onlyNotCompleted]);

    if (!categoryAchievements || isLoading) {
        return (<div>Loading...</div>);
    }

    const getDate = (date: string) => {
        const parse = new Date(date);

        return parse.toLocaleString('ru-RU', {
            dateStyle: 'short',
            timeStyle: 'short'
        });
    }

    return (
        <div className="achievements">
            <ul className="achievements__container">
                {categoryAchievements.subcategories.map((category, index) => (
                    <li key={index} className="achievements__item">
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
            {selectedCategoryId === 0 &&
                <div>
                    <div>{t("Count")}: {achievemntCompletedCount}/{achievemntCount}</div>
                    <ul className="selected-achievements__container">
                        {achievements.map((achiev, index) => (
                            <li key={index} className="selected-achievements__item">
                                <div className={`selected-achievement btn-shadow ${achiev.completedTime ? 'completed' : 'not-completed'}`}
                                    onClick={() => setSelectedAchievementId(prev => prev === 0 ? achiev.id : 0)}>
                                    <div>{achiev.name}</div>
                                    {selectedAchievementId === achiev.id &&
                                        <SelectedAchievemnt
                                            achievementId={achiev.id}
                                            t={t}
                                        />
                                    }
                                </div>
                                <div className="when">{achiev.completedTime ? getDate(achiev.completedTime) : ""}</div>
                            </li>
                        ))
                        }
                    </ul>
                </div>

            }
        </div>
    );
}

export default SubAchievementsCategory;