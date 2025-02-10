import { SetStateAction } from "react";
import { AppUser } from "../AppUser";

export interface AddPeopleProps {
    user: AppUser;
    communityUsersId: string[];
    peopleToJoin: AppUser[];
    setPeopleToJoin: (value: SetStateAction<AppUser[]>) => void;
}