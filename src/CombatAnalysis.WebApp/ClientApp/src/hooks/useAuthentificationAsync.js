import { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/Customer.api';
import { useAuthenticationAsyncQuery } from '../store/api/UserApi';

const useAuthentificationAsync = () => {
    const authQuery = useAuthenticationAsyncQuery();
    const [ getCustomerAsync ] = useLazySearchByUserIdAsyncQuery();

    const navigate = useNavigate();

    const [isAuth, setIsAuth] = useState(false);
    const [customer, setCustomer] = useState(null);
    const [user, setUser] = useState(null);

    useEffect(() => {
        const auth = async () => {
            if (authQuery.status === 'fulfilled') {
                setUser(authQuery.data);

                const getCustomerResult = await getCustomerAsync(authQuery.data.id);
                if (getCustomerResult.status === 'fulfilled') {
                    setCustomer(getCustomerResult.data);
                    setIsAuth(true);
                }
            }
            else if (authQuery.status === 'rejected') {
                setIsAuth(false);
                navigate('/');
            }
        }
        auth();
    }, [authQuery])

    return [user, customer, setIsAuth, isAuth];
}

export default useAuthentificationAsync;