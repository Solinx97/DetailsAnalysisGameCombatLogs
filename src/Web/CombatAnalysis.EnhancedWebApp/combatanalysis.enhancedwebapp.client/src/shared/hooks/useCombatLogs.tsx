type CombatLogResult = {
    removeServerName: (username: string) => string;
}

const useCombatLogs = (): CombatLogResult => {
    const removeServerName = (name: string): string => {
        const fullname = name.replaceAll('"', '').split('-')[0];

        return fullname;
    }

    return { removeServerName };
}

export default useCombatLogs;