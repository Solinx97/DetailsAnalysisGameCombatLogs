import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";

export interface PeopleInvitesToCommunityProps {
    me: AppUser;
    targetUser: AppUser;
    setOpenInviteToCommunity: (value: SetStateAction<boolean>) => void;
}