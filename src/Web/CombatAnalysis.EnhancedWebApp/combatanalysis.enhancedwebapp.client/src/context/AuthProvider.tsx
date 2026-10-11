import { useLazyFindCustomerByUserIdQuery } from '@/features/user/api/Customer.api';
import { useLazyGetUserPrivacyQuery, useLazyRefreshTokenQuery, useLogoutMutation } from '@/features/user/api/Identity.api';
import { useLazyAuthenticationQuery } from '@/features/user/api/User.api';
import { updateCustomer } from '@/features/user/store/CustomerSlice';
import { updateUserPrivacy } from '@/features/user/store/UserPrivacySlice';
import { updateUser } from '@/features/user/store/UserSlice';
import { AuthContext } from '@/shared/hooks/useAuth';
import logger from '@/utils/Logger';
import { useState, type ReactNode } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [authInProgress, setAuthInProgress] = useState(false);

    const [getAuth] = useLazyAuthenticationQuery();
    const [logout] = useLogoutMutation();
    const [findCustomer] = useLazyFindCustomerByUserIdQuery();

    const [getUserPrivacy] = useLazyGetUserPrivacyQuery();
    const [refreshToken] = useLazyRefreshTokenQuery();

    const checkAuthAsync = async () => {
        try {
            setAuthInProgress(true);

            const response = await getAuth();
            if (response.data !== undefined) {
                const user = response.data;

                dispatch(updateUser(user));

                await getCustomerDataAsync(user.id);
                await getUserPrivacyAsync(user.identityUserId);

                setIsAuthenticated(true);
            }
            else {
                await refreshTokenAsync();
            }

            setAuthInProgress(false);
        } catch (e) {
            logger.error("Failed to check auth", e);

            dispatch(updateUser(null));
            dispatch(updateCustomer(null));
            dispatch(updateUserPrivacy(null));

            setAuthInProgress(false);
        }
    }

    const refreshTokenAsync = async () => {
        try {
            await refreshToken().unwrap();

            const user = await getAuth().unwrap();

            dispatch(updateUser(user));

            await getCustomerDataAsync(user.id);
            await getUserPrivacyAsync(user.identityUserId);

            setIsAuthenticated(true);
        } catch (e) {
            logger.error("Failed to refresh Auth token", e);

            dispatch(updateUser(null));
            dispatch(updateCustomer(null));
            dispatch(updateUserPrivacy(null));
        }
    }

    const getCustomerDataAsync = async (userId: string) => {
        try {
            const customer = await findCustomer(userId).unwrap();

            dispatch(updateCustomer(customer));
        } catch (e) {
            logger.error("Failed to receive customer data", e);
        }
    }

    const getUserPrivacyAsync = async (id: string) => {
        try {
            const userPrivacy = await getUserPrivacy(id).unwrap();

            dispatch(updateUserPrivacy(userPrivacy));
        } catch (e) {
            logger.error("Failed to receive user privacy data", e);
        }
    }

    const logoutAsync = async () => {
        setIsAuthenticated(false);
        dispatch(updateCustomer(null));
        dispatch(updateUser(null));
        dispatch(updateUserPrivacy(null));

        await logout();

        navigate("/");
    };

    return (
        <AuthContext.Provider value={{ 
                isAuthenticated, 
                authInProgress,
                checkAuthAsync, 
                logoutAsync
             }}>
            {children}
        </AuthContext.Provider>
    );
}

export default AuthProvider;