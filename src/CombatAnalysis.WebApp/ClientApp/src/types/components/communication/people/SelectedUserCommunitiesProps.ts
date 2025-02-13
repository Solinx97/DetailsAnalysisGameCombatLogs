import { AppUser } from '../../../AppUser';

export interface SelectedUserCommunitiesProps {
    user: AppUser | null;
    t: (key: string) => string;
}