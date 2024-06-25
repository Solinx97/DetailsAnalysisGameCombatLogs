import { useEffect } from 'react';

const usePollNewPosts = (pollingInterval, checkNewPostsAsync) => {
    useEffect(() => {
        const interval = setInterval(() => {
            checkNewPostsAsync();
        }, pollingInterval);

        return () => clearInterval(interval);
    }, [pollingInterval, checkNewPostsAsync]);
}

export default usePollNewPosts;