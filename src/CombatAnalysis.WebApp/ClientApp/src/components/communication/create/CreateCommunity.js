import { useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCreateCommunityUserMutation } from '../../../store/api/community/CommunityUser.api';
import { useCreateCommunityMutation } from '../../../store/api/core/Community.api';
import CommunicationMenu from '../CommunicationMenu';
import CommunityRulesItem from './CommunityRulesItem';

import "../../../styles/communication/create.scss";

const CreateCommunity = () => {
    const me = useSelector((state) => state.user.value);

    const { t } = useTranslation("communication/create");

    const navigate = useNavigate();

    const communityNameRef = useRef(null);
    const communityDescriptionRef = useRef(null);

    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [policy, setPolicy] = useState(0);
    const [isCreating, setIsCreating] = useState(false);

    const [createCommunityAsyncMut] = useCreateCommunityMutation();
    const [createCommunityUserAsyncMut] = useCreateCommunityUserMutation();

    const createCommunityAsync = async () => {
        const newCommunity = {
            name: name,
            description: description,
            policyType: policy,
            appUserId: me?.id
        };

        const createdCommunity = await createCommunityAsyncMut(newCommunity);
        if (createdCommunity.data !== undefined) {
            await createCommunityUserAsync(createdCommunity.data.id);
        }
    }

    const createCommunityUserAsync = async (communityId) => {
        const newCommunityUser = {
            id: "",
            username: me?.username,
            appUserId: me?.id,
            communityId: communityId
        };

        await createCommunityUserAsyncMut(newCommunityUser);
    }

    const handleCreateNewCommunityAsync = async () => {
        setIsCreating(true);

        await createCommunityAsync();

        setIsCreating(false);

        navigate("/communities");
    }

    const communityNameChangeHandler = () => {
        setName(communityNameRef.current?.value);
    }

    const communityDescriptionChangeHandler = () => {
        setDescription(communityDescriptionRef.current?.value);
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={4}
            />
            <div className="communication-content create-communication-object box-shadow">
                <div>{t("CreateCommunity")}</div>
                <div className="create-communication-object__content">
                    <div className="create-communication-object__item">
                        <div className="form-group">
                            <label htmlFor="name">{t("Name")}</label>
                            <input type="text" className="form-control" name="name" id="name"
                                onChange={communityNameChangeHandler} ref={communityNameRef} required />
                        </div>
                        {name.length === 0 &&
                            <div className="community-name-required">{t("NameRequired")}</div>
                        }
                        <div className="form-group">
                            <label htmlFor="description">{t("Description")}</label>
                            <textarea className="form-control" name="description" id="description"
                                onChange={communityDescriptionChangeHandler} ref={communityDescriptionRef} required />
                        </div>
                        {description.length === 0 &&
                            <div className="community-description-required">{t("DescriptionRequired")}</div>
                        }
                    </div>
                    <CommunityRulesItem
                        setPolicy={setPolicy}
                        t={t}
                    />
                </div>
                <div className="actions">
                    <div className={`btn-shadow create ${(name.length > 0 && description.length > 0) ? '' : 'can-not-finish'}`}
                        onClick={(name.length > 0 && description.length > 0) ? handleCreateNewCommunityAsync : null}>{t("Create")}</div>
                    <div className="btn-shadow" onClick={() => navigate("/communities")}>{t("Cancel")}</div>
                </div>
                {isCreating &&
                    <>
                        <span className="creating"></span>
                        <div className="notify">Creating...</div>
                    </>
                }
            </div>
        </>
    );
}

export default CreateCommunity;