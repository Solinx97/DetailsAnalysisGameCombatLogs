import { useState } from 'react';
import { useLazyGetCustomersQuery } from '../store/api/UserApi';
import { useTranslation } from 'react-i18next';
import { useRef } from 'react';
import PeopleItem from './communication/people/PeopleItem';

const Search = ({ me }) => {
    const { t, i18n } = useTranslation("translate");

    const [loadingPeople] = useLazyGetCustomersQuery();
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
            <input type="text" className="form-control" placeholder={t("Search")} id="inputUsername" ref={searchText}
                onChange={searchTextHandle}
                onClick={async () => await loadingPeopleAsync()}
            />
            <div className={`search__content${showSearch ? "_active" : ""}`}>
                <div>People</div>
                <div className="empty">Empty</div>
                <ul className="people__cards">
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
                <div className="close" onClick={() => setShowSearch(false)}>Close</div>
            </div>
        </div>
    );
}

export default Search;