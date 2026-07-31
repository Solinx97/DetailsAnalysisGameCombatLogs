type CombatLogResult = {
    removeServerName: (username: string) => string;
}

const useCombatLogs = (): CombatLogResult => {
    const removeServerName = (username: string): string => {
        const fullname = username.replaceAll('"', '').split('-')[0];

        return fullname;
    }

    return { removeServerName };
}

export default useCombatLogs;