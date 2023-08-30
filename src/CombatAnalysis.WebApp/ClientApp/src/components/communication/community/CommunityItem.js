import { useTranslation } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { useGetCommunityByIdQuery } from '../../../store/api/communication/community/Community.api';

const CommunityItem = ({ id }) => {
    const { t } = useTranslation("communication/community/Communities");

    const { data: community, isLoading } = useGetCommunityByIdQuery(id);

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{community?.name}</h5>
                    <p className="card-text">{community?.description}</p>
                    <NavLink
                        className="card-link"
                        to={`/community?id=${community?.id}`}
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
        </>
    );
}

export default CommunityItem;