import { useEffect, useRef, useState } from "react";
import { useSelector } from 'react-redux';

import "../../../styles/communication/myFeed.scss";

const MyFeed = () => {
    const user = useSelector((state) => state.user.value);

    const [posts, setPosts] = useState(<></>);
    const postContentRef = useRef(null);
    const [showCreatePost, setShowCreatePost] = useState(false);

    useEffect(() => {
        let getPost = async () => {
            await getMyPostsAsync();
        }

        getPost();
    }, [])

    const getMyPostsAsync = async () => {
        const response = await fetch(`/api/v1/Post/searchByOwnerId/${user.id}`);
        const status = response.status;

        if (status === 200) {
            const allPosts = await response.json();
            fillPosts(allPosts);
        }
    }

    const fillPosts = (allPosts) => {
        const list = allPosts.map((element) => createPostStyle(element));

        setPosts(list);
    }

    const createPostStyle = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.when}</h5>
                    <p className="card-text">{element.content}</p>
                </div>
            </div>
        </li>);
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
            await getMyPostsAsync();
        }
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
            <ul>{posts}</ul>
        </div>);
    }

    return render();
}

export default MyFeed;