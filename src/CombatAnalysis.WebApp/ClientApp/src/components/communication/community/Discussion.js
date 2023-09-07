import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateCommunityDiscussionAsyncMutation, useRemoveCommunityDiscussionAsyncMutation, useGetCommunityDiscussionByIdQuery } from '../../../store/api/communication/community/CommunityDiscussion.api';

const Discussion = ({ discussionId, setShowDiscussion }) => {
    const { t } = useTranslation("communication/community/selectedCommunity");

    const [title, setTitle] = useState("");
    const [content, setContent] = useState("");
    const [editModeOne, setEditModeOne] = useState(false);

    const [updateCommunityAsyncMut] = useUpdateCommunityDiscussionAsyncMutation();
    const [removeCommunityDiscussionAsyncMut] = useRemoveCommunityDiscussionAsyncMutation();
    const { data: discussion, isLoading} = useGetCommunityDiscussionByIdQuery(discussionId);

    useEffect(() => {
        if (discussion === undefined) {
            return;
        }

        setTitle(discussion.title);
        setContent(discussion.content);
    }, [discussion])

    const updateDiscussionAsync = async (event) => {
        event.preventDefault();

        const newDiscussion = {
            id: discussion.id,
            title: title,
            content: content,
            when: discussion.when,
            customerId: discussion.customerId,
            communityId: discussion.communityId
        }

        const updated = await updateCommunityAsyncMut(newDiscussion);
        if (updated.data !== undefined) {
            setEditModeOne(false);
        }
    }

    const removeDiscussionAsync = async () => {
        setShowDiscussion(false);

        await removeCommunityDiscussionAsyncMut(discussionId);
    }

    const handleTitle = (event) => {
        setTitle(event.target.value);
    }

    const handleContent = (event) => {
        setContent(event.target.value);
    }

    const information = () => {
        return (
            <div className="discussion__selected-discussion">
                <div className="tool">
                    <div className="title">{discussion.title}</div>
                    <div className="actions">
                        <FontAwesomeIcon
                            icon={faPen}
                            title={t("Edit")}
                            onClick={() => setEditModeOne(true)}
                        />
                        <FontAwesomeIcon
                            icon={faTrash}
                            title={t("Remvoe")}
                            onClick={async () => await removeDiscussionAsync()}
                        />
                    </div>
                </div>
                <div className="form-control content">{discussion.content}</div>
                <div className="actions">
                    <input type="button" className="btn btn-light" value={t("Close")} onClick={() => setShowDiscussion(false)} />
                </div>
            </div>
        );
    }

    const edit = () => {
        return (
            <form className="discussion__create" onSubmit={async (event) => await updateDiscussionAsync(event)}>
                <div className="form-group">
                    <label htmlFor="title">{t("Title")}</label>
                    <input type="text" className="form-control" id="title" defaultValue={discussion.title} onChange={handleTitle} />
                </div>
                <div className="form-group">
                    <label htmlFor="Content">{t("Content")}</label>
                    <textarea className="form-control" id="Content" rows="8" defaultValue={discussion.content} onChange={handleContent} />
                </div>
                <div className="actions">
                    <input type="submit" className="btn btn-primary" value={t("Save")} />
                    <input type="button" className="btn btn-light" value={t("Cancel")} onClick={() => setEditModeOne(false)} />
                </div>
            </form>
        );
    }

    if (isLoading) {
        return <></>;
    }

    return editModeOne ? edit() : information();
}

export default Discussion;