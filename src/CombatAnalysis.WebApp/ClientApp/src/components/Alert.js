import { useEffect, useState } from "react";

import '../styles/alert.scss';

const Alert = ({ isVisible }) => {
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(false);

    useEffect(() => {
        isVisible ? setShowUpdatingAlert(true) : setShowUpdatingAlert(false)
    }, [isVisible])

    const render = () => {
        return (<div>
            {showUpdatingAlert
                ? <div className="request-alert">
                    <div className="alert alert-primary" role="alert">
                        Updating...
                    </div>
                </div>
                : null
            }
        </div>);
    }

    return render();
}

export default Alert;