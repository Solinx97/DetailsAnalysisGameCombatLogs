import { CombatLogStatus } from '@/shared/helpers/EnumHelper';
import { faRemove } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useAddCombatLogStatusMutation, useRemoveCombatLogMutation } from '../api/GameLogs.api';
import type { CombatLogModel } from '../types/CombatLogModel';

const CombatLogItemActions: React.FC<{ t: (key: string) => string, combatLog: CombatLogModel }> = ({ t, combatLog }) => {
    const [updateCombatLogStatus] = useAddCombatLogStatusMutation();
    const [removeLog] = useRemoveCombatLogMutation();

    const updateStatusAsync = async () => {
        if (combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleting"]
            || combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleted"]) {
            return;
        }

        try {
            await updateCombatLogStatus({ combatLogId: combatLog.id, status: CombatLogStatus["Deleting"] }).unwrap();
        } catch (error) {
            console.error("Failed to update combat log status:", error);
        }
    }

    const removeCombatLogAsync = async () => {
        if (combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleting"]
            || combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleted"]) {
            return;
        }

        try {
            await updateCombatLogStatus({ combatLogId: combatLog.id, status: CombatLogStatus["Deleting"] }).unwrap();
            await removeLog(combatLog.id).unwrap();
        } catch (error) {
            console.error("Failed to remove combat log:", error);
        }
    }

    const removeHandle = async () => {
        if (combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleting"]
            || combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleted"]) {
            return;
        }

        await updateStatusAsync();
        await removeCombatLogAsync();
    }

    return (
        <div className={`logs-actions ${combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleting"] ? 'in-progress' : ''}`}>
            <div className="btn-shadow" onClick={removeHandle}>
                <FontAwesomeIcon
                    icon={faRemove}
                />
                <div>{t("Delete")}</div>
            </div>
            {combatLog.statuses.at(-1)?.status === CombatLogStatus["Deleting"] &&
                <div className="removing">{t("Deleting")}</div>
            }
        </div>
    );
}

export default CombatLogItemActions;