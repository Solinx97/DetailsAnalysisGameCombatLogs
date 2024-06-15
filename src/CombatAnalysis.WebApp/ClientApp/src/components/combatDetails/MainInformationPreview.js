
const MainInformationPreview = () => {
    return (
        <div className="preview">
            <p>Combat logs features:</p>
            <div className="preview__responsibilities">
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Save Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Save as private or public Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Explore your or other users Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Share link to Combat logs with friends</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="flexRadioDefault" id="flexRadioDefault1" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="flexRadioDefault1">Discuss with your teammates results of Combat logs</label>
                </div>
            </div>
        </div>
    );
}

export default MainInformationPreview;