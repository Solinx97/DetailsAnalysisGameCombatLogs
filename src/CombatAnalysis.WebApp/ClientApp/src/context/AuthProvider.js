import React, { createContext, useContext, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLogoutAsyncMutation } from '../store/api/Account.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/Customer.api';
import { useLazyAuthenticationAsyncQuery } from '../store/api/UserApi';
import { useLazyGetUserPrivacyQuery } from '../store/api/Identity.api';
import { updateCustomer } from '../store/slicers/CustomerSlice';
import { updateUser } from '../store/slicers/UserSlice';
import { updateUserPrivacy } from '../store/slicers/UserPrivacySlice';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const [getAuthAsync] = useLazyAuthenticationAsyncQuery();
    const [logoutAsyncMut] = useLogoutAsyncMutation();
    const [getCustomerAsync] = useLazySearchByUserIdAsyncQuery();

    const [getUserPrivacyAsyncMut] = useLazyGetUserPrivacyQuery();

    const checkAuthAsync = async () => {
        const response = await getAuthAsync();
        if (response.error !== undefined || !response.data) {
            dispatch(updateCustomer(null));

            return;
        }

        dispatch(updateUser(response.data));
        await getCustomerDataAsync(response.data?.id);

        await getUserPrivacyAsync(response.data.identityUserId);
    }

    const getCustomerDataAsync = async (userId) => {
        const response = await getCustomerAsync(userId);
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

        navigate("/");

        await logoutAsyncMut();
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, checkAuthAsync, logoutAsync, getCustomerDataAsync }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);