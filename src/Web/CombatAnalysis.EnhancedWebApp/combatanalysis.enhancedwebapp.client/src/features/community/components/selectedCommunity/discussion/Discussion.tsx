import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import { faCircleXmark, faPen, faSquarePlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState, type ChangeEvent, type SetStateAction } from 'react';
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
    user: AppUserModel;
    discussionId: number;
    communityId: number;
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
}

const Discussion: React.FC<DiscussionProps> = ({ user, discussionId, communityId, setShowDiscussion }) => {
    const maxTitleLength = 128;
    const maxContentLength = 512;
    const maxCommentContentLength = 256;

    const { t } = useTranslation('communication/community/discussion');

    const maxTitleLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDiscussionTitleMaxLength ?? maxTitleLength);
    const maxContentLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDiscussionContentMaxLength ?? maxContentLength);
    const maxCommentContentLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDiscussionCommentContentMaxLength ?? maxCommentContentLength);

    const [currentTitleLength, setCurrentTileLength] = useState(0);
    const [currentContentLength, setCurrentContentLength] = useState(0);
    const [currentCommentContentLength, setCurrentCommentContentLength] = useState(0);

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
        setCurrentTileLength(discussion.title.length);
        setContent(discussion.content);
        setCurrentContentLength(discussion.content.length);
    }, [discussion])

    const updateDiscussionAsync = async () => {
        try {
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
        try {
            setShowDiscussion(false);

            await removeCommunityDiscussionAsyncMut({ id: discussionId, communityId }).unwrap();
        } catch (error) {
            logger.error("Failed to remove community descussion", error);
        }
    }

    const createDiscussionCommentAsync = async () => {
        try {
            const newDiscussionComment: CommunityDiscussionCommentModel = {
                id: 0,
                content: discussionCommentContent,
                createdAt: new Date(),
                communityDiscussionId: discussionId,
                appUserId: user.id,
            }

            await createCommunityDiscussionCommentAsyncMut(newDiscussionComment).unwrap();
            setDiscussionCommentContent("");
            setAddShowComment(false);
            setCurrentCommentContentLength(0);
        } catch (error) {
            logger.error("Failed to create community descussion comment", error);
        }
    }

    const titleHandle = (event: ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
        setCurrentTileLength(event.target.value.length);
    }

    const contentHandle = (event: ChangeEvent<HTMLTextAreaElement>) => {
        setContent(event.target.value);
        setCurrentContentLength(event.target.value.length);
    }

    const commentContentHandle = (event: ChangeEvent<HTMLTextAreaElement>) => {
        setDiscussionCommentContent(event.target.value);
        setCurrentCommentContentLength(event.target.value.length);
    }

    if (!discussion) {
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
                                    ? <div className="info">
                                        <div>{t("AddComment")}</div>
                                        <div className={`content-length ${discussionCommentContent.length === maxCommentContentLengthRef.current ? 'limit' : ''}`}>{currentCommentContentLength}/{maxCommentContentLengthRef.current}</div>
                                    </div>
                                    : <div className="btn-shadow add-comment" onClick={() => setAddShowComment((item) => !item)}>{t("AddComment")}</div>
                                }
                            </div>
                            {showAddComment &&
                                <div className="add-new-discussion-comment__content">
                                    <textarea className="form-control" rows={3} cols={60} value={discussionCommentContent} maxLength={maxCommentContentLengthRef.current}
                                        onChange={commentContentHandle} />
                                    <div className="actions">
                                        <div className="btn-shadow create" onClick={createDiscussionCommentAsync}>{t("Add")}</div>
                                        <div className="btn-shadow hide" onClick={() => setAddShowComment((item) => !item)}>{t("Cancel")}</div>
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
            <div className="edit">
                <div className="form-group">
                    <label htmlFor="title">{t("Title")}</label>
                    <div className={`content-length ${discussion.title.length === maxTitleLengthRef.current ? 'limit' : ''}`}>{currentTitleLength}/{maxTitleLengthRef.current}</div>
                    <input type="text" className="form-control" id="title" value={title} maxLength={maxTitleLengthRef.current}
                        onChange={titleHandle} />
                </div>
                <div className="form-group">
                    <label htmlFor="Content">{t("Content")}</label>
                    <div className={`content-length ${discussion.content.length === maxContentLengthRef.current ? 'limit' : ''}`}>{currentContentLength}/{maxContentLengthRef.current}</div>
                    <textarea className="form-control" id="Content" rows={8} value={content} maxLength={maxContentLengthRef.current}
                        onChange={contentHandle} />
                </div>
                <div className="actions">
                    <div className="btn-shadow create" onClick={updateDiscussionAsync}>{t("Save")}</div>
                    <div className="btn-shadow secondary" onClick={() => setEditModeOne(false)}>{t("Cancel")}</div>
                </div>
            </div>
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