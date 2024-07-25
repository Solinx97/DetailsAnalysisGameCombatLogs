import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreateCommunityDiscussionAsyncMutation } from '../../../store/api/communication/community/CommunityDiscussion.api';

const CreateDiscussion = ({ community, customer, setShowDiscussion }) => {
    const { t } = useTranslation("communication/community/discussion");

    const title = useRef(null);
    const content = useRef(null);

    const [createCommunityAsyncMut] = useCreateCommunityDiscussionAsyncMutation();

    const createDiscussionAsync = async (event) => {
        event.preventDefault();

        const newDiscussion = {
            title: title.current.value,
            content: content.current.value,
            when: new Date(),
            appUserId: customer?.id,
            communityId: community?.id
        }

        const created = await createCommunityAsyncMut(newDiscussion);
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
                <textarea className="form-control" id="Content" rows="8" ref={content} />
            </div>
            <div className="actions">
                <div className="btn-shadow create" onClick={async (event) => await createDiscussionAsync(event)}>{t("Create")}</div>
                <div className="btn-shadow" onClick={() => setShowDiscussion(false)}>{t("Cancel")}</div>
            </div>
        </form>
    );
}

export default CreateDiscussion;