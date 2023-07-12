import React, { useRef } from 'react';
import { useSelector } from 'react-redux';

import '../../../styles/communication/createCommunity.scss';

const CreateCommunity = ({ setShowCreateCommunity }) => {
    const customer = useSelector((state) => state.customer.value);

    const name = useRef(null);
    const description = useRef(null);
    const policy = useRef(null);

    const createCommunityAsync = async () => {
        const data = {
            name: name.current.value,
            description: description.current.value,
            communityPolicyType: +policy.current.value,
            ownerId: customer.id
        };

        const response = await fetch('api/v1/Community', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.status === 200) {
            const createdCommunity = await response.json();

            await createCommunityUserAsync(createdCommunity.id);
        }
    }

    const createCommunityUserAsync = async (communityId) => {
        const data = {
            communityId: communityId,
            customerId: customer.id
        };

        const response = await fetch('api/v1/CommunityUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.status === 200) {
            setShowCreateCommunity(false);
        }
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await createCommunityAsync();
    }

    const render = () => {
        return (<div className="create-community">
            <form onSubmit={handleSubmitAsync}>
                <p>Create community</p>
                <div className="mb-3">
                    <label htmlFor="inputName" className="form-label">Name</label>
                    <input type="text" className="form-control" id="inputName" ref={name} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputDescription" className="form-label">Description</label>
                    <input type="text" className="form-control" id="inputDescription" ref={description} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputDescription" className="form-label">Policy</label>
                    <select className="form-select" ref={policy}>
                        <option value="1">Open</option>
                        <option value="2">Close</option>
                        <option value="3">Close (use link)</option>
                    </select>
                </div>
                <div>
                    <input type="submit" className="btn btn-primary" value="Create" />
                    <input type="button" className="btn btn-info" value="Cancel" onClick={() => setShowCreateCommunity(false)} />
                </div>
            </form>
        </div>);
    }

    return render();
}

export default CreateCommunity;