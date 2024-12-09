import { useEffect, useState } from "react";

const CombatAuraItem = ({ combatAuras }) => {
    const [selectedCreatorAuras, setSelectedCreatorAuras] = useState(new Map());

    useEffect(() => {
        getCreatorAurasCount();
    }, [combatAuras]);

    const getCreatorAurasCount = () => {
        const auraMap = new Map();

        combatAuras.forEach(aura => {
            const time = clearTime(aura.startTime.split('.')[0]);

            if (auraMap.has(aura.name)) {
                const creatorAurasCount = auraMap.get(aura.name).count;
                const creatorAurasTimes = auraMap.get(aura.name).times;
                creatorAurasTimes.push(time);

                auraMap.set(aura.name, { count: creatorAurasCount + 1, times: creatorAurasTimes });
            } else {
                auraMap.set(aura.name, { count: 1, times: [time] });
            }
        });

        setSelectedCreatorAuras(auraMap);
    }

    const clearTime = (timeString) => {
        const splitedTime = timeString.split(':');
        let time = +splitedTime[0] > 0 ? `${splitedTime[0]}:` : "";
        time += `${splitedTime[1]}:${splitedTime[2]}`;

        return time;
    }

    if (selectedCreatorAuras.size === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <ul className="creator-auras">
            {Array.from(selectedCreatorAuras.entries()).map(([key, value]) => (
                <li key={key} className="creator-auras__details">
                    <div>{key}</div>
                    <div>{value.count}</div>
                    <ul className="times">
                        {value.times.map((time, index) => (
                            <li ket={index}>{time}</li>
                        ))}
                    </ul>
                </li>
            ))}
        </ul>
    );
}

export default CombatAuraItem;