import React, { useRef } from 'react';
import { useSelector } from 'react-redux';
import CommunityService from '../../../services/CommunityService';
import CommunityUserService from '../../../services/CommunityUserService';

import '../../../styles/communication/createCommunity.scss';

const CreateCommunity = ({ setShowCreateCommunity }) => {
    const communityService = new CommunityService();
    const communityUserService = new CommunityUserService();

    const customer = useSelector((state) => state.customer.value);

    const name = useRef(null);
    const description = useRef(null);
    const policyType = useRef(null);

    const createCommunityAsync = async () => {
        const newCommunity = {
            name: name.current.value,
            description: description.current.value,
            policyType: +policyType.current.value,
            ownerId: customer.id
        };

        const createdCommunity = await communityService.createAsync(newCommunity);
        if (createdCommunity !== null) {
            await createCommunityUserAsync(createdCommunity.id);
        }
    }

    const createCommunityUserAsync = async (communityId) => {
        const newCommunityUser = {
            communityId: communityId,
            customerId: customer.id
        };

        const createdCommunityUser = await communityUserService.createAsync(newCommunityUser);
        if (createdCommunityUser !== null) {
            setShowCreateCommunity(false);
        }
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await createCommunityAsync();
    }

    const render = () => {
        return (
            <div className="create-community">
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
                        <select className="form-select" ref={policyType}>
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
            </div>
        );
    }

    return render();
}

export default CreateCommunity;