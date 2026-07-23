interface InstantCastProps {
    spell: string | undefined;
    isRunCast: boolean;
}

const InstantCast: React.FC<InstantCastProps> = ({ spell, isRunCast }) => {
    return (
        <div className="instant-cast">
            {isRunCast ? spell : ''}
        </div>
    );
}

export default InstantCast;