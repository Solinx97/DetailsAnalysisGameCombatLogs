import { useCallback, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetCustomersQuery } from '../../../store/api/UserApi';
import Communication from '../Communication';
import PeopleItem from './PeopleItem';

import '../../../styles/communication/people/people.scss';

const People = () => {
    const { t } = useTranslation("communication/people/people");

    const customer = useSelector((state) => state.customer.value);
    const [menuItem, setMenuItem] = useState(7);

    const { people, isLoading } = useGetCustomersQuery(undefined, {
        selectFromResult: ({ data }) => ({
            people: data !== undefined ? data.filter((item) => item.id !== customer?.id) : []
        }),
    });

    const peopleListFilter = useCallback((value) => {
        if (value.id !== customer?.id) {
            return value;
        }
    }, [])

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="communication">
            <Communication
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
                                    customer={customer}
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