import { useEffect, useState } from "react";

import '../styles/alert.scss';

const Alert = ({ isVisible, typeOfAlert = 0, content = "", timeout = -1 }) => {
    const [showAlert, setShowAlert] = useState(false);

    useEffect(() => {
        if (isVisible) {
            setShowAlert(true);

            if (timeout > 0) {
                setTimeout(() => {
                    setShowAlert(false);
                }, timeout);
            }
        } else if (!isVisible && timeout === -1) {
            setShowAlert(false);
        }
    }, [isVisible])

    const getTypeOfAlert = () => {
        switch (typeOfAlert) {
            case 0:
                return "alert alert-primary";
            case 1:
                return "alert alert-secondary";
            case 2:
                return "alert alert-success";
            case 3:
                return "alert alert-danger";
            case 4:
                return "alert alert-warning";
            case 5:
                return "alert alert-info";
            case 6:
                return "alert alert-dark";
            default:
                return "alert alert-light";
        }
    }

    const render = () => {
        if (!showAlert) {
            return (<></>);
        }

        return (<div className="request-alert">
            <div className={getTypeOfAlert()} role="alert">
                {content}
            </div>
        </div>);
    }

    return render();
}

export default Alert;