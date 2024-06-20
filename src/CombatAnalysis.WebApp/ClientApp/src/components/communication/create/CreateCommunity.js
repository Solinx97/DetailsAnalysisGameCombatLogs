import { faCircleCheck, faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCreateCommunityAsyncMutation } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import AddPeople from '../../AddPeople';
import CommunicationMenu from '../CommunicationMenu';
import CommunityRulesItem from './CommunityRulesItem';
import ItemConnector from './ItemConnector';

import "../../../styles/communication/create.scss";

const CreateCommunity = () => {
    const customer = useSelector((state) => state.customer.value);

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
            customerId: customer.id
        };

        const createdCommunity = await createCommunityAsyncMut(newCommunity);
        if (createdCommunity.data !== undefined) {
            peopleToJoin.push(customer);

            await createCommunityUserAsync(createdCommunity.data.id);
        }
    }

    const createCommunityUserAsync = async (communityId) => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const newCommunityUser = {
                id: "",
                username: peopleToJoin[i].username,
                customerId: peopleToJoin[i].id,
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

    const changeMenuItem = (index) => {
        if (passedItemIndex < index) {
            return;
        }

        seItemIndex(index);
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={4}
            />
            <div className="communication__content create-community box-shadow">
                <div>Create community</div>
                <div className="create-community__content">
                    <ul className="create-community__menu">
                        <li className={`menu-item ${passedItemIndex >= 0 && "passed"}`} onClick={() => changeMenuItem(0)}>
                            {(passedItemIndex > 0 && itemIndex !== 0)
                                ? <FontAwesomeIcon
                                    className="menu-item__passed"
                                    icon={faCircleCheck}
                                />
                                : itemIndex === 0 &&
                                <FontAwesomeIcon
                                    icon={faCircleQuestion}
                                />
                            }
                            <div>{t("Description")}</div>
                        </li>
                        <li className={`menu-item ${passedItemIndex >= 1 && "passed"}`} onClick={() => changeMenuItem(1)}>
                            {(passedItemIndex > 1 && itemIndex !== 1)
                                ? <FontAwesomeIcon
                                    className="menu-item__passed"
                                    icon={faCircleCheck}
                                />
                                : itemIndex === 1 &&
                                <FontAwesomeIcon
                                    icon={faCircleQuestion}
                                />
                            }
                            <div>{t("Rules")}</div>
                        </li>
                        <li className={`menu-item ${passedItemIndex >= 2 && "passed"}`} onClick={() => changeMenuItem(2)}>
                            {(passedItemIndex > 2 && itemIndex !== 2)
                                ? <FontAwesomeIcon
                                    className="menu-item__passed"
                                    icon={faCircleCheck}
                                />
                                : itemIndex === 2 &&
                                <FontAwesomeIcon
                                    icon={faCircleQuestion}
                                />
                            }
                            <div>{t("InvitePeople")}</div>
                        </li>
                    </ul>
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
                                    customer={customer}
                                    communityUsersId={[customer.id]}
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
                <div className="finish-create">
                    <div className="btn-shadow" onClick={async () => await handleCreateNewCommunityAsync()}>{t("Create")}</div>
                    <div className="btn-shadow" onClick={() => navigate("/communities")}>{t("Cancel")}</div>
                </div>
            </div>
        </>
    );
}

export default CreateCommunity;