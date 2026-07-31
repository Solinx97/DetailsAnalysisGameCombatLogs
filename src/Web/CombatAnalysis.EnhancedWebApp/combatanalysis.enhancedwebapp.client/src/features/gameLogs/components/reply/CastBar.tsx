interface CastBarProps {
    spell: string | undefined;
    progress: number;
    isSuccess: boolean | undefined;
    isRunCast: boolean;
}

const CastBar: React.FC<CastBarProps> = ({ spell, progress, isSuccess, isRunCast }) => {
    return (
        <div className={`cast-bar ${isRunCast ? (isSuccess ? 'success' : isSuccess === false ? 'failed' : '') : ''}`}>
            {isRunCast &&
                <>
                    <div
                        className="cast-bar-fill"
                        style={{ width: `${progress}%` }}
                    />
                    <span className="cast-bar-text">
                        {spell}
                    </span>
                </>
            }
        </div>
    );
}

export default CastBar;