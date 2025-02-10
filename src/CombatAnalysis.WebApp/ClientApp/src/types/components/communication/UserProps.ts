export interface UserProps {
    targetUserId: string;
    setUserInformation: (information: any | null) => void;
    allowRemoveFriend: boolean;
    actionAfterRequests: any | null;
    friendId: number | null;
}