import { useRef, useState } from 'react';
import { useLazyGetUsersQuery } from '../store/api/core/User.api';
import { AppUser } from '../types/AppUser';
import { SearchProps } from '../types/components/SearchProps';
import PeopleItem from './communication/people/PeopleItem';

const Search: React.FC<SearchProps> = ({ me, t }) => {
    const [loadingPeople] = useLazyGetUsersQuery();
    const [people, setPeople] = useState<AppUser[]>([]);
    const [filteredPeople, setFilteredPeople] = useState<AppUser[]>([]);
    const [showSearch, setShowSearch] = useState(false);

    const searchText = useRef<any>(null);

    const loadingPeopleAsync = async () => {
        const peopleData = await loadingPeople();

        if (peopleData.data !== undefined) {
            setPeople(peopleData.data);
            setShowSearch(true);
        }
    }

    const searchTextHandle = (e: any) => {
        if (searchText.current.value.length === 0) {
            setFilteredPeople([]);
            return;
        }

        const filteredPeople = people.filter((item) => item.username.toLowerCase().startsWith(e.target.value.toLowerCase()));
        setFilteredPeople(filteredPeople);
    }

    return (
        <div className="search">
            <div className="search__container">
                <input type="text" className="form-control" placeholder={t("UsersSearch")} id="inputUsername" autoComplete="off" ref={searchText}
                    onChange={searchTextHandle}
                    onClick={loadingPeopleAsync}
                />
            </div>
            <div className={`search__content${showSearch ? "_active" : ""}`}>
                <div>{t("Users")}</div>
                <div className="container">
                    {filteredPeople.length === 0
                        ? <div className="empty">{t("Empty")}</div>
                        : <ul className="people__cards">
                            {filteredPeople?.map((user) => (
                                    <li key={user.id}>
                                        <PeopleItem
                                            me={me}
                                            targetUser={user}
                                        />
                                    </li>
                                ))
                            }
                        </ul>
                    }
                </div>
                <div className="close" onClick={() => setShowSearch(false)}>{t("Close")}</div>
            </div>
        </div>
    );
}

export default Search;