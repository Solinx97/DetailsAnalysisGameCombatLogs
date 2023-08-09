import MyCommunities from "../myEnvironment/MyCommunities";
import Communities from "./Communities";

const AllCommunities = () => {
    return (
        <div className="communities">
            <MyCommunities />
            <Communities />
        </div>
    );
}

export default AllCommunities;