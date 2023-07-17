import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useState } from 'react';
import CustomerService from '../../services/CustomerService';
import PostCommentService from '../../services/PostCommentService';

import '../../styles/communication/postComment.scss';

const PostComment = ({ dateFormatting, customerId, postId, updatePostAsync }) => {
    const postCommentService = new PostCommentService();
    const customerService = new CustomerService();

    const [commentsList, setCommentsList] = useState(<></>);
    const [postCommentContent, setPostCommentContent] = useState("");

    useEffect(() => {
        let getPostCommentsByPostId = async () => {
            await getPostCommentsByPostIdAsync();
        }

        getPostCommentsByPostId();
    }, [])

    const getPostCommentsByPostIdAsync = async (editablePostCommentId = 0) => {
        const allComments = await postCommentService.searchByPostIdAsync(postId);
        if (allComments === null) {
            return;
        }

        for (let i = 0; i < allComments.length; i++) {
            const customer = await getCustomerByIdAsync(allComments[i].ownerId);
            allComments[i].username = customer.username;
        }

        fillPostComments(allComments, editablePostCommentId);
    }

    const getCustomerByIdAsync = async (customerId) => {
        const customer = await customerService.getByIdAsync(customerId);
        return customer;
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            id: 0,
            content: postCommentContent,
            when: new Date(),
            postId: postId,
            ownerId: customerId
        }

        const createdPostComment = await postCommentService.createAsync(newPostComment);
        if (createdPostComment !== null) {
            setPostCommentContent("");

            await updatePostAsync(postId, 0, 0, 1);
            await getPostCommentsByPostIdAsync();
        }
    }

    const updatePostCommentAsync = async (postComment) => {
        const updatedPostComment = await postCommentService.updateAsync(postComment);
        if (updatedPostComment !== null) {
            await getPostCommentsByPostIdAsync(postComment.postId);
        }
    }

    const deletePostCommentAsync = async (postCommentId) => {
        const deletedItem = await postCommentService.deleteAsync(postCommentId);
        if (deletedItem !== null) {
            await updatePostAsync(postId, 0, 0, -1);
            await getPostCommentsByPostIdAsync();
        }
    }

    const fillPostComments = (allComments, editablePostCommentId) => {
        const list = allComments.map((element) => createPostComment(element, editablePostCommentId));

        setCommentsList(list);
    }

    const createPostComment = (postComment, editablePostCommentId) => {
        let editCommentElements = (<></>);
        if (editablePostCommentId === postComment.id) {
            editCommentElements = <>
                <textarea rows="1" cols="57" onChange={e => postComment.content = e.target.value} defaultValue={postComment.content} />
                <div>
                    <button type="button" className="btn btn-outline-info" onClick={async () => await updatePostCommentAsync(postComment)}>Save</button>
                    <button type="button" className="btn btn-outline-warning" onClick={async () => await getPostCommentsByPostIdAsync(0)}>Cancel</button>
                </div>
            </>;
        }
        else {
            editCommentElements = <div className="card-text">{postComment.content}</div>;
        }

        return (
            <li key={postComment.id} className="card">
                <ul className="list-group list-group-flush">
                    <li className="post-comments__title list-group-item">
                        <div className="card-title">{postComment.username}</div>
                        <div className="card-title">{dateFormatting(postComment.when)}</div>
                        {postComment.ownerId === customerId
                            ? (<div className="post-comments__menu">
                                <FontAwesomeIcon icon={faPen} title="Edit" onClick={async () => await getPostCommentsByPostIdAsync(postComment.id)} />
                                <FontAwesomeIcon icon={faTrash} title="Remove" onClick={async () => await deletePostCommentAsync(postComment.id)} />
                            </div>)
                            : null
                        }
                    </li>
                    <li className="post-comments__edit list-group-item">
                        {editCommentElements}
                    </li>
                </ul>
            </li>
        );
    }

    const render = () => {
        return (
            <div>
                <ul className="post-comments">
                    {commentsList}
                </ul>
                <div className="add-new-comment">
                    <div className="add-new-comment__title">
                        <div>Add comment:</div>
                    </div>
                    <textarea rows="1" cols="75" onChange={e => setPostCommentContent(e.target.value)} value={postCommentContent} />
                    <button type="button" className="btn btn-outline-info" onClick={async () => await createPostCommentAsync()}>Add</button>
                </div>
            </div>
        );
    }

    return render();
}

export default memo(PostComment);