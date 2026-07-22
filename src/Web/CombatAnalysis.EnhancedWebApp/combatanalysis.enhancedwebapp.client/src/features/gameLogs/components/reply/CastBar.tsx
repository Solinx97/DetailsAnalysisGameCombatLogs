interface CastBarProps {
    spell: string;
    progress: number;
    isSuccess: boolean;
}

const CastBar: React.FC<CastBarProps> = ({ spell, progress, isSuccess }) => {
    return (
        <div className={`cast-bar ${isSuccess ? 'success' : 'failed'}`}>
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