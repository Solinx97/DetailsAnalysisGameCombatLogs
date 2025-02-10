import { SetStateAction } from "react";

export interface ChatRulesItemProps {
    setInvitePeople: (value: SetStateAction<number>) => void;
    setRemovePeople: (value: SetStateAction<number>) => void;
    setPinMessage: (value: SetStateAction<number>) => void;
    setAnnouncements: (value: SetStateAction<number>) => void;
    payload: any;
    t: (key: string) => string;
}