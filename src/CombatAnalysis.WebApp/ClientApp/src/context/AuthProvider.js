﻿import React, { createContext, useContext, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLogoutAsyncMutation } from '../store/api/Account.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/Customer.api';
import { useLazyAuthenticationAsyncQuery } from '../store/api/UserApi';
import { updateCustomer } from '../store/slicers/CustomerSlice';
import { updateUser } from '../store/slicers/UserSlice';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const [getAuthAsync] = useLazyAuthenticationAsyncQuery();
    const [logoutAsyncMut] = useLogoutAsyncMutation();
    const [getCustomerAsync] = useLazySearchByUserIdAsyncQuery();

    const checkAuthAsync = async () => {
        const auth = await getAuthAsync();
        if (auth.error !== undefined || !auth.data) {
            dispatch(updateCustomer(null));

            return;
        }

        dispatch(updateUser(auth.data));
        await getCustomerDataAsync(auth.data?.id);
    }

    const getCustomerDataAsync = async (userId) => {
        const customer = await getCustomerAsync(userId);
        if (customer.error !== undefined) {
            return;
        }

        dispatch(updateCustomer(customer.data));

        setIsAuthenticated(true);
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