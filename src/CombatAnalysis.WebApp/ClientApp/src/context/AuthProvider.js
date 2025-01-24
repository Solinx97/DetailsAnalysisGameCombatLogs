import React, { createContext, useContext, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLazyAuthenticationQuery } from '../store/api/core/User.api';
import { useLogoutAsyncMutation } from '../store/api/user/Account.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/user/Customer.api';
import { useLazyGetUserPrivacyQuery } from '../store/api/user/Identity.api';
import { updateCustomer } from '../store/slicers/CustomerSlice';
import { updateUserPrivacy } from '../store/slicers/UserPrivacySlice';
import { updateUser } from '../store/slicers/UserSlice';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const [getAuth] = useLazyAuthenticationQuery();
    const [logout] = useLogoutAsyncMutation();
    const [getCustomer] = useLazySearchByUserIdAsyncQuery();

    const [getUserPrivacyAsyncMut] = useLazyGetUserPrivacyQuery();

    const checkAuthAsync = async () => {
        const response = await getAuth();
        if (response.error !== undefined || !response.data) {
            dispatch(updateUser(null));
            dispatch(updateCustomer(null));
            dispatch(updateUserPrivacy(null));

            return;
        }

        dispatch(updateUser(response.data));
        await getCustomerDataAsync(response.data?.id);

        await getUserPrivacyAsync(response.data.identityUserId);
    }

    const getCustomerDataAsync = async (userId) => {
        const response = await getCustomer(userId);
        if (response.error !== undefined) {
            return;
        }

        dispatch(updateCustomer(response.data));

        setIsAuthenticated(true);
    }

    const getUserPrivacyAsync = async (id) => {
        const response = await getUserPrivacyAsyncMut(id);
        if (response.error !== undefined) {
            return;
        }

        dispatch(updateUserPrivacy(response.data));
    }

    const logoutAsync = async () => {
        setIsAuthenticated(false);
        dispatch(updateCustomer(null));
        dispatch(updateUser(null));
        dispatch(updateUserPrivacy(null));

        navigate("/");

        await logout();
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, checkAuthAsync, logoutAsync }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);