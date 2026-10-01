import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import type { CharacterAchievementStatisticModel } from '../../types/achievements/CharacterAchievementStatisticModel';
import type { CharacterAchievementStatisticsSubCategoryModel } from '../../types/achievements/CharacterAchievementStatisticsSubCategoryModel';

const AchievementsStatisticsCategory: React.FC<{ 
    statistics: CharacterAchievementStatisticModel[], 
    subCategories?: CharacterAchievementStatisticsSubCategoryModel[] 
}> = ({ statistics, subCategories }) => {
    const [selectedCategoryId, setSelectedCategoryId] = useState<number>(-1);

    return (
        <div className="achievements-statistics-category">
            {subCategories &&
                <>
                    <ul className="achievements-statistics-category__container">
                        {subCategories.map((category, index) => (
                            <li key={index} className="achievements-statistics-category__item">
                                <div className="achievements-statistics-category__name">
                                    <div className={`btn-shadow ${selectedCategoryId === category.id ? 'selected' : ''}`}
                                        onClick={() => setSelectedCategoryId(prev => prev === category.id ? -1 : category.id)}>
                                        <FontAwesomeIcon
                                            icon={selectedCategoryId === category.id ? faLocationCrosshairs : faPlus}
                                        />
                                        <div>{category.name}</div>
                                    </div>
                                </div>
                                {selectedCategoryId === category.id &&
                                    <AchievementsStatisticsCategory
                                        statistics={category.statistics}
                                    />
                                }
                            </li>
                        ))
                        }
                    </ul>
                </>
            }
            <ul className="achievements-statistics-category__container">
                {statistics.map((statistic, index) => (
                    <li key={index} className="achievements-statistics-category__item statistics">
                        <div>{statistic.name}</div>
                        <div>{statistic.quantity}</div>
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default AchievementsStatisticsCategory;