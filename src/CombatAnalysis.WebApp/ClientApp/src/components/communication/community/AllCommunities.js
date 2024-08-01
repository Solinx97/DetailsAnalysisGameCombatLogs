import CommunicationMenu from "../CommunicationMenu";
import MyCommunities from "../myEnvironment/MyCommunities";
import Communities from "./Communities";

const AllCommunities = () => {
    return (
        <div className="communication">
            <CommunicationMenu
                currentMenuItem={3}
            />
            <div className="communication-content communities">
                <MyCommunities />
                <Communities />
            </div>
        </div>
    );
}

export default AllCommunities;