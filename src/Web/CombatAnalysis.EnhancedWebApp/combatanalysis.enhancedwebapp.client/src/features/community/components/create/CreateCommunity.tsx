import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import logger from '@/utils/Logger';
import { useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreateCommunityMutation } from '../../api/Community.api';
import type { RulesModel } from '../../types/community/RulesModel';
import type { CommunityModel } from '../../types/CommunityModel';
import CommunityRulesItem from './CommunityRulesItem';

import './Create.scss';

const CreateCommunity: React.FC<{ setShowCreateCommunity: (value: SetStateAction<boolean>) => void }> = ({ setShowCreateCommunity }) => {
    const maxNameLength = 128;
    const maxDescriptionLength = 512;

    const { t } = useTranslation('communication/create');

    const myself = useSelector((state: RootState) => state.user.value);

    const maxNameLengthRef = useRef<number>(APP_CONFIG.communication.length.communityNameMaxLength ?? maxNameLength);
    const maxDescriptionLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDescriptionMaxLength ?? maxDescriptionLength);
    
    const communityNameRef = useRef<HTMLInputElement | null>(null);
    const communityDescriptionRef = useRef<HTMLTextAreaElement | null>(null);

    const [currentNameLength, setCurrentNameLength] = useState(0);
    const [currentDescriptionLength, setCurrentDescriptionLength] = useState(0);
    const [isCreating, setIsCreating] = useState(false);
    const [rules, setRules] = useState<RulesModel>({
        policy: 0,
        invite: 0,
        remove: 0
    });

    const [createCommunity] = useCreateCommunityMutation();

    const createCommunityAsync = async () => {
        try {
            if (!myself) {
                return;
            }

            const newCommunity: CommunityModel = {
                id: 0,
                name: communityNameRef.current?.value ?? "",
                description: communityDescriptionRef.current?.value ?? "",
                policyType: rules.policy,
                appUserId: myself.id
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

    const canBeCreated = () => {
        if (!communityNameRef.current || !communityDescriptionRef.current) {
            return false;
        }

        return communityNameRef.current.value.length > 0 && communityDescriptionRef.current.value.length > 0;
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
                            <div className={`content-length ${communityNameRef.current?.value.length === maxNameLengthRef.current ? 'limit' : ''}`}>{currentNameLength}/{maxNameLengthRef.current}</div>
                            <input type="text" className="form-control" name="name" id="name" maxLength={maxNameLengthRef.current}
                                onChange={e => setCurrentNameLength(e.target.value.length)} ref={communityNameRef} required />
                        </div>
                        {currentNameLength === 0 &&
                            <div className="community-name-required">{t("NameRequired")}</div>
                        }
                        <div className="form-group">
                            <label htmlFor="description">{t("Description")}</label>
                            <div className={`content-length ${communityDescriptionRef.current?.value.length === maxDescriptionLengthRef.current ? 'limit' : ''}`}>{currentDescriptionLength}/{maxDescriptionLengthRef.current}</div>
                            <textarea className="form-control" name="description" id="description" maxLength={maxDescriptionLengthRef.current}
                                onChange={e => setCurrentDescriptionLength(e.target.value.length)} ref={communityDescriptionRef} required />
                        </div>
                        {currentDescriptionLength === 0 &&
                            <div className="community-description-required">{t("DescriptionRequired")}</div>
                        }
                    </div>
                    <CommunityRulesItem
                        t={t}
                        rules={rules}
                        setRules={setRules}
                    />
                </div>
                <div className="actions">
                    <div className={`btn-shadow create ${canBeCreated() ? '' : 'can-not-finish'}`}
                        onClick={canBeCreated() ? handleCreateNewCommunityAsync : () => { }}>{t("Create")}</div>
                    <div className="btn-shadow" onClick={() => setShowCreateCommunity(false)}>{t("Cancel")}</div>
                </div>
                {isCreating &&
                    <>
                        <span className="creating"></span>
                        <div className="notify">{t("Creating")}</div>
                    </>
                }
            </div>
        </>
    );
}

export default CreateCommunity;