import { SetStateAction } from "react";

export interface TargetCommunityProps {
    communityId: number;
    communityIdToInvite: number[];
    setCommunityIdToInvite: (value: SetStateAction<number[]>) => void;
}