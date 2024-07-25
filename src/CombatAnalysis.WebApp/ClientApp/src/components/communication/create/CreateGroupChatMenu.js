import { faCircleCheck, faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';

const CreateGroupChatMenu = ({ passedItemIndex, seItemIndex, itemIndex }) => {
    const { t } = useTranslation("communication/create");

    const changeMenuItem = (index) => {
        if (passedItemIndex < index) {
            return;
        }

        seItemIndex(index);
    }

    return (
        <ul className="create-community__menu">
            <li className={`menu-item ${passedItemIndex >= 0 ? "passed" : ""}`} onClick={() => changeMenuItem(0)}>
                {(passedItemIndex > 0 && itemIndex !== 0)
                    ? <FontAwesomeIcon
                        className={`menu-item__passed ${itemIndex === 0 ? "active" : ""}`}
                        icon={faCircleCheck}
                    />
                    : <FontAwesomeIcon
                        className={`menu-item__not-passed ${itemIndex === 0 ? "active" : ""}`}
                        icon={faCircleQuestion}
                    />
                }
                <span className={`${passedItemIndex > 0 ? "move-to-next-step" : ""}`}></span>
            </li>
            <li className={`menu-item ${passedItemIndex >= 1 ? "passed" : ""}`} onClick={() => changeMenuItem(1)}>
                {(passedItemIndex > 1 && itemIndex !== 1)
                    ? <FontAwesomeIcon
                        className={`menu-item__passed ${itemIndex === 1 ? "active" : ""}`}
                        icon={faCircleCheck}
                    />
                    : <FontAwesomeIcon
                        className={`menu-item__not-passed ${itemIndex === 1 ? "active" : ""}`}
                        icon={faCircleQuestion}
                    />
                }
                <span className={`${passedItemIndex > 1 ? "move-to-next-step" : ""}`}></span>
            </li>
            <li className={`menu-item ${passedItemIndex >= 2 ? "passed" : ""}`} onClick={() => changeMenuItem(2)}>
                {(passedItemIndex > 2 && itemIndex !== 2)
                    ? <FontAwesomeIcon
                        className={`menu-item__passed ${itemIndex === 2 ? "active" : ""}`}
                        icon={faCircleCheck}
                    />
                    : <FontAwesomeIcon
                        className={`menu-item__not-passed ${itemIndex === 2 ? "active" : ""}`}
                        icon={faCircleQuestion}
                    />
                }
            </li>
        </ul>
    );
}

export default CreateGroupChatMenu;