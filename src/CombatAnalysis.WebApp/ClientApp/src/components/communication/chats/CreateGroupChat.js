import { useRef, useState } from "react";
import { useCreateGroupChatAsyncMutation } from '../../../store/api/GroupChat.api';
import { useCreateGroupChatUserAsyncMutation } from '../../../store/api/GroupChatUser.api';

const CreateGroupChat = ({ setCreateGroupChatIsActive, customer, createGroupChatIsActive }) => {
    const [chatPolicyType, setChatPolicyType] = useState(0);

    const nameInput = useRef(null);
    const shortNameInput = useRef(null);
    const memberNumberSelect = useRef(null);

    const [createGroupChatAsync] = useCreateGroupChatAsyncMutation();
    const [createGroupChatUserAsync] = useCreateGroupChatUserAsyncMutation();

    const createNewGroupChatAsync = async (event) => {
        event.preventDefault();

        const newGroupChat = {
            name: nameInput.current.value,
            shortName: shortNameInput.current.value,
            lastMessage: " ",
            memberNumber: +memberNumberSelect.current.value,
            chatPolicyType: chatPolicyType,
            ownerId: customer?.id
        };

        const createdGroupChat = await createGroupChatAsync(newGroupChat);
        if (createdGroupChat.error !== undefined) {
            return;
        }

        const newGroupChatUser = {
            userId: customer?.id,
            groupChatId: createdGroupChat.data.id,
        };

        const createdGroupChatUser = await createGroupChatUserAsync(newGroupChatUser);
        if (createdGroupChatUser.error !== undefined) {
            return;
        }

        setCreateGroupChatIsActive(false);
    }

    return (
        <div className={`create-group-chat${createGroupChatIsActive ? "_active" : ""}`}>
            <p className="create-group-chat__title">Create a new group chat</p>
            <form onSubmit={createNewGroupChatAsync}>
                <div className="form-group">
                    <label htmlFor="group-chat-name">Name</label>
                    <input type="text" className="form-control" name="name" id="group-chat-name" ref={nameInput} required />
                </div>
                <div className="form-group">
                    <label htmlFor="short-group-chat-name">Short name</label>
                    <input type="text" className="form-control" name="shortName" id="short-group-chat-name" ref={shortNameInput} required />
                </div>
                <div className="form-group">
                    <label htmlFor="exampleFormControlSelect1">Member number</label>
                    <select className="form-control" name="memberNumber" ref={memberNumberSelect} id="exampleFormControlSelect1">
                        <option>100</option>
                        <option>500</option>
                        <option>1000</option>
                        <option>2500</option>
                    </select>
                </div>
                <div className="chat-policy">
                    <p>Chat policy</p>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="chat-policy" id="public" value="0" defaultChecked onChange={() => setChatPolicyType(0)} />
                        <label className="form-check-label" htmlFor="public">Public</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="chat-policy" id="private" value="1" onChange={() => setChatPolicyType(1)} />
                        <label className="form-check-label" htmlFor="private">Private</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="chat-policy" id="privatelinks" value="2" disabled onChange={() => setChatPolicyType(2)} />
                        <label className="form-check-label" htmlFor="privatelinks">Private with link</label>
                    </div>
                </div>
                <div className="create-group-chat__accept">
                    <input type="submit" value="Create" className="btn btn-success" />
                    <input type="button" value="Close" className="btn btn-light" onClick={() => setCreateGroupChatIsActive(false)} />
                </div>
            </form>
        </div>
    );
}

export default CreateGroupChat;