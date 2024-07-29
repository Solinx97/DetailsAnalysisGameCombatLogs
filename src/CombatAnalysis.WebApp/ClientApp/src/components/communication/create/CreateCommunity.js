import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCreateCommunityAsyncMutation } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import AddPeople from '../../AddPeople';
import CommunicationMenu from '../CommunicationMenu';
import CommunityRulesItem from './CommunityRulesItem';
import CreateGroupChatMenu from './CreateGroupChatMenu';
import ItemConnector from './ItemConnector';

import "../../../styles/communication/create.scss";

const CreateCommunity = () => {
    const user = useSelector((state) => state.user.value);

    const { t } = useTranslation("communication/create");

    const navigate = useNavigate();

    const [itemIndex, seItemIndex] = useState(0);
    const [passedItemIndex, setPassedItemIndex] = useState(0);
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [policy, setPolicy] = useState(0);
    const [canFinishCreate, setCanFinishCreate] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState([]);

    const [createCommunityAsyncMut] = useCreateCommunityAsyncMutation();
    const [createCommunityUserAsyncMut] = useCreateCommunityUserAsyncMutation();

    const createCommunityAsync = async () => {
        const newCommunity = {
            name: name,
            description: description,
            policyType: policy,
            appUserId: user?.id
        };

        const createdCommunity = await createCommunityAsyncMut(newCommunity);
        if (createdCommunity.data !== undefined) {
            peopleToJoin.push(user);

            await createCommunityUserAsync(createdCommunity.data.id);
        }
    }

    const createCommunityUserAsync = async (communityId) => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const newCommunityUser = {
                id: "",
                username: peopleToJoin[i].username,
                appUserId: peopleToJoin[i].id,
                communityId: communityId
            };

            await createCommunityUserAsyncMut(newCommunityUser);
        }
    }

    const handleCreateNewCommunityAsync = async () => {
        await createCommunityAsync();

        navigate("/communities");
    }

    const nextStep = (index) => {
        seItemIndex(index);
        passedItemIndex < index && setPassedItemIndex((index) => index + 1);

        if (index === 3) {
            setCanFinishCreate(true);

            return;
        }
    }

    const previouslyStep = () => {
        seItemIndex((index) => index - 1);
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={4}
            />
            <div className="communication__content create-community box-shadow">
                <div>{t("CreateCommunity")}</div>
                <CreateGroupChatMenu
                    passedItemIndex={passedItemIndex}
                    seItemIndex={seItemIndex}
                    itemIndex={itemIndex}
                />
                <div className="create-community__content">
                    <div className="create-community__items">
                        {itemIndex === 0 &&
                            <div className="create-community__item">
                                <div className="title">{t("Description")}</div>
                                <div>
                                    <div>
                                        <div className="form-group">
                                            <label htmlFor="name">{t("Name")}</label>
                                            <input type="text" className="form-control" name="name" id="name"
                                                onChange={(e) => setName(e.target.value)} defaultValue={name} required />
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="description">{t("Description")}</label>
                                            <textarea className="form-control" name="description" id="description"
                                                onChange={(e) => setDescription(e.target.value)} defaultValue={description} required />
                                        </div>
                                    </div>
                                    <ItemConnector
                                        connectorType={1}
                                        nextStep={nextStep}
                                        nextStepIndex={1}
                                    />
                                </div>
                            </div>
                        }
                        {itemIndex === 1 &&
                            <CommunityRulesItem
                                setPolicy={setPolicy}
                                connector={
                                    <ItemConnector
                                        connectorType={3}
                                        nextStep={nextStep}
                                        previouslyStep={previouslyStep}
                                        nextStepIndex={2}
                                    />
                                }
                            />
                        }
                        {itemIndex >= 2 &&
                            <div className="create-community__item">
                                <AddPeople
                                    user={user}
                                    communityUsersId={[user.id]}
                                    peopleToJoin={peopleToJoin}
                                    setPeopleToJoin={setPeopleToJoin}
                                />
                                <ItemConnector
                                    connectorType={3}
                                    nextStep={nextStep}
                                    previouslyStep={previouslyStep}
                                    nextStepIndex={3}
                                />
                            </div>
                        }
                    </div>
                </div>
                {((name.length === 0 || description.length === 0) && passedItemIndex > 0) &&
                    <div className="chat-name-required">{t("NameDescriptionRequired")}</div>
                }
                <div className="actions">
                    {(canFinishCreate && name.length > 0 && description.length > 0) &&
                        <div className="btn-shadow create" onClick={async () => await handleCreateNewCommunityAsync()}>{t("Create")}</div>
                    }
                    <div className="btn-shadow" onClick={() => navigate("/communities")}>{t("Cancel")}</div>
                </div>
            </div>
        </>
    );
}

export default CreateCommunity;