import { faCircleQuestion, faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useGetCommunityByIdQuery } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';

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
                    <div className="links">
                        <div className="open-community">
                            <div className="open-community__content" onClick={() => navigate(`/community?id=${community?.id}`)}>
                                <FontAwesomeIcon
                                    icon={faCircleQuestion}
                                />
                                <div>{t("Open")}</div>
                            </div>
                        </div>
                        <div className="join-to-community">
                            <div className="join-to-community__content" onClick={async () => await createCommunityUserAsync()}>
                                <FontAwesomeIcon
                                    icon={faCirclePlus}
                                />
                                <div>{t("Join")}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default CommunityItem;