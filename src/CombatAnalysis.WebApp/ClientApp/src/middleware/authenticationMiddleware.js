import { isRejectedWithValue } from '@reduxjs/toolkit';
import { updateCustomer } from '../store/slicers/CustomerSlice';

const statusCode = {
    notAuthorized: 401
};

const pageWithoutAuth = ["/", "/login", "/registration", "/main-information", "/general-analysis", "/details-specifical-combat", "/combat-general-details"];
const unautorizedRedirectTo = "/";

const authenticationMiddleware = (store) => (next) => (action) => {
    const pathName = window.location.pathname;

    if (!pageWithoutAuth.includes(pathName) && isRejectedWithValue(action)) {
        if (action.payload.status === statusCode["notAuthorized"]) {
            store.dispatch(updateCustomer(null));
            window.location.href = unautorizedRedirectTo + "?shouldBeAuthorize";
        }
    }

    return next(action);
};

export default authenticationMiddleware;