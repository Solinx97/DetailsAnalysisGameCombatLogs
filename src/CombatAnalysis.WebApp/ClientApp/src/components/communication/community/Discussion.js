import { faPen, faSquarePlus, faTrash, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCommunityDiscussionByIdQuery, useRemoveCommunityDiscussionAsyncMutation, useUpdateCommunityDiscussionAsyncMutation } from '../../../store/api/communication/community/CommunityDiscussion.api';
import { useCreateCommunityDiscussionCommentAsyncMutation } from '../../../store/api/communication/community/CommunityDiscussionComment.api';
import DiscussionComments from './DiscussionComments';

import '../../../styles/communication/community/discussion.scss';

const Discussion = ({ discussionId, setShowDiscussion, customer }) => {
    const { t } = useTranslation("communication/community/discussion");

    const [title, setTitle] = useState("");
    const [content, setContent] = useState("");
    const [editModeOne, setEditModeOne] = useState(false);
    const [showComments, setShowComments] = useState(false);
    const [showAddComment, setAddShowComment] = useState(false);
    const [discussionCommentContent, setDiscussionCommentContent] = useState("");

    const [updateCommunityAsyncMut] = useUpdateCommunityDiscussionAsyncMutation();
    const [removeCommunityDiscussionAsyncMut] = useRemoveCommunityDiscussionAsyncMutation();
    const [createCommunityDiscussionCommentAsyncMut] = useCreateCommunityDiscussionCommentAsyncMutation();
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

    const createDiscussionCommentAsync = async () => {
        const newDiscussionComment = {
            content: discussionCommentContent,
            when: new Date(),
            communityDiscussionId: discussionId,
            customerId: customer.id
        }

        const created = await createCommunityDiscussionCommentAsyncMut(newDiscussionComment);
        if (created.data !== undefined) {
            setDiscussionCommentContent("");
        }
    }

    const handleTitle = (event) => {
        setTitle(event.target.value);
    }

    const handleContent = (event) => {
        setContent(event.target.value);
    }

    const dateFormatting = (stringOfDate) => {
        const date = new Date(stringOfDate);
        const month = date.getMonth();
        let nameOfMonth = "";

        switch (month) {
            case 0:
                nameOfMonth = "January";
                break;
            case 1:
                nameOfMonth = "February";
                break;
            case 2:
                nameOfMonth = "March";
                break;
            case 3:
                nameOfMonth = "April";
                break;
            case 4:
                nameOfMonth = "May";
                break;
            case 5:
                nameOfMonth = "June";
                break;
            case 6:
                nameOfMonth = "July";
                break;
            case 7:
                nameOfMonth = "August";
                break;
            case 8:
                nameOfMonth = "September";
                break;
            case 9:
                nameOfMonth = "October";
                break;
            case 10:
                nameOfMonth = "November";
                break;
            case 11:
                nameOfMonth = "December";
                break;
            default:
                break;
        }

        const formatted = `${date.getDate()} ${nameOfMonth}, ${date.getHours()}:${date.getMinutes()}`;

        return formatted;
    }

    const information = () => {
        return (
            <div className="information">
                <div className="tool">
                    <div className="menu">
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            title={t("Close")}
                            onClick={() => setShowDiscussion(false)}
                        />
                    </div>
                    <div className="title">{discussion.title}</div>
                    {discussion?.customerId === customer?.id &&
                        <div className="actions">
                            <div className={`btn-shadow ${showComments ? "active" : ""}`} onClick={() => setEditModeOne(true)}>
                                <FontAwesomeIcon
                                    icon={faPen}
                                    title={t("Edit")}
                                />
                                <div>{t("Edit")}</div>
                            </div>
                            <div className={`btn-shadow ${showComments ? "active" : ""}`} onClick={async () => await removeDiscussionAsync()}>
                                <FontAwesomeIcon
                                    icon={faTrash}
                                    title={t("Remove")}
                                />
                                <div>{t("Remove")}</div>
                            </div>
                        </div>
                    }
                </div>
                <div className="form-control content">
                    <div>{discussion.content}</div>
                    <div className="select-add-new-discussion-comment">
                        <div className={`btn-shadow ${showComments ? "active" : ""}`} onClick={() => setShowComments((item) => !item)}>
                            <FontAwesomeIcon
                                icon={faSquarePlus}
                                title={t("AddComment")}
                            />
                            <div>{t("Comments")}</div>
                        </div>
                    </div>
                </div>
                {showComments &&
                    <>
                        <DiscussionComments
                            dateFormatting={dateFormatting}
                            customerId={customer?.id}
                            discussionId={discussionId}
                        />
                        <div className="add-new-discussion-comment">
                            <div className="add-new-discussion-comment__title">
                            {showAddComment
                                ? <div>{t("AddComment")}</div>
                                : <div className="btn-shadow add-comment" onClick={() => setAddShowComment((item) => !item)}>{t("AddComment")}</div>
                            }
                        </div>
                        {showAddComment &&
                            <div className="add-new-discussion-comment__content">
                                <textarea className="form-control" rows="3" cols="60" onChange={e => setDiscussionCommentContent(e.target.value)} value={discussionCommentContent} />
                                <div className="actions">
                                    <div className="btn-shadow create" onClick={async () => await createDiscussionCommentAsync()}>{t("Add")}</div>
                                    <div className="btn-shadow hide" onClick={() => setAddShowComment((item) => !item)}>{t("Hide")}</div>
                                </div>
                            </div>
                        }
                        </div>
                    </>
                }
                <div className="actions">
                    <div className="btn-shadow" onClick={() => setShowDiscussion(false)}>{t("Close")}</div>
                </div>
            </div>
        );
    }

    const edit = () => {
        return (
            <form className="edit" onSubmit={async (event) => await updateDiscussionAsync(event)}>
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

    return (
        <div className="discussion__selected-discussion box-shadow">
            {editModeOne
                ? edit()
                : information()
            }
        </div>
    );
}

export default Discussion;