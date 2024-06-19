import "../../styles/inDevNotes/inDev.scss";

const InDev = ({ inDevItem }) => {
    return (
        <div className="in-dev">
            <div className="in-dev__content">{inDevItem}</div>
            <div className="in-dev__title">In dev</div>
        </div>
    );
}

export default InDev;