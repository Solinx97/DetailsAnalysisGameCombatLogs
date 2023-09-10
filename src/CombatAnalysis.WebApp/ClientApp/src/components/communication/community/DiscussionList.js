import { faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import "../../../styles/communication/community/discussionList.scss";

const DiscussionList = ({ discussions, setShowDiscussion, handleDiscussion }) => {
    const { t } = useTranslation("communication/community/discussion");

    const [showSearchPeople, setShowSearchPeople] = useState(false);

    const hidePeopleInspectionMode = () => {
        setShowDiscussion(false);
    }

    const openDiscussion = (item) => {
        setShowDiscussion(false);

        handleDiscussion(item);
    }

    return (
        <div className="discussion-inspection">
            <div className="title">
                {showSearchPeople
                    ? <FontAwesomeIcon
                        icon={faMagnifyingGlassMinus}
                        title={t("HideSearchDiscussion")}
                        onClick={() => setShowSearchPeople(false)}
                    />
                    : <FontAwesomeIcon
                        icon={faMagnifyingGlassPlus}
                        title={t("ShowSearchDiscussion")}
                        onClick={() => setShowSearchPeople(true)}
                    />
                }
                <div>{t("Discussion")}</div>
            </div>
            <div className={`mb-3 discussion-inspection__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputDiscussion" className="form-label">{t("SearchDiscussion")}</label>
                <div className="discussion-inspection__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeDiscussion")} id="inputDiscussion" />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean")}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="discussion-inspection__content">
                {discussions?.map((item) => (
                    <li key={item.id} title={item.title} onClick={() => openDiscussion(item)}>
                        {item.title}
                    </li>
                ))
                }
            </ul>
            <div className="item-result">
                <input type="button" value={t("Close")} className="btn btn-secondary" onClick={hidePeopleInspectionMode} />
            </div>
        </div>
    );
}

export default DiscussionList;