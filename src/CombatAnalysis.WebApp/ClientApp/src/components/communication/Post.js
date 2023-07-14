import { faArrowsRotate, faHeart, faMessage, faThumbsDown, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useSelector } from 'react-redux';
import PostComment from './PostComment';
import UserInformation from './UserInformation';

import '../../styles/communication/post.scss';

const Post = ({ selectedPostsType, createPostAsync, updatePostsAsync, setShowUpdatingAlert }) => {
    const customer = useSelector((state) => state.customer.value);

    const postContentRef = useRef(null);

    const [posts, setPosts] = useState(<></>);
    const [userInformation, setUserInformation] = useState(<></>);
    const [showCreatePost, setShowCreatePost] = useState(false);
    const [showComments, setShowComments] = useState(false);
    const [selectedPostCommentId, setSelectedPostCommentId] = useState(0);
    const [selectedPostCustomerId, setSelectedPostCustomerId] = useState(0);

    useEffect(() => {
        let getPosts = async () => {
            await getPostsAsync();
        }

        setShowUpdatingAlert(true);
        getPosts();
    }, [selectedPostsType])

    useEffect(() => {
        if (posts.key === undefined) {
            setShowUpdatingAlert(false);
        }
    }, [posts])

    const getPostsAsync = async () => {
        const searchedPosts = [];
        for (let i = 0; i < selectedPostsType.length; i++) {
            const post = await getPostByIdAsync(selectedPostsType[i].postId);
            const customer = await getCustomerByIdAsync(post.ownerId);
            post.username = customer.username;
            searchedPosts.push(post);
        }

        fillPosts(searchedPosts);
    }

    const getPostByIdAsync = async (postId) => {
        const response = await fetch(`/api/v1/Post/${postId}`);

        if (response.status === 200) {
            let post = await response.json();

            return post;
        }

        return null;
    }

    const getCustomerByIdAsync = async (customerId) => {
        const response = await fetch(`/api/v1/Customer/${customerId}`);

        if (response.status === 200) {
            const customer = await response.json();
            return customer;
        }

        return null;
    }

    const handleCreatePostAsync = async () => {
        const isCreated = await createPostAsync(postContentRef.current.value);
        if (isCreated) {
            postContentRef.current.value = "";
            setShowCreatePost(false);

            await getPostsAsync();
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
            await getPostsAsync();
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
            ownerId: customer.id
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
            if (postLikes[i].ownerId === customer.id) {
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
            ownerId: customer.id
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
            if (postDislikes[i].ownerId === customer.id) {
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
        const list = allPosts.reverse().map((element) => createPostCard(element));

        setPosts(list);
    }

    const postCommentsHandler = (postId) => {
        setShowComments((item) => !item);
        if (selectedPostCommentId !== postId) {
            setShowComments(true);
        }

        setSelectedPostCommentId(postId);
    }

    const userInformationHandlerAsync = async (post) => {
        const customer = await getCustomerByIdAsync(post.ownerId);
        setUserInformation(<UserInformation customer={customer} closeUserInformation={closeUserInformation} />);

        setSelectedPostCustomerId(post.id);
    }

    const closeUserInformation = () => {
        setUserInformation(<></>);
        setSelectedPostCustomerId(0);
    }

    const createPostCard = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <ul className="list-group list-group-flush">
                    <li className="posts__title list-group-item">
                        <div className="posts__title-username">
                            <div>{element.username}</div>
                            <FontAwesomeIcon icon={faWindowRestore} title="Show details" onClick={async () => await userInformationHandlerAsync(element)} />
                        </div>
                        <div>{dateFormatting(element.when)}</div>
                    </li>
                    {selectedPostCustomerId === element.id &&
                        userInformation
                    }
                    <li className="list-group-item">
                        <div className="card-text">{element.content}</div>
                    </li>
                    <li className="posts__reaction list-group-item">
                        <div className="posts__reaction item">
                            <FontAwesomeIcon className="post__reaction_like" icon={faHeart} title="Like"
                                onClick={async () => await createPostLikeAsync(element.id)} />
                            <div className="count">{element.likeCount}</div>
                        </div>
                        <div className="posts__reaction item">
                            <FontAwesomeIcon className="post__reaction_dislike" icon={faThumbsDown} title="Dislike"
                                onClick={async () => await createPostDislikeAsync(element.id)} />
                            <div className="count">{element.dislikeCount}</div>
                        </div>
                        <div className="posts__reaction item">
                            <FontAwesomeIcon icon={faMessage} title="Comment"
                                onClick={() => postCommentsHandler(element.id)} />
                            <div className="count">{element.commentCount}</div>
                        </div>
                    </li>
                </ul>
            </div>
            {showComments && selectedPostCommentId === element.id &&
                <PostComment dateFormatting={dateFormatting} customerId={customer.id} postId={element.id} updatePostAsync={updatePostAsync} />
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

    const handleUpdatePostsAsync = async () => {
        setShowUpdatingAlert(true);
        await updatePostsAsync();
    }

    const render = () => {
        return (<div>
            <div className="create-post">
                <div>
                    <div className="create-post__tool" style={{ display: !showCreatePost ? "flex" : "none" }}>
                        <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" onClick={async () => handleUpdatePostsAsync()} />
                        <button type="button" className="btn btn-outline-info" onClick={() => setShowCreatePost((item) => !item)}>New post</button>
                    </div>
                    <div style={{ display: showCreatePost ? "flex" : "none" }} className="create-post__create-tool">
                        <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" onClick={async () => updatePostsAsync()} />
                        <button type="button" className="btn btn-outline-warning" onClick={() => setShowCreatePost((item) => !item)}>Cancel</button>
                        <button type="button" className="btn btn-outline-success" onClick={async () => await handleCreatePostAsync()}>Create</button>
                    </div>
                </div>
                <textarea rows="5" cols="100" ref={postContentRef} style={{ display: showCreatePost ? "flex" : "none" }} />
            </div>
            <ul className="posts">{posts}</ul>
        </div>);
    }

    return render();
}

export default Post;