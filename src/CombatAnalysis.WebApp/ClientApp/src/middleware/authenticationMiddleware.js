import { isRejectedWithValue } from '@reduxjs/toolkit';
import { updateCustomer } from '../store/slicers/CustomerSlice';

const statusCode = {
    notAuthorized: 401
};

const unautorizedRedirectTo = "/login";

const authenticationMiddleware = (store) => (next) => (action) => {
    if (isRejectedWithValue(action)) {
        if (action.payload.status === statusCode["notAuthorized"]) {
            store.dispatch(updateCustomer(null));
            window.location.href = unautorizedRedirectTo;
        }
    }

    return next(action);
};

export default authenticationMiddleware;