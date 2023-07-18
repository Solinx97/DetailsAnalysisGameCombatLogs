import { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom';
import { useAuthenticationAsyncQuery } from '../store/api/UserApi';
import { useSearchByUserIdAsyncQuery } from '../store/api/Customer.api';

const useAuthentificationAsync = () => {
    const auth = useAuthenticationAsyncQuery();
    const customer = useSearchByUserIdAsyncQuery(auth.data?.id);

    const [isAuth, setIsAuth] = useState(false);

    const navigate = useNavigate();

    useEffect(() => {
        if (auth.status === 'fulfilled' && auth.data !== null) {
            setIsAuth(true);
        }
        else if (auth.status === 'rejected' || auth.data === null) {
            setIsAuth(false);
            navigate("/");
        }
    }, [auth])

    return [auth.data, customer.data, setIsAuth, isAuth];
}

export default useAuthentificationAsync;