import { useTranslation } from 'react-i18next';

const RulesItem = ({ nextStep, previouslyStep }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    const handleInviteChange = (event) => {

    }

    const handleRemoveChange = (event) => {

    }

    const handlePinMessageChange = (event) => {

    }

    const handleAnnounceChange = (event) => {

    }

    return (
        <div className="create-group-chat__item">
            <div>Rules</div>
            <ul className="chat-rules">
                <li>
                    <div>Who can invite another people?</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-anyone" value="0"
                                onChange={handleInviteChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="invite-people-anyone">Anyone</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-special" value="1"
                                onChange={handleInviteChange} disabled/>
                            <label className="form-check-label" htmlFor="invite-people-special">Special people</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>Who can remove people?</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-anyone" value="0"
                                onChange={handleRemoveChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="remove-people-anyone">Anyone</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-special" value="1"
                                onChange={handleRemoveChange} disabled/>
                            <label className="form-check-label" htmlFor="remove-people-special">Special people</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>Who can pin message?</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="pin-message-anyone" value="0"
                                onChange={handlePinMessageChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="pin-message-anyone">Anyone</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="pin-message-special" value="1"
                                onChange={handlePinMessageChange} disabled/>
                            <label className="form-check-label" htmlFor="pin-message-special">Special people</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>Who can make announcements?</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="announce" id="announce-anyone" value="0"
                                onChange={handleAnnounceChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="announce-anyone">Anyone</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="announce-special" value="1"
                                onChange={handleAnnounceChange} disabled/>
                            <label className="form-check-label" htmlFor="announce-special">Special people</label>
                        </div>
                    </div>
                </li>
            </ul>
            <div>
                <input type="button" value="Next" className="btn btn-success" onClick={() => nextStep(2)} />
                <input type="button" value="Back" className="btn btn-light" onClick={previouslyStep} />
            </div>
        </div>
    );
}

export default RulesItem;