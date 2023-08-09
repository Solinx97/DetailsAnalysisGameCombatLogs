import React, { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreateCommunityAsyncMutation } from '../../../store/api/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';

import '../../../styles/communication/createCommunity.scss';

const CreateCommunity = ({ customer, setShowCreateCommunity }) => {
    const { t } = useTranslation("communication/myEnvironment/createCommunity");

    const name = useRef(null);
    const description = useRef(null);
    const policyType = useRef(null);

    const [createCommunityAsyncMut] = useCreateCommunityAsyncMutation();
    const [createCommunityUserAsyncMut] = useCreateCommunityUserAsyncMutation();

    const createCommunityAsync = async () => {
        const newCommunity = {
            name: name.current.value,
            description: description.current.value,
            policyType: +policyType.current.value,
            ownerId: customer.id
        };

        const createdCommunity = await createCommunityAsyncMut(newCommunity);
        if (createdCommunity.data !== undefined) {
            await createCommunityUserAsync(createdCommunity.data.id);
        }
    }

    const createCommunityUserAsync = async (communityId) => {
        const newCommunityUser = {
            communityId: communityId,
            customerId: customer.id
        };

        const createdCommunityUser = await createCommunityUserAsyncMut(newCommunityUser);
        if (createdCommunityUser.data !== undefined) {
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
                    <p>{t("CreateCommunity")}</p>
                    <div className="mb-3">
                        <label htmlFor="inputName" className="form-label">{t("Name")}</label>
                        <input type="text" className="form-control" id="inputName" ref={name} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputDescription" className="form-label">{t("Description")}</label>
                        <input type="text" className="form-control" id="inputDescription" ref={description} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputDescription" className="form-label">{t("Policy")}</label>
                        <select className="form-select" ref={policyType}>
                            <option value="1">{t("Open")}</option>
                            <option value="2">{t("Close")}</option>
                            <option value="3">{t("CloseWithLink")}</option>
                        </select>
                    </div>
                    <div>
                        <input type="submit" className="btn btn-primary" value={t("Create")} />
                        <input type="button" className="btn btn-info" value={t("Cancel")} onClick={() => setShowCreateCommunity(false)} />
                    </div>
                </form>
            </div>
        );
    }

    return render();
}

export default CreateCommunity;