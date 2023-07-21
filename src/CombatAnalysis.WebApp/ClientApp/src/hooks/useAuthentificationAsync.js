import { useEffect, useState } from "react";
import { useDispatch, useSelector } from 'react-redux';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/Customer.api';
import { useAuthenticationAsyncQuery } from '../store/api/UserApi';
import { update } from '../store/slicers/AuthSlice';
import { updateCustomer } from '../store/slicers/CustomerSlice';

const useAuthentificationAsync = () => {
    const isAuth = useSelector((state) => state.auth.value);
    const customer = useSelector((state) => state.customer.value);
    const dispatch = useDispatch();

    const [user, setUser] = useState(null);

    const authRes = useAuthenticationAsyncQuery();
    const [getCustomerAsync] = useLazySearchByUserIdAsyncQuery();

    useEffect(() => {
        const checkAuth = async () => {
            await checkAuthAsync();
        }

        if (authRes.status === 'fulfilled') {
            checkAuth();
        }
        else {
            dispatch(updateCustomer(null));
            dispatch(update(false));
        }
    }, [authRes])

    const checkAuthAsync = async () => {
        setUser(authRes.data);

        const getCustomerResult = await getCustomerAsync(authRes.data.id);
        if (getCustomerResult.data !== undefined) {
            dispatch(updateCustomer(getCustomerResult.data));
            dispatch(update(true));
        }
    }

    return [user, customer, isAuth];
}

export default useAuthentificationAsync;