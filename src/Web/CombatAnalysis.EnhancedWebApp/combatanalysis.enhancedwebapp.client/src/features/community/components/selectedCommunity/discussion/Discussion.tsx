import logger from '@/utils/Logger';
import { faCircleXmark, faPen, faSquarePlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type ChangeEvent, type FormEvent, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../../user/types/AppUserModel';
import {
    useGetCommunityDiscussionByIdQuery,
    useRemoveCommunityDiscussionMutation,
    useUpdateCommunityDiscussionMutation
} from '../../../api/CommunityDiscussion.api';
import { useCreateCommunityDiscussionCommentAsyncMutation } from '../../../api/CommunityDiscussionComment.api';
import type { CommunityDiscussionCommentModel } from '../../../types/CommunityDiscussionCommentModel';
import type { CommunityDiscussionModel } from '../../../types/CommunityDiscussionModel';
import DiscussionComments from './DiscussionComments';

import './Discussion.scss';

interface DiscussionProps {
    user: AppUserModel | null;
    discussionId: number;
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
}

const Discussion: React.FC<DiscussionProps> = ({ user, discussionId, setShowDiscussion }) => {
    const { t } = useTranslation('communication/community/discussion');

    const [title, setTitle] = useState("");
    const [content, setContent] = useState("");
    const [editModeOne, setEditModeOne] = useState(false);
    const [showComments, setShowComments] = useState(false);
    const [showAddComment, setAddShowComment] = useState(false);
    const [discussionCommentContent, setDiscussionCommentContent] = useState("");

    const [updateCommunityDiscussionAsync] = useUpdateCommunityDiscussionMutation();
    const [removeCommunityDiscussionAsyncMut] = useRemoveCommunityDiscussionMutation();
    const [createCommunityDiscussionCommentAsyncMut] = useCreateCommunityDiscussionCommentAsyncMutation();
    const { data: discussion, isLoading } = useGetCommunityDiscussionByIdQuery(discussionId);

    useEffect(() => {
        if (discussion === undefined) {
            return;
        }

        setTitle(discussion.title);
        setContent(discussion.content);
    }, [discussion])

    const updateDiscussionAsync = async (event: FormEvent<HTMLFormElement>) => {
        try {
            event.preventDefault();

            if (!discussion) {
                return;
            }

            const updateDiscussion: CommunityDiscussionModel = {
                id: discussion.id,
                title: title,
                content: content,
                createdAt: discussion.createdAt,
                appUserId: discussion.appUserId,
                communityId: discussion.communityId
            }

            await updateCommunityDiscussionAsync({ id: updateDiscussion.id, discussion: updateDiscussion }).unwrap();
            setEditModeOne(false);
        } catch (error) {
            logger.error("Failed to update community descussion", error);
        }
    }

    const removeDiscussionAsync = async () => {
        setShowDiscussion(false);

        await removeCommunityDiscussionAsyncMut(discussionId);
    }

    const createDiscussionCommentAsync = async () => {
        try {
            const newDiscussionComment: CommunityDiscussionCommentModel = {
                id: 0,
                content: discussionCommentContent,
                createdAt: new Date(),
                communityDiscussionId: discussionId,
                appUserId: user?.id ?? "",
            }

            await createCommunityDiscussionCommentAsyncMut(newDiscussionComment).unwrap();
            setDiscussionCommentContent("");
        } catch (error) {
            logger.error("Failed to create community descussion comment", error);
        }
    }

    const titleHandle = (event: ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    }

    const contentHandle = (event: ChangeEvent<HTMLTextAreaElement>) => {
        setContent(event.target.value);
    }

    if (!discussion || !user) {
        return (<></>);
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
                    {discussion?.appUserId === user.id &&
                        <div className="actions">
                            <div className={`btn-shadow ${showComments ? "active" : ""}`} onClick={() => setEditModeOne(true)}>
                                <FontAwesomeIcon
                                    icon={faPen}
                                    title={t("Edit")}
                                />
                                <div>{t("Edit")}</div>
                            </div>
                            <div className={`btn-shadow ${showComments ? "active" : ""}`} onClick={removeDiscussionAsync}>
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
                        <div className="add-new-discussion-comment">
                            <div className="add-new-discussion-comment__title">
                                {showAddComment
                                    ? <div>{t("AddComment")}</div>
                                    : <div className="btn-shadow add-comment" onClick={() => setAddShowComment((item) => !item)}>{t("AddComment")}</div>
                                }
                            </div>
                            {showAddComment &&
                                <div className="add-new-discussion-comment__content">
                                    <textarea className="form-control" rows={3} cols={60} onChange={e => setDiscussionCommentContent(e.target.value)} value={discussionCommentContent} />
                                    <div className="actions">
                                        <div className="btn-shadow create" onClick={createDiscussionCommentAsync}>{t("Add")}</div>
                                        <div className="btn-shadow hide" onClick={() => setAddShowComment((item) => !item)}>{t("Hide")}</div>
                                    </div>
                                </div>
                            }
                        </div>
                        <DiscussionComments
                            userId={user.id}
                            discussionId={discussionId}
                        />
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
            <form className="edit" onSubmit={updateDiscussionAsync}>
                <div className="form-group">
                    <label htmlFor="title">{t("Title")}</label>
                    <input type="text" className="form-control" id="title" defaultValue={discussion.title}
                        onChange={titleHandle} />
                </div>
                <div className="form-group">
                    <label htmlFor="Content">{t("Content")}</label>
                    <textarea className="form-control" id="Content" rows={8} defaultValue={discussion.content}
                        onChange={contentHandle} />
                </div>
                <div className="actions">
                    <input type="submit" className="btn btn-primary" value={t("Save")} />
                    <input type="button" className="btn btn-secondary" value={t("Cancel")} onClick={() => setEditModeOne(false)} />
                </div>
            </form>
        );
    }

    if (isLoading) {
        return (<div>Loading...</div>);
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