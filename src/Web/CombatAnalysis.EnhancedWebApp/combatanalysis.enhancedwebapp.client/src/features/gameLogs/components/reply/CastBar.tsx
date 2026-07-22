interface CastBarProps {
    spell: string;
    progress: number;
}

const CastBar: React.FC<CastBarProps> = ({ spell, progress }) => {
    return (
        <div className="cast-bar">
            <div
                className="cast-bar-fill"
                style={{ width: `${progress}%` }}
            />
            <span className="cast-bar-text">
                {spell}
            </span>
        </div>
    );
}

export default CastBar;