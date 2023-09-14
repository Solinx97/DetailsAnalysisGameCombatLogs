import { useTranslation } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { useGetCommunityByIdQuery } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import { useNavigate } from 'react-router-dom';

const CommunityItem = ({ id, me }) => {
    const { t } = useTranslation("communication/community/Communities");

    const navigate = useNavigate();

    const { data: community, isLoading } = useGetCommunityByIdQuery(id);
    const [createCommunityUserAsyncMut] = useCreateCommunityUserAsyncMutation();

    const createCommunityUserAsync = async () => {
        const newCommunityUser = {
            id: "",
            username: me?.username,
            customerId: me?.id,
            communityId: id
        };

        const createdUser = await createCommunityUserAsyncMut(newCommunityUser);
        if (createdUser.data !== undefined) {
            navigate(`/community?id=${id}`);
        }
    }

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
                        onClick={async () => await createCommunityUserAsync()}
                    >
                        {t("Join")}
                    </NavLink>
                </div>
            </div>
        </>
    );
}

export default CommunityItem;