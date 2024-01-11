import { useCallback, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetCustomersQuery } from '../../../store/api/UserApi';
import CommunicationMenu from '../CommunicationMenu';
import PeopleItem from './PeopleItem';

import '../../../styles/communication/people/people.scss';

const peopleInterval = 5000;

const People = () => {
    const { t } = useTranslation("communication/people/people");

    const me = useSelector((state) => state.customer.value);
    const [menuItem, setMenuItem] = useState(7);

    const { people, isLoading } = useGetCustomersQuery(undefined, {
        pollingInterval: peopleInterval,
        selectFromResult: ({ data }) => ({
            people: data !== undefined ? data.filter((item) => item.id !== me?.id) : []
        }),
    });

    const peopleListFilter = useCallback((value) => {
        if (value.id !== me?.id) {
            return value;
        }
    }, [])

    if (isLoading) {
        return (
            <CommunicationMenu
                currentMenuItem={menuItem}
                setMenuItem={setMenuItem}
            />
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
                    <div>{t("People")}</div>
                </div>
                <ul className="people__cards">
                    {
                        people.filter(peopleListFilter).map((item) => (
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