
const CombatAuraTargets = ({ combatAuras }) => {
    const uniqueTargets = [...new Set(combatAuras.map(aura => aura.target))];

    return (
        <ul className="targets">
            {uniqueTargets.map((target, index) => (
                <div key={index}>{target}</div>
            ))}
        </ul>
    );
}

export default CombatAuraTargets;