import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import { useGetAchievementsStatisticsQuery } from '../../api/WoWCharacter.api';
import AchievementsStatisticsCategory from './AchievementsStatisticsCategory';

import './Achievements.scss';

const AchievementsStatistics: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { username, serverName, regionName } = context;

    const [selectedCategoryId, setSelectedCategoryId] = useState<number>(-1);

    const { data: achievementsStatistics, isLoading, error } = useGetAchievementsStatisticsQuery({ username, serverName, regionName });

    if (!achievementsStatistics || isLoading || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!achievementsStatistics || isLoading}
        />);
    }

    return (
        <div className="achievements-statistics-category">
            <ul className="achievements-statistics-category__container">
                {achievementsStatistics.map((statistic, index) => (
                    <li key={index} className="achievements-statistics-category__item">
                        <div className="achievements-statistics-category__name">
                            <div className={`btn-shadow ${selectedCategoryId === statistic.id ? 'selected' : ''}`}
                                onClick={() => setSelectedCategoryId(prev => prev === statistic.id ? -1 : statistic.id)}>
                                <FontAwesomeIcon
                                    icon={selectedCategoryId === statistic.id ? faLocationCrosshairs : faPlus}
                                />
                                <div>{statistic.name}</div>
                            </div>
                        </div>
                        {selectedCategoryId === statistic.id &&
                            <AchievementsStatisticsCategory
                                subCategories={statistic.subCategories}
                                statistics={statistic.statistics}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default AchievementsStatistics;