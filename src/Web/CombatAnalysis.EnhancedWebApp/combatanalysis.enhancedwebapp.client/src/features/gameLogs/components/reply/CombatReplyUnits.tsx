import { useEffect, useMemo, useState, type Dispatch, type SetStateAction } from 'react';
import type { CombatUnitModel } from '../../types/CombatUnitModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { UnitHealthModel } from '../../types/UnitHealthModel';
import CombatReplyItem from './CombatReplyItem';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import {
    useLazyGetCombatUnitsByCombatIdQuery, useLazyGetCombatPlayersByCombatIdQuery,
    useLazyGetUnitsHealthByCombatIdQuery
} from '../../api/GameLogs.api';

import './CombatReplyUnits.scss';

interface CombatReplyUnitsProps {
    details: CombatDetailsModel;
    selectedGameId: string;
    setSelectedGameId: Dispatch<SetStateAction<string>>;
    currentTime: number;
    colors: Map<string, string>;
}

const CombatReplyUnits: React.FC<CombatReplyUnitsProps> = ({ details, selectedGameId, setSelectedGameId, currentTime, colors }) => {
    const [combatUnits, setCombatUnits] = useState<CombatUnitModel[]>([]);
    const [combatPlayers, setCombatPlayers] = useState<CombatPlayerModel[]>([]);
    const [unitsHealth, setUnitsHealth] = useState<UnitHealthModel[]>([]);

    const [getCombatUnits] = useLazyGetCombatUnitsByCombatIdQuery();
    const [getCombatPlayers] = useLazyGetCombatPlayersByCombatIdQuery();
    const [getUnitsHealth] = useLazyGetUnitsHealthByCombatIdQuery();

    useEffect(() => {
        if (details.id === 0) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatUnits, combatPlayers, unitHealths] = await Promise.all([
                    getCombatUnits(details.id).unwrap(),
                    getCombatPlayers(details.id).unwrap(),
                    getUnitsHealth(details.id).unwrap()
                ]);

                setCombatUnits(combatUnits);
                setCombatPlayers(combatPlayers);
                setUnitsHealth(unitHealths);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [details]);

    const playerUnits = useMemo(() => {
        return combatUnits.filter(x => x.gameId.startsWith("Player"));
    }, [combatUnits]);

    const playerCreatureUnits = useMemo(() => {
        return combatUnits.filter(x => x.gameId.startsWith("Pet") || (x.gameId.startsWith("Creature"))
            && x.creatorGameId && x.creatorGameId.startsWith("Player") && x.unitType && x.unitType.startsWith("0x20"));
    }, [combatUnits]);

    const enemyUnits = useMemo(() => {
        return combatUnits.filter(x => (x.gameId.startsWith("Vehicle") || x.gameId.startsWith("Pet") || x.gameId.startsWith("Creature"))
            && (x.creatorGameId && !x.creatorGameId.startsWith("Player") || !x.creatorGameId));
    }, [combatUnits]);

    return (
        <ul className="units">
            <li className="units__category">
                <div className="title">Players</div>
                <ul className="content">
                    {playerUnits.map((item) => (
                        <li className="player" key={item.gameId}>
                            <CombatReplyItem
                                combatPlayerId={combatPlayers.find(x => x.player.gameId === item.gameId)?.id ?? 0}
                                unit={item}
                                unitsHealth={unitsHealth.filter(x => x.gameId === item.gameId)}
                                selectedPlayerId={selectedGameId}
                                setSelectedPlayerId={setSelectedGameId}
                                currentTime={currentTime}
                                color={colors.get(item.gameId) ?? "#FFFFFF"}
                            />
                        </li>
                    ))
                    }
                </ul>
            </li>
            <li className="units__category">
                <div className="title">Players creators</div>
                <ul className="content">
                    {playerCreatureUnits.map((item) => (
                        <li className="player" key={item.gameId}>
                            <CombatReplyItem
                                combatPlayerId={combatPlayers.find(x => x.player.gameId === item.gameId)?.id ?? 0}
                                unit={item}
                                unitsHealth={unitsHealth.filter(x => x.gameId === item.gameId)}
                                selectedPlayerId={selectedGameId}
                                setSelectedPlayerId={setSelectedGameId}
                                currentTime={currentTime}
                                color={colors.get(item.gameId) ?? "#FFFFFF"}
                            />
                        </li>
                    ))
                    }
                </ul>
            </li>
            <li className="units__category">
                <div className="title">Enemy</div>
                <ul className="content">
                    {enemyUnits.map((item) => (
                        <li className="player" key={item.gameId}>
                            <CombatReplyItem
                                combatPlayerId={combatPlayers.find(x => x.player.gameId === item.gameId)?.id ?? 0}
                                unit={item}
                                unitsHealth={unitsHealth.filter(x => x.gameId === item.gameId)}
                                selectedPlayerId={selectedGameId}
                                setSelectedPlayerId={setSelectedGameId}
                                currentTime={currentTime}
                                color={colors.get(item.gameId) ?? "#FFFFFF"}
                            />
                        </li>
                    ))
                    }
                </ul>
            </li>
        </ul>
    );
}

export default CombatReplyUnits;