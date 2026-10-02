import WoWGameDataContext from '@/context/WoWGameDataContext';
import type { MythicKeystoneBestRunModel } from '../../types/mythicKeystone/MythicKeystoneBestRunModel';
import Character from '../Character';
import { useContext } from 'react';

const SelectedMythicKeystoneDungeon: React.FC<{ run: MythicKeystoneBestRunModel }> = ({ run }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;

    return (
        <ul className="dungeon-members">
            {run.members.map((member, index1) => (
                <li className="dungeon-members__member" key={index1}>
                    <div className="equipments">
                        <div className="special special-name">{t("EquippedItemLevel")}</div>
                        <div className="special">{member.equippedItemLevel}</div>
                    </div>
                    <Character
                        character={member.character}
                    />
                </li>
            ))
            }
        </ul>
    );
}

export default SelectedMythicKeystoneDungeon;