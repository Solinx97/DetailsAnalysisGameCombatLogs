import type { CommunityDiscussionModel } from './CommunityDiscussionModel';

export interface AllDiscussionModel {
    discussions: CommunityDiscussionModel[];
    count: number;
}