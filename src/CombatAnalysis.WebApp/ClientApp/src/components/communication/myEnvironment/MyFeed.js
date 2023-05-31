import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useSelector } from 'react-redux';
import PostComment from './PostComment';

import "../../../styles/communication/myFeed.scss";

const MyFeed = () => {
    const user = useSelector((state) => state.user.value);

    const postContentRef = useRef(null);
    const [posts, setPosts] = useState(<></>);
    const [showCreatePost, setShowCreatePost] = useState(false);
    const [showComments, setShowComments] = useState(false);
    const [selectedPostCommentId, setSelectedPostCommentId] = useState(0);

    useEffect(() => {
        let getPosts = async () => {
            await getPostsAsync();
        }

        getPosts();
    })

    const getPostsAsync = async () => {
        const response = await fetch(`/api/v1/Post/searchByOwnerId/${user.id}`);

        if (response.status === 200) {
            const allPosts = await response.json();
            fillPosts(allPosts);
        }
    }

    const getPostByIdAsync = async (postId) => {
        const response = await fetch(`/api/v1/Post/${postId}`);

        if (response.status === 200) {
            const post = await response.json();
            return post;
        }

        return null;
    }

    const createPostAsync = async () => {
        const newPost = {
            id: 0,
            content: postContentRef.current.value,
            when: new Date(),
            ownerId: user.id
        }

        const response = await fetch("/api/v1/Post", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newPost)
        });

        if (response.status === 200) {
            postContentRef.current.value = "";
            setShowCreatePost(false);
        }
    }

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        const post = await getPostByIdAsync(postId);
        if (post == null) {
            return;
        }

        const updatedPost = {
            id: post.id,
            content: post.content,
            when: post.when,
            likeCount: post.likeCount + likesCount,
            dislikeCount: post.dislikeCount + dislikesCount,
            commentCount: post.commentCount + commentsCount,
            ownerId: post.ownerId
        }

        const response = await fetch("/api/v1/Post", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedPost)
        });

        if (response.status === 200) {
            //console.log(123);
        }
    }

    const createPostLikeAsync = async (postId) => {
        const postLikeIsExist = await getPostLikesAsync(postId);
        if (postLikeIsExist) {
            return;
        }

        await getPostDislikesAsync(postId);

        const newPostLike = {
            id: 0,
            postId: postId,
            ownerId: user.id
        }

        const response = await fetch("/api/v1/PostLike", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newPostLike)
        });

        if (response.status === 200) {
            await updatePostAsync(postId, 1, 0, 0);
        }
    }

    const getPostLikesAsync = async (postId) => {
        const response = await fetch(`/api/v1/PostLike/searchByPostId/${postId}`);

        if (response.status === 200) {
            const postLikes = await response.json();
            return await checkIfPostLikeExistAsync(postLikes);
        }

        return false;
    }

    const checkIfPostLikeExistAsync = async (postLikes) => {
        for (var i = 0; i < postLikes.length; i++) {
            if (postLikes[i].ownerId === user.id) {
                await deletePostLikeAsync(postLikes[i].postId, postLikes[i].id);
                return true;
            }
        }

        return false;
    }

    const deletePostLikeAsync = async (postId, postLikeId) => {
        const response = await fetch(`/api/v1/PostLike/${postLikeId}`, {
            method: 'DELETE'
        });

        if (response.status === 200) {
            await updatePostAsync(postId, -1, 0, 0);
        }
    }

    const createPostDislikeAsync = async (postId) => {
        const postDislikeIsExist = await getPostDislikesAsync(postId);
        if (postDislikeIsExist) {
            return;
        }

        await getPostLikesAsync(postId);

        const newPostDislike = {
            id: 0,
            postId: postId,
            ownerId: user.id
        }

        const response = await fetch("/api/v1/PostDislike", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newPostDislike)
        });

        if (response.status === 200) {
            await updatePostAsync(postId, 0, 1, 0);
        }
    }

    const getPostDislikesAsync = async (postId) => {
        const response = await fetch(`/api/v1/PostDislike/searchByPostId/${postId}`);

        if (response.status === 200) {
            const postDislikes = await response.json();
            return await checkIfPostDislikeExistAsync(postDislikes);
        }

        return false;
    }

    const checkIfPostDislikeExistAsync = async (postDislikes) => {
        for (var i = 0; i < postDislikes.length; i++) {
            if (postDislikes[i].ownerId === user.id) {
                await deletePostDislikeAsync(postDislikes[i].postId, postDislikes[i].id);
                return true;
            }
        }

        return false;
    }

    const deletePostDislikeAsync = async (postId, postDislikeId) => {
        const response = await fetch(`/api/v1/PostDislike/${postDislikeId}`, {
            method: 'DELETE'
        });

        if (response.status === 200) {
            await updatePostAsync(postId, 0, -1, 0);
        }
    }

    const fillPosts = (allPosts) => {
        const list = allPosts.map((element) => createPostCard(element));

        setPosts(list);
    }

    const postCommentsHandler = (postId) => {
        setShowComments(!showComments);
        if (selectedPostCommentId !== postId) {
            setShowComments(true);
        }

        setSelectedPostCommentId(postId);
    }

    const createPostCard = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <p className="card-title">{dateFormatting(element.when)}</p>
                    </li>
                    <li className="list-group-item">
                        <div className="card-text">{element.content}</div>
                    </li>
                    <li className="post__reaction list-group-item">
                        <div className="post__reaction item">
                            <FontAwesomeIcon className="post__reaction_like" icon={faHeart} title="Like"
                                onClick={async () => await createPostLikeAsync(element.id)} />
                            <div className="count">{element.likeCount}</div>
                        </div>
                        <div className="post__reaction item">
                            <FontAwesomeIcon className="post__reaction_dislike" icon={faThumbsDown} title="Dislike"
                                onClick={async () => await createPostDislikeAsync(element.id)} />
                            <div className="count">{element.dislikeCount}</div>
                        </div>
                        <div className="post__reaction item">
                            <FontAwesomeIcon icon={faMessage} title="Comment"
                                onClick={() => postCommentsHandler(element.id)} />
                            <div className="count">{element.commentCount}</div>
                        </div>
                    </li>
                </ul>
            </div>
            {showComments && selectedPostCommentId === element.id &&
                <PostComment user={user} postId={element.id} updatePostAsync={updatePostAsync} />
            }
        </li>);
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

    const render = () => {
        return (<div>
            <div className="create-post">
                <div>
                    <button type="button" className="btn btn-outline-info" onClick={() => setShowCreatePost(true)}
                        style={{ display: !showCreatePost ? "flex" : "none" }}>New post</button>
                    <div style={{ display: showCreatePost ? "flex" : "none" }}>
                        <button type="button" className="btn btn-outline-warning" onClick={() => setShowCreatePost(false)}>Cancel</button>
                        <button type="button" className="btn btn-outline-success" onClick={async () => await createPostAsync()}>Create</button>
                    </div>
                </div>
                <textarea rows="5" cols="100" ref={postContentRef} style={{ display: showCreatePost ? "flex" : "none" }} />
            </div>
            <ul className="post">{posts}</ul>
        </div>);
    }

    return render();
}

export default MyFeed;