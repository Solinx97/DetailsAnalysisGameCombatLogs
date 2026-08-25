import type { CommunityUserModel } from './CommunityUserModel';

export interface AllCommunityUserModel {
    users: CommunityUserModel[];
    count: number;
}