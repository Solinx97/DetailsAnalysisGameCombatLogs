import { useEffect } from "react";
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { checkAuth } from '../features/AuthenticationReducer';
import { customerUpdate } from '../features/CustomerReducer';
import { userUpdate } from '../features/UserReducer';

const useAuthentificationAsync = () => {
    const user = useSelector((state) => state.user.value);

    const dispatch = useDispatch();
    const navigate = useNavigate();

    useEffect(() => {
        if (user === null) {
            return;
        }

        const checkAuth = async () => {
            await checkAuthAsync();
        }

        checkAuth();
    }, []);

    const checkAuthAsync = async () => {
        const response = await fetch("api/v1/Authentication");

        if (response.status === 200) {
            const currentUser = await response.json();

            dispatch(userUpdate(currentUser));
            dispatch(checkAuth(true));

            await getCustomerByUserIdAsync(currentUser.id);
        }
        else {
            dispatch(checkAuth(false));

            navigate("/");
        }
    }

    const getCustomerByUserIdAsync = async (userId) => {
        const response = await fetch(`api/v1/Customer/searchByUserId/${userId}`);

        if (response.status === 200
            || response.status === 204) {
            const customer = await response.json();

            dispatch(customerUpdate(customer));
        }
        else {
            dispatch(checkAuth(false));
        }
    }

    return checkAuthAsync;
}

export default useAuthentificationAsync;