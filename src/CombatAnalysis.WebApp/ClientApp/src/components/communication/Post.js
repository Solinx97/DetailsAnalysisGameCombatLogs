import { faHeart, faMessage, faThumbsDown, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import CustomerService from '../../services/CustomerService';
import PostDislikeService from '../../services/PostDislikeService';
import PostLikeService from '../../services/PostLikeService';
import PostService from '../../services/PostService';
import { useGetPostByIdQuery } from '../../store/api/Post.api';
import PostComment from './PostComment';
import UserInformation from './UserInformation';

import '../../styles/communication/post.scss';

const Post = ({ customer, targetPostType }) => {
    const postService = new PostService();
    const customerService = new CustomerService();
    const postLikeService = new PostLikeService();
    const postDislikeService = new PostDislikeService();

    const { data: post, isLoading } = useGetPostByIdQuery(targetPostType?.postId);

    const [userInformation, setUserInformation] = useState(<></>);
    const [showComments, setShowComments] = useState(false);
    const [selectedPostCommentId, setSelectedPostCommentId] = useState(0);
    const [selectedPostCustomerId, setSelectedPostCustomerId] = useState(0);

    const username = "Oleg";

    const getPostByIdAsync = async (postId) => {
        const post = await postService.getByIdAsync(postId);
        return post;
    }

    const getCustomerByIdAsync = async (customerId) => {
        const customer = await customerService.getByIdAsync(customerId);
        return customer;
    }

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        const post = await getPostByIdAsync(postId);
        if (post === null) {
            return;
        }

        const postForUpdate = {
            id: post.id,
            content: post.content,
            when: post.when,
            likeCount: post.likeCount + likesCount,
            dislikeCount: post.dislikeCount + dislikesCount,
            commentCount: post.commentCount + commentsCount,
            ownerId: post.ownerId
        }

        await postService.updateAsync(postForUpdate);
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

        const createdPostLike = await postLikeService.createAsync(newPostLike);
        if (createdPostLike !== null) {
            await updatePostAsync(postId, 1, 0, 0);
        }
    }

    const getPostLikesAsync = async (postId) => {
        const postLikes = await PostLikeService.searchByPostIdAsync(postId);
        if (postLikes !== null) {
            return await checkIfPostLikeExistAsync(postLikes);
        }

        return false;
    }

    const checkIfPostLikeExistAsync = async (postLikes) => {
        for (let i = 0; i < postLikes.length; i++) {
            if (postLikes[i].ownerId === customer.id) {
                await deletePostLikeAsync(postLikes[i].postId, postLikes[i].id);
                return true;
            }
        }

        return false;
    }

    const deletePostLikeAsync = async (postId, postLikeId) => {
        const deletedItem = await postLikeService.deleteAsync(postLikeId);
        if (deletedItem !== null) {
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

        const createdPostDislike = await postDislikeService.createAsync(newPostDislike);
        if (createdPostDislike !== null) {
            await updatePostAsync(postId, 0, 1, 0);
        }
    }

    const getPostDislikesAsync = async (postId) => {
        const postDislikes = await postDislikeService.searchByPostIdAsync(postId);
        if (postDislikes !== null) {
            return await checkIfPostDislikeExistAsync(postDislikes);
        }

        return false;
    }

    const checkIfPostDislikeExistAsync = async (postDislikes) => {
        for (let i = 0; i < postDislikes.length; i++) {
            if (postDislikes[i].ownerId === customer.id) {
                await deletePostDislikeAsync(postDislikes[i].postId, postDislikes[i].id);
                return true;
            }
        }

        return false;
    }

    const deletePostDislikeAsync = async (postId, postDislikeId) => {
        const deletedItem = await postDislikeService.deleteAsync(postDislikeId);
        if (deletedItem !== null) {
            await updatePostAsync(postId, 0, -1, 0);
        }
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

    if (isLoading) {
        return <></>;
    }

    return (
        <div>
            <div className="card">
                <ul className="list-group list-group-flush">
                    <li className="posts__title list-group-item">
                        <div className="posts__title-username">
                            <div>{username}</div>
                            <FontAwesomeIcon icon={faWindowRestore} title="Show details" onClick={async () => await userInformationHandlerAsync(post)} />
                        </div>
                        <div>{dateFormatting(post.when)}</div>
                    </li>
                    {selectedPostCustomerId === post.id &&
                        userInformation
                    }
                    <li className="list-group-item">
                        <div className="card-text">{post.content}</div>
                    </li>
                    <li className="posts__reaction list-group-item">
                        <div className="posts__reaction item">
                            <FontAwesomeIcon className="post__reaction_like" icon={faHeart} title="Like"
                                onClick={async () => await createPostLikeAsync(post.id)} />
                            <div className="count">{post.likeCount}</div>
                        </div>
                        <div className="posts__reaction item">
                            <FontAwesomeIcon className="post__reaction_dislike" icon={faThumbsDown} title="Dislike"
                                onClick={async () => await createPostDislikeAsync(post.id)} />
                            <div className="count">{post.dislikeCount}</div>
                        </div>
                        <div className="posts__reaction item">
                            <FontAwesomeIcon icon={faMessage} title="Comment"
                                onClick={() => postCommentsHandler(post.id)} />
                            <div className="count">{post.commentCount}</div>
                        </div>
                    </li>
                </ul>
            </div>
            {showComments && selectedPostCommentId === post.id &&
                <PostComment dateFormatting={dateFormatting} customerId={customer.id} postId={post.id} updatePostAsync={updatePostAsync} />
            }
        </div>
    );
}

export default Post;