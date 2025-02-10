import { SetStateAction } from "react";
import { AppUser } from "../../AppUser";

export interface UserProps {
    me: AppUser;
    targetUserId: string;
    setUserInformation: (value: SetStateAction<any>) => void;
    friendId?: number | 0;
}