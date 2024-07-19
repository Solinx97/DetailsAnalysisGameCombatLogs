import { useCallback, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetUsersQuery } from '../../../store/api/UserApi';
import Loading from "../../Loading";
import CommunicationMenu from '../CommunicationMenu';
import PeopleItem from './PeopleItem';

import '../../../styles/communication/people/people.scss';

const peopleInterval = 3000;

const People = () => {
    const { t } = useTranslation("communication/people/people");

    const me = useSelector((state) => state.user.value);
    const [menuItem, setMenuItem] = useState(7);
    const [skipFetching, setSkipFetching] = useState(true);

    const { people, isLoading } = useGetUsersQuery(undefined, {
        pollingInterval: peopleInterval,
        skip: skipFetching,
        selectFromResult: ({ data }) => ({
            people: data !== undefined ? data?.filter((item) => item.id !== me?.id) : []
        }),
    });

    useEffect(() => {
        me !== null ? setSkipFetching(false) : setSkipFetching(true);
    }, [me]);

    const peopleListFilter = useCallback((value) => {
        if (value.id !== me?.id) {
            return value;
        }
    }, []);

    if (isLoading || people === undefined) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={menuItem}
                    setMenuItem={setMenuItem}
                />
                <Loading />
            </>
        );
    }

    return (
        <div className="communication">
            <CommunicationMenu
                currentMenuItem={menuItem}
                setMenuItem={setMenuItem}
            />
            <div className="communication__content people">
                <div>
                    <div className="people__title">{t("People")}</div>
                </div>
                <ul className="people__cards">
                    {
                        people?.filter(peopleListFilter).map((item) => (
                            <li key={item.id}>
                                <PeopleItem
                                    me={me}
                                    people={item}
                                />
                            </li>
                        ))
                    }
                </ul>
            </div>
        </div>
    );
}

export default People;