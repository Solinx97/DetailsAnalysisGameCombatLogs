import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { Community } from "../../../Community";

export interface CommunityMembersProps {
    community: Community;
    user: AppUser;
    setIsCommunityMember: (value: SetStateAction<boolean>) => void;
}