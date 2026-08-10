import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import logger from '@/utils/Logger';
import { useRef, useState, type SetStateAction } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreateCommunityMutation } from '../../api/Community.api';
import type { CommunityModel } from '../../types/CommunityModel';
import CommunityRulesItem from './CommunityRulesItem';

import './Create.scss';

const CreateCommunity: React.FC<{ setShowCreateCommunity: (value: SetStateAction<boolean>) => void }> = ({ setShowCreateCommunity }) => {
    const myself = useSelector((state: RootState) => state.user.value);

    const { t } = useTranslation('communication/create');

    const communityNameRef = useRef<HTMLInputElement | null>(null);
    const communityDescriptionRef = useRef<HTMLTextAreaElement | null>(null);

    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [isCreating, setIsCreating] = useState(false);

    const [createCommunity] = useCreateCommunityMutation();

    const createCommunityAsync = async () => {
        try {
            const newCommunity: CommunityModel = {
                id: 0,
                name: name,
                description: description,
                policyType: 0,
                appUserId: myself?.id ?? ""
            };

            await createCommunity(newCommunity).unwrap();
        } catch (e) {
            logger.error("Failed to create community", e);
        }
    }

    const handleCreateNewCommunityAsync = async () => {
        setIsCreating(true);

        await createCommunityAsync();

        setIsCreating(false);

        setShowCreateCommunity(false);
    }

    const communityNameChangeHandler = () => {
        if (communityNameRef.current) {
            setName(communityNameRef.current?.value);
        }
    }

    const communityDescriptionChangeHandler = () => {
        if (communityDescriptionRef.current) {
            setDescription(communityDescriptionRef.current.value);
        }
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
                        t={t}
                    />
                </div>
                <div className="actions">
                    <div className={`btn-shadow create ${(name.length > 0 && description.length > 0) ? '' : 'can-not-finish'}`}
                        onClick={(name.length > 0 && description.length > 0) ? handleCreateNewCommunityAsync : () => { } }>{t("Create")}</div>
                    <div className="btn-shadow" onClick={() => setShowCreateCommunity(false)}>{t("Cancel")}</div>
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