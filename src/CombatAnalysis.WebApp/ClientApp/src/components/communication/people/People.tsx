import { useCallback, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetUsersQuery } from '../../../store/api/core/User.api';
import { AppUser } from '../../../types/AppUser';
import Loading from "../../Loading";
import CommunicationMenu from '../CommunicationMenu';
import PeopleItem from './PeopleItem';

import '../../../styles/communication/people/people.scss';

const peopleInterval = 3000;

const People: React.FC = () => {
    const { t } = useTranslation("communication/people/people");

    const me = useSelector((state: any) => state.user.value);

    const [skipFetching, setSkipFetching] = useState(true);

    const { people, isLoading } = useGetUsersQuery(undefined, {
        pollingInterval: peopleInterval,
        skip: skipFetching,
        selectFromResult: ({ data }: { data?: AppUser[] }) => ({
            people: data !== undefined ? data.filter((item) => item.id !== me?.id) : []
        }),
    });

    useEffect(() => {
        me !== null ? setSkipFetching(false) : setSkipFetching(true);
    }, [me]);

    const peopleListFilter = useCallback((value: AppUser) => {
        if (value.id !== me?.id) {
            return value;
        }
    }, []);

    if (isLoading || !people) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={4}
                    hasSubMenu={false}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <div className="communication-content people">
                <div>
                    <div className="people__title">{t("People")}</div>
                </div>
                <ul className="people__cards">
                    {people?.filter(peopleListFilter).map((item: AppUser) => (
                            <li className="person" key={item.id}>
                                <PeopleItem
                                    me={me}
                                    targetUser={item}
                                />
                            </li>
                        ))
                    }
                </ul>
            </div>
            <CommunicationMenu
                currentMenuItem={4}
                hasSubMenu={false}
            />
        </>
    );
}

export default People;