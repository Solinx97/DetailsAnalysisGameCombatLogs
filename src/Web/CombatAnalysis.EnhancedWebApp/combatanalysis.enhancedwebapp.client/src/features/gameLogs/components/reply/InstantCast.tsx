interface InstantCastProps {
    spell: string;
}

const InstantCast: React.FC<InstantCastProps> = ({ spell }) => {
    return (
        <div className="instant-cast">
            {spell}
        </div>
    );
}

export default InstantCast;