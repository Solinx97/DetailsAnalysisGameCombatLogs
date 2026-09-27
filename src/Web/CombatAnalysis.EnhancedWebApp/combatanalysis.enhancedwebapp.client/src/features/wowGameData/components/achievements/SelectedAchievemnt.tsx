import WoWGameDataContext from '@/context/WoWGameDataContext';
import { useContext } from 'react';
import { useGetAchievementQuery } from '../../api/WoWData.api';

interface SelectedAchievemntProps {
    achievementId: number;
    t: (key: string) => string;
}

const SelectedAchievemnt: React.FC<SelectedAchievemntProps> = ({ achievementId }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { regionName } = context;

    const { data: achievement, isLoading } = useGetAchievementQuery({ achievementId, regionName });

    if (!achievement || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="description">
            <div>{achievement.description}</div>
        </div>
    );
}

export default SelectedAchievemnt;