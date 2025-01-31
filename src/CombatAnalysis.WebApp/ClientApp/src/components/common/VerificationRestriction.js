import { faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";

import "../../styles/common/verificationRestriction.scss";

const VerificationRestriction = ({ contentText, infoText }) => {
    const [showForbiddenInfo, setShowForbiddenInfo] = useState(false);

    return (
        <div className="forbidden">
            <div className="forbidden__content">{contentText}</div>
            <FontAwesomeIcon
                icon={faCircleQuestion}
                className={showForbiddenInfo ? "forbidden__info_active" : "forbidden__info_hide"}
                onClick={() => setShowForbiddenInfo((prev) => !prev)}
            />
            {showForbiddenInfo &&
                <div className="forbidden__info">{infoText}</div>
            }
        </div>
    );
}

export default VerificationRestriction;