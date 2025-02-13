export interface InviteToCommunity {
    id: number;
    communityId: number;
    toAppUserId: string;
    when: string;
    appUserId: string;
}