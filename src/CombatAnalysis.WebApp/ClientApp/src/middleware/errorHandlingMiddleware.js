import { toast } from 'react-toastify';

const errorHandlingMiddleware = (store) => (next) => (action) => {
    if (action.error) {
        toast.error(`Error: ${action.error.message || 'An unexpected error occurred'}`, {
            position: "top-right",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
    }

    return next(action);
}

export default errorHandlingMiddleware;