import CommunicationMenu from "../CommunicationMenu";
import Communities from "./Communities";

const AllCommunities: React.FC = () => {
    return (
        <>
            <Communities />
            <CommunicationMenu
                currentMenuItem={2}
                hasSubMenu={false}
            />
        </>
    );
}

export default AllCommunities;