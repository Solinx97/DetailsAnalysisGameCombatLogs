import { useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';

const Home = () => {
    const navigate = useNavigate();
    const isAuth = useSelector((state) => state.authentication.value);

    const render = () => {
        return (<div>
            <div>
                <div>Communication (need be authorized)</div>
                <button onClick={() => navigate("/chats")} disabled={isAuth ? false : true}>Open</button>
            </div>
            <div>
                <div>Analyzing of combats</div>
                <button onClick={() => navigate("/main-information")}>Open</button>
            </div>
        </div>);
    }

    return render();
}

export default Home;