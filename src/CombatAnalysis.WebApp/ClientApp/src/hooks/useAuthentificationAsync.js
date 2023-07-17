import { useEffect } from "react";
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { checkAuth } from '../features/AuthenticationReducer';
import { customerUpdate } from '../features/CustomerReducer';
import { userUpdate } from '../features/UserReducer';
import AuthenticationService from '../services/AuthenticationService';
import CustomerService from '../services/CustomerService';

const useAuthentificationAsync = () => {
    const authenticationService = new AuthenticationService();
    const customerService = new CustomerService();

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
        const currentUser = await authenticationService.authenticationAsync();
        if (currentUser !== null) {
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
        const customer = await customerService.searchByUserIdAsync(userId);
        customer !== null
            ? dispatch(customerUpdate(customer))
            : dispatch(checkAuth(false));
    }

    return checkAuthAsync;
}

export default useAuthentificationAsync;