import { faRemove } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRemoveCombatLogMutation } from '../api/GameLogs.api';
import { useState } from 'react';

const CombatLogItemActions: React.FC<{ t: (key: string) => string, combatLogId: number }> = ({ t, combatLogId }) => {
    const [removeLog] = useRemoveCombatLogMutation();

    const [removing, setRemoving] = useState(false);

    const removeHandle = async () => {
        if (removing) {
            return;
        }

        try {
            setRemoving(true);
            await removeLog(combatLogId).unwrap();
        } catch (error) {
            console.error("Failed to remove combat log:", error);
            setRemoving(false);
        }
    }

    return (
        <div className={`logs-actions ${removing ? 'in-progress' : ''}`}>
            <div className="btn-shadow" onClick={removeHandle}>
                <FontAwesomeIcon
                    icon={faRemove}
                />
                <div>{t("Remove")}</div>
            </div>
            {removing &&
                <div className="removing">{t("Removing")}</div>
            }
        </div>
    );
}

export default CombatLogItemActions;