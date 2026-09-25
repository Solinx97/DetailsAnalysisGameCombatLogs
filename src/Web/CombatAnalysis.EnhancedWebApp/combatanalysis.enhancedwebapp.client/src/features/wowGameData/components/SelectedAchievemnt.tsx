import { useGetAchievementQuery } from '../api/WoWData.api';

interface SelectedAchievemntProps {
    achievementId: number;
    t: (key: string) => string;
}

const SelectedAchievemnt: React.FC<SelectedAchievemntProps> = ({ achievementId, t }) => {
    const { data: achievement, isLoading } = useGetAchievementQuery({ achievementId, regionName: "eu" });

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