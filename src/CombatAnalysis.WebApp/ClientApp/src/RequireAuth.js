import { Navigate } from "react-router-dom";
import { useLazyAuthenticationAsyncQuery } from './store/api/UserApi';

const RequireAuth = ({ children }) => {
    const [checkAuth] = useLazyAuthenticationAsyncQuery();

    const checkAuthAsync = () => {
        const result = checkAuth();
        return result.error !== undefined;
    }

    if (checkAuthAsync()) {
        return <Navigate to='/' />
    }

    return children;
}

export default RequireAuth;