import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useCreateGroupChatUserAsyncMutation } from '../../../../store/api/GroupChatUser.api';
import AddPeople from '../../../AddPeople';

const InvitePeopleItem = ({ createNewGroupChatAsync, previouslyStep, customer, peopleIdToJoin, setPeopleToJoin }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();

    const navigate = useNavigate();

    const joinPeopleToChatAsync = async (groupChatId) => {
        for (var i = 0; i < peopleIdToJoin.length; i++) {
            const newGroupChatUser = {
                userId: peopleIdToJoin[i],
                groupChatId: groupChatId,
            };

            await createGroupChatUserMutAsync(newGroupChatUser);
        }
    }

    const handleCreateNewGroupChatAsync = async () => {
        const createdGroupChat = await createNewGroupChatAsync();
        await joinPeopleToChatAsync(createdGroupChat.id);

        navigate("/chats");
    }

    return (
        <div className="create-group-chat__item">
            <AddPeople
                customer={customer}
                communityUsersId={[0]}
                peopleToJoin={peopleIdToJoin}
                setPeopleToJoin={setPeopleToJoin}
            />
            <div className="item-result">
                <input type="button" value={t("Create")} className="btn btn-success" onClick={async () => await handleCreateNewGroupChatAsync()} />
                <input type="button" value={t("Back")} className="btn btn-light" onClick={previouslyStep} />
            </div>
        </div>
    );
}

export default InvitePeopleItem;