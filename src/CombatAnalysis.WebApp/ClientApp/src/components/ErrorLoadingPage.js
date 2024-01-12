import { faArrowsRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../styles/loading.scss";

const ErrorLoadingPage = ({ handleRefetch }) => {
    return (
        <div className="loading-page">
            <div className="loading-page__error">
                <div className="content">
                    <div>Error loading</div>
                    <div class="alert alert-warning" role="alert">Some problem during loading page. Try refresh page or in another time</div>
                    <div className="btn-shadow" onClick={handleRefetch}>
                        <FontAwesomeIcon
                            icon={faArrowsRotate}
                        />
                        <div>Refresh</div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ErrorLoadingPage;