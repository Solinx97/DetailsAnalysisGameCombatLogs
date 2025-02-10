import { SetStateAction } from "react";
import { CommunityUser } from "../../../CommunityUser";

export interface CommunityUsersProps {
    me: any;
    communityUsers: CommunityUser[];
    removeUsersAsync: (peopleToRemove: CommunityUser[]) => Promise<void>;
    setShowMembers: (value: SetStateAction<boolean>) => void;
    isPopup: boolean;
    canRemovePeople: () => boolean;
}