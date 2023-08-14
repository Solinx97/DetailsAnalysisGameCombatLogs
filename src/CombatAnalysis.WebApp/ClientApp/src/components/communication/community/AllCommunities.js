import Communication from "../Communication";
import MyCommunities from "../myEnvironment/MyCommunities";
import Communities from "./Communities";

const AllCommunities = () => {
    return (
        <div className="communication">
            <Communication
                currentMenuItem={3}
            />
            <div className="communication__content communities">
                <MyCommunities />
                <Communities />
            </div>
        </div>
    );
}

export default AllCommunities;