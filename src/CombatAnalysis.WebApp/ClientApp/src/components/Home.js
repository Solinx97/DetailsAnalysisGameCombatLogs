import { useNavigate } from 'react-router-dom';
import useAuthentificationAsync from '../hooks/useAuthentificationAsync';

const Home = () => {
    const navigate = useNavigate();

    const [, , , isAuth] = useAuthentificationAsync();

    const render = () => {
        return (
            <div>
                <div>
                    <div>Communication <span style={{ display: isAuth ? "none" : "flex" }}>(need be authorized)</span></div>
                    <button className="btn btn-info" onClick={() => navigate("/communication")} disabled={!isAuth}>Open</button>
                </div>
                <div>
                    <div>Analyzing of combats</div>
                    <button className="btn btn-info"  onClick={() => navigate("/main-information")}>Open</button>
                </div>
            </div>
        );
    }

    return render();
}

export default Home;