import { CommunityPost } from '../../../CommunityPost';

export interface CommunityPostProps {
    userId: string;
    communityId: number;
    post: CommunityPost;
}