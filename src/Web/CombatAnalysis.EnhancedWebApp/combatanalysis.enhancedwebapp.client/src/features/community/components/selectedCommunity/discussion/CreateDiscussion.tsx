import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../../user/types/AppUserModel';
import { useCreateCommunityDiscussionMutation } from '../../../api/CommunityDiscussion.api';
import type { CommunityDiscussionModel } from '../../../types/CommunityDiscussionModel';
import type { CommunityModel } from '../../../types/CommunityModel';

interface CreateDiscussionProps {
    community: CommunityModel;
    myself: AppUserModel;
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
}

const CreateDiscussion: React.FC<CreateDiscussionProps> = ({ community, myself, setShowDiscussion }) => {
    const maxTitleLength = 128;
    const maxContentLength = 512;

    const { t } = useTranslation("communication/community/discussion");

    const maxTitleLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDiscussionTitleMaxLength ?? maxTitleLength);
    const maxContentLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDiscussionContentMaxLength ?? maxContentLength);
    const titleRef = useRef<HTMLInputElement | null>(null);
    const contentRef = useRef<HTMLTextAreaElement | null>(null);

    const [currentTitleLength, setCurrentTileLength] = useState(0);
    const [currentContentLength, setCurrentContentLength] = useState(0);

    const [createCommunityAsync] = useCreateCommunityDiscussionMutation();

    const createDiscussionAsync = async () => {
        try {
            const newDiscussion: CommunityDiscussionModel = {
                id: 0,
                title: titleRef.current?.value ?? "",
                content: contentRef.current?.value ?? "",
                createdAt: new Date(),
                appUserId: myself.id,
                communityId: community.id
            }

            await createCommunityAsync(newDiscussion).unwrap();
            setShowDiscussion(false);
        } catch (error) {
            logger.error("Failed to create community discussion", error);
        }
    }

    return (
        <form className="discussion__create box-shadow">
            <div className="menu">
                <FontAwesomeIcon
                    icon={faCircleXmark}
                    title={t("Close")}
                    onClick={() => setShowDiscussion(false)}
                />
            </div>
            <div className="title">{t("CreateDiscussion")}</div>
            <div className="form-group">
                <label htmlFor="title">{t("Title")}</label>
                <div className={`content-length ${titleRef.current?.value.length === maxTitleLengthRef.current ? 'limit' : ''}`}>{currentTitleLength}/{maxTitleLengthRef.current}</div>
                <input type="text" className="form-control" id="title" ref={titleRef} maxLength={maxTitleLengthRef.current} 
                    onChange={e => setCurrentTileLength(e.target.value.length)} />
            </div>
            <div className="form-group">
                <label htmlFor="Content">{t("Content")}</label>
                <div className={`content-length ${titleRef.current?.value.length === maxContentLengthRef.current ? 'limit' : ''}`}>{currentContentLength}/{maxContentLengthRef.current}</div>
                <textarea className="form-control" id="Content" rows={8} ref={contentRef} maxLength={maxContentLengthRef.current} 
                    onChange={e => setCurrentContentLength(e.target.value.length)} />
            </div>
            <div className="actions">
                <div className="btn-shadow create" onClick={createDiscussionAsync}>{t("Create")}</div>
                <div className="btn-shadow" onClick={() => setShowDiscussion(false)}>{t("Cancel")}</div>
            </div>
        </form>
    );
}

export default CreateDiscussion;