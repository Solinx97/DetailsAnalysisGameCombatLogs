import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";

export interface GroupChatAddUserProps {
    me: AppUser;
    chatId: number;
    groupChatUsersId: string[];
    setShowAddPeople: (value: SetStateAction<boolean>) => void;
    t: (key: string) => string;
}