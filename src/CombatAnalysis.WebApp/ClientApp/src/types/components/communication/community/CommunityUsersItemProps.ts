import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { CommunityUser } from "../../../CommunityUser";

export interface CommunityUsersItemProps {
    me: AppUser;
    communityUser: CommunityUser;
    usersToRemove: CommunityUser[];
    setUsersToRemove: (value: SetStateAction<CommunityUser[]>) => void;
    showRemoveUser: boolean;
}