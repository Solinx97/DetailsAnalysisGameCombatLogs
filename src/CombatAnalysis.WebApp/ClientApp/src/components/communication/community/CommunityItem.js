import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { useGetCommunitiesQuery } from '../../../store/api/ChatApi';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/CommunityUser.api';

const CommunityItem = ({ filterContent }) => {
    const { t } = useTranslation("communication/community/Communities");

    const customer = useSelector((state) => state.customer.value);

    const { data: communities, isLoading } = useGetCommunitiesQuery();
    const { data: userCommunities, isLoading: userCommunitiesIsLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    if (isLoading || userCommunitiesIsLoading) {
        return <></>;
    }

    const anotherCommunity = (community) => {
        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    const render = () => {
        return (
            <ul>
                {
                    communities?.map((item) => (
                        (anotherCommunity(item) && item.name.toLowerCase().startsWith(filterContent.toLowerCase())) &&
                        <li key={item.id} className="community">
                            <div className="card">
                                <div className="card-body">
                                    <h5 className="card-title">{item.name}</h5>
                                    <p className="card-text">{item.description}</p>
                                    <NavLink
                                        className="card-link"
                                        to={`/community?id=${item?.id}`}
                                    >
                                        {t("Open")}
                                    </NavLink>
                                    <NavLink
                                        className="card-link"
                                    >
                                        {t("Join")}
                                    </NavLink>
                                </div>
                            </div>
                        </li>
                    ))
                }
            </ul>
        );
    }

    return render();
}

export default CommunityItem;