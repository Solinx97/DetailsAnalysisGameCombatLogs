import { AppUser } from '../../../AppUser';

export interface CreateUserPostProps {
    user: AppUser;
    owner: string;
    t: (key: string) => string;
}