import { useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { useState } from 'react';
import { useEffect } from 'react';

const Home = () => {
    const navigate = useNavigate();
    const isAuth = useSelector((state) => state.authentication.value);
    const [showAuthNotification, setShowAuthNotification] = useState(false);

    useEffect(() => {
        setShowAuthNotification(!isAuth);
    }, [isAuth])

    const render = () => {
        return (<div>
            <div>
                <div>Communication <span style={{ display: showAuthNotification ? "flex" : "none" }}>(need be authorized)</span></div>
                <button className="btn btn-info" onClick={() => navigate("/communication")} disabled={isAuth ? false : true}>Open</button>
            </div>
            <div>
                <div>Analyzing of combats</div>
                <button className="btn btn-info"  onClick={() => navigate("/main-information")}>Open</button>
            </div>
        </div>);
    }

    return render();
}

export default Home;