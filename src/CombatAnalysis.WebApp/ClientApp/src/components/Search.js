import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetUsersQuery } from '../store/api/UserApi';
import PeopleItem from './communication/people/PeopleItem';

const Search = ({ me }) => {
    const { t } = useTranslation("translate");

    const [loadingPeople] = useLazyGetUsersQuery();
    const [people, setPeople] = useState([]);
    const [filteredPeople, setFilteredPeople] = useState([]);
    const [showSearch, setShowSearch] = useState(false);

    const searchText = useRef(null);

    const loadingPeopleAsync = async () => {
        const peopleData = await loadingPeople();

        if (peopleData.data !== undefined) {
            setPeople(peopleData.data);
            setShowSearch(true);
        }
    }

    const searchTextHandle = (e) => {
        if (searchText.current.value.length === 0) {
            setFilteredPeople([]);
            return;
        }

        const filteredPeople = people.filter((item) => item.username.toLowerCase().startsWith(e.target.value.toLowerCase()));
        setFilteredPeople(filteredPeople);
    }

    const cleanSearch = () => {
        searchText.current.value = "";

        setFilteredPeople([]);
        setShowSearch(false);
    }

    return (
        <div className="search">
            <input type="text" className="form-control" placeholder={t("Search")} id="inputUsername" autoComplete="off" ref={searchText}
                onChange={searchTextHandle}
                onClick={async () => await loadingPeopleAsync()}
            />
            <div className={`search__content${showSearch ? "_active" : ""}`}>
                <div>{t("People")}</div>
                <div className="container">
                    {filteredPeople.length === 0
                        ? <div className="empty">{t("Empty")}</div>
                        : <ul className="people__cards">
                            {
                                filteredPeople?.map((user) => (
                                    <li key={user.id}>
                                        <PeopleItem
                                            me={me}
                                            people={user}
                                            actionAfterRequests={cleanSearch}
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