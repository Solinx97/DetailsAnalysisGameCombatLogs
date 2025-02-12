import { AppUser } from "../AppUser";

export interface AddFriendItemProps {
    friendUserId: string;
    addUserIdToList: (user: AppUser) => void;
    removeUserIdToList: (user: AppUser) => void;
    filterContent: string;
    peopleIdToJoin: AppUser[];
}