
const LoginPreview = () => {
    return (
        <div className="preview">
            <p>Account open new features:</p>
            <div className="preview__responsibilities">
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="login-reason-1" id="login-reason-1" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="login-reason-1">Explore feed</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="login-reason-2" id="login-reason-2" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="login-reason-2">Explore community</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="login-reason-3" id="login-reason-3" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="login-reason-3">Create posts and messages in chat</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="login-reason-4" id="login-reason-4" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="login-reason-4">Save private Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="login-reason-5" id="login-reason-5" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="login-reason-5">Share Combat logs with your friends</label>
                </div>
            </div>
        </div>
    );
}

export default LoginPreview;