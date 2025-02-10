import { SetStateAction } from "react";
import { GroupChatUser } from "../../../GroupChatUser";

export interface GroupChatMembersProps {
    me: any;
    groupChatUsers: GroupChatUser[];
    removeUsersAsync: (peopleToRemove: GroupChatUser[]) => Promise<void>;
    setShowMembers: (value: SetStateAction<boolean>) => void;
    isPopup: boolean;
    canRemovePeople: () => boolean;
}