import { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreateCommunityDiscussionAsyncMutation } from '../../../store/api/communication/community/CommunityDiscussion.api';

const CreateDiscussion = ({ community, customer, setShowDiscussion }) => {
    const { t } = useTranslation("communication/community/selectedCommunity");

    const title = useRef(null);
    const content = useRef(null);

    const [createCommunityAsyncMut] = useCreateCommunityDiscussionAsyncMutation();

    const createDiscussionAsync = async (event) => {
        event.preventDefault();

        const newDiscussion = {
            title: title.current.value,
            content: content.current.value,
            when: new Date(),
            customerId: customer?.id,
            communityId: community?.id
        }

        const created = await createCommunityAsyncMut(newDiscussion);
        if (created.data !== undefined) {
            setShowDiscussion(false);
        }
    }

    return (
        <form className="discussion__create" onSubmit={async (event) => await createDiscussionAsync(event)}>
            <div className="title">{t("CreateDiscussion")}</div>
            <div className="form-group">
                <label htmlFor="title">{t("Title")}</label>
                <input type="text" className="form-control" id="title" ref={title} />
            </div>
            <div className="form-group">
                <label htmlFor="Content">{t("Content")}</label>
                <textarea className="form-control" id="Content" rows="8" ref={content} />
            </div>
            <div className="actions">
                <input type="submit" className="btn btn-primary" value={t("Create")} />
                <input type="button" className="btn btn-light" value={t("Cancel")} onClick={() => setShowDiscussion(false)} />
            </div>
        </form>
    );
}

export default CreateDiscussion;