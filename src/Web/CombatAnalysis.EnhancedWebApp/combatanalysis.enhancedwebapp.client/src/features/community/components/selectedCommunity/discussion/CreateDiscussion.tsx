import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../../user/types/AppUserModel';
import { useCreateCommunityDiscussionMutation } from '../../../api/CommunityDiscussion.api';
import type { CommunityDiscussionModel } from '../../../types/CommunityDiscussionModel';
import type { CommunityModel } from '../../../types/CommunityModel';

interface CreateDiscussionProps {
    community: CommunityModel;
    myself: AppUserModel | null;
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
}

const CreateDiscussion: React.FC<CreateDiscussionProps> = ({ community, myself, setShowDiscussion }) => {
    const { t } = useTranslation("communication/community/discussion");

    const title = useRef<HTMLInputElement | null>(null);
    const content = useRef<HTMLTextAreaElement | null>(null);

    const [createCommunityAsync] = useCreateCommunityDiscussionMutation();

    const createDiscussionAsync = async () => {
        const newDiscussion: CommunityDiscussionModel = {
            id: 0,
            title: title.current?.value ?? "",
            content: content.current?.value ?? "",
            when: new Date(),
            appUserId: myself?.id ?? "",
            communityId: community?.id
        }

        const created = await createCommunityAsync(newDiscussion);
        if (created.data !== undefined) {
            setShowDiscussion(false);
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
                <input type="text" className="form-control" id="title" ref={title} />
            </div>
            <div className="form-group">
                <label htmlFor="Content">{t("Content")}</label>
                <textarea className="form-control" id="Content" rows={8} ref={content} />
            </div>
            <div className="actions">
                <div className="btn-shadow create" onClick={createDiscussionAsync}>{t("Create")}</div>
                <div className="btn-shadow" onClick={() => setShowDiscussion(false)}>{t("Cancel")}</div>
            </div>
        </form>
    );
}

export default CreateDiscussion;