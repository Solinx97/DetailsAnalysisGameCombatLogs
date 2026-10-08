export type CreateGroupChatModel = {
    name: string;
    ownerUsername: string;
    invitePeopleRule: number;
    removePeopleRule: number;
    pinMessageRule: number;
    announcementsRule: number;
    ownerId: string;
}