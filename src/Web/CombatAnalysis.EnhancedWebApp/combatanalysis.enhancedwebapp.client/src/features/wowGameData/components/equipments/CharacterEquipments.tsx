import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { CharacterEquipmentQuality } from '@/shared/helpers/EnumHelper';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterEquipmentsQuery } from '../../api/WoWCharacter.api';
import CharacterEquipmentEnchantments from './CharacterEquipmentEnchantments';
import CharacterEquipmentSet from './CharacterEquipmentSet';
import CharacterEquipmentSockets from './CharacterEquipmentSockets';
import CharacterEquipmentSpells from './CharacterEquipmentSpells';
import CharacterEquipmentStats from './CharacterEquipmentStats';

import './CharacterEquipments.scss';

const CharacterEquipments: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);

    const { data: equipments, isLoading, error } = useGetCharacterEquipmentsQuery({ username, serverName, regionName },
        {
            skip: isSkipRequest
        }
    );

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!equipments || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!equipments || isLoading}
        />);
    }

    return (
        <div className="equipments">
            <div className="equipments__title">
                <h6>{t("Equipments")}</h6>
            </div>
            <ul className="equipments__container">
                {equipments.equippedItems.map((equip, index) => (
                    <li key={index} className="equipment">
                        <div className="equipment__inventory-type">
                            <div className="inventory-type">{equip.inventoryType.name}</div>
                            {equip.nameDescription &&
                                <div className="inventory-type"
                                    style={{ color: `rgb(${equip.nameDescription.color.red}, ${equip.nameDescription.color.green}, ${equip.nameDescription.color.blue}, ${equip.nameDescription.color.alfa})` }}>
                                    {equip.nameDescription?.displayString}
                                </div>
                            }
                            <div className="inventory-type">{equip.binding.name}</div>
                            <div className={`quality ${equip.quality.type === CharacterEquipmentQuality[0]
                                ? 'epic'
                                : equip.quality.type === CharacterEquipmentQuality[1]
                                    ? 'uncommon' : ''}`}>{equip.quality.name}</div>
                        </div>
                        {equip.transmog &&
                            <div className="equipment__transmog">
                                <div className="transmog">{equip.transmog?.item.name}</div>
                            </div>
                        }
                        <div className="equipment__name">
                            <div className="level">{equip.itemSubclass.name}</div>
                            <div className="level">{equip.level.value}</div>
                            <div>{equip.name}</div>
                        </div>
                        <div>{equip.armor?.display.displayString}</div>
                        <CharacterEquipmentSockets
                            sockets={equip.sockets}
                        />
                        <CharacterEquipmentEnchantments
                            enchantments={equip.enchantments}
                        />
                        <CharacterEquipmentStats
                            stats={equip.stats}
                        />
                        {equip.spells &&
                            <CharacterEquipmentSpells
                                spells={equip.spells}
                            />
                        }
                        {equip.set &&
                            <CharacterEquipmentSet
                                set={equip.set}
                            />
                        }
                        <div className="equipment__name">
                            <div>{equip.durability?.displayString}</div>
                        </div>
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterEquipments;