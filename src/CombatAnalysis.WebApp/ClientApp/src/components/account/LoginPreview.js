
const LoginPreview = () => {
    return (
        <div className="preview">
            <p>Account open new features:</p>
            <div className="preview__responsibilities">
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" checked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Explore feed</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" checked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Explore community</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" checked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Create posts and messages in chat</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" checked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Save private Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" checked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Share Combat logs with your friends</label>
                </div>
            </div>
        </div>
    );
}

export default LoginPreview;