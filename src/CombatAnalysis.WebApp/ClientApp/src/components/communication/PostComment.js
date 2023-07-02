import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useState } from 'react';

import "../../styles/communication/postComment.scss";

const PostComment = ({ dateFormatting, customerId, postId, updatePostAsync }) => {
    const [commentsList, setCommentsList] = useState(<></>);
    const [postCommentContent, setPostCommentContent] = useState("");

    useEffect(() => {
        let getPostCommentsByPostId = async () => {
            await getPostCommentsByPostIdAsync();
        }

        getPostCommentsByPostId();
    }, [])

    const getPostCommentsByPostIdAsync = async (editablePostCommentId = 0) => {
        const response = await fetch(`/api/v1/PostComment/searchByPostId/${postId}`);

        if (response.status === 200) {
            const allComments = await response.json();
            for (var i = 0; i < allComments.length; i++) {
                const customer = await getCustomerByIdAsync(allComments[i].ownerId);
                allComments[i].username = customer.username;
            }

            fillPostComments(allComments, editablePostCommentId);
        }
    }


    const getCustomerByIdAsync = async (customerId) => {
        const response = await fetch(`/api/v1/Customer/${customerId}`);

        if (response.status === 200) {
            let customer = await response.json();
            return customer;
        }

        return null;
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            id: 0,
            content: postCommentContent,
            when: new Date(),
            postId: postId,
            ownerId: customerId
        }

        const response = await fetch("/api/v1/PostComment", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newPostComment)
        });

        if (response.status === 200) {
            setPostCommentContent("");

            await updatePostAsync(postId, 0, 0, 1);
            await getPostCommentsByPostIdAsync();
        }
    }

    const updatePostCommentAsync = async (postComment) => {
        const response = await fetch("/api/v1/PostComment", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(postComment)
        });

        if (response.status === 200) {
            await getPostCommentsByPostIdAsync(postComment.postId);
        }
    }

    const deletePostCommentAsync = async (postCommentId) => {
        const response = await fetch(`/api/v1/PostComment/${postCommentId}`, {
            method: 'DELETE'
        });

        if (response.status === 200) {
            await updatePostAsync(postId, 0, 0, -1);
            await getPostCommentsByPostIdAsync();
        }
    }

    const fillPostComments = (allComments, editablePostCommentId) => {
        const list = allComments.map((element) => createPostComment(element, editablePostCommentId));

        setCommentsList(list);
    }

    const createPostComment = (element, editablePostCommentId) => {
        let editCommentElements = (<></>);
        if (editablePostCommentId === element.id) {
            editCommentElements = <>
                <textarea rows="1" cols="57" onChange={e => element.content = e.target.value} defaultValue={element.content} />
                <div>
                    <button type="button" className="btn btn-outline-info" onClick={async () => await updatePostCommentAsync(element)}>Save</button>
                    <button type="button" className="btn btn-outline-warning" onClick={async () => await getPostCommentsByPostIdAsync(0)}>Cancel</button>
                </div>
            </>;
        }
        else {
            editCommentElements = <div className="card-text">{element.content}</div>;
        }

        return (<li key={element.id} className="card">
            <ul className="list-group list-group-flush">
                <li className="post-comments__title list-group-item">
                    <div className="card-title">{element.username}</div>
                    <div className="card-title">{dateFormatting(element.when)}</div>
                    {element.ownerId === customerId
                        ? (<div className="post-comments__menu">
                            <FontAwesomeIcon icon={faPen} title="Edit" onClick={async () => await getPostCommentsByPostIdAsync(element.id)} />
                            <FontAwesomeIcon icon={faTrash} title="Remove" onClick={async () => await deletePostCommentAsync(element.id)} />
                        </div>)
                        : null
                    }
                </li>
                <li className="post-comments__edit list-group-item">
                    {editCommentElements}
                </li>
            </ul>
        </li>);
    }

    const render = () => {
        return (<>
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
        </>);
    }

    return render();
}

export default memo(PostComment);