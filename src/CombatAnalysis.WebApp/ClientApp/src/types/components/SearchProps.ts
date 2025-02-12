import { AppUser } from "../AppUser";

export interface SearchProps {
    me: AppUser;
    t: (key: string) => string;
}