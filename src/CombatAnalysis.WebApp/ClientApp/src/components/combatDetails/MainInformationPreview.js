
const MainInformationPreview = () => {
    return (
        <div className="preview">
            <p>Combat logs features:</p>
            <div className="preview__responsibilities">
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="combat-logs-reason-1" id="combat-logs-reason-1" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="combat-logs-reason-1">Save Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="combat-logs-reason-2" id="combat-logs-reason-2" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="combat-logs-reason-2">Save as private or public Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="combat-logs-reason-3" id="combat-logs-reason-3" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="combat-logs-reason-3">Explore your or other users Combat logs</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="combat-logs-reason-4" id="combat-logs-reason-4" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="combat-logs-reason-4">Share link to Combat logs with friends</label>
                </div>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" name="combat-logs-reason-5" id="combat-logs-reason-5" defaultChecked disabled />
                    <label className="form-check-label" htmlFor="combat-logs-reason-5">Discuss with your teammates results of Combat logs</label>
                </div>
            </div>
        </div>
    );
}

export default MainInformationPreview;