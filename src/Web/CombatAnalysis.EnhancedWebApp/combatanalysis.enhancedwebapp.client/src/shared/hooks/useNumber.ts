type NumberResult = {
    formatNumber: (value: number | string | undefined) => string;
}

const useNumber = (): NumberResult => {
    const formatNumber = (value: number | string | undefined): string => {
        if (value == null) {
            return "";
        }

        const num = Number(value);

        if (num >= 1_000_000_000) {
            return `${(num / 1_000_000_000).toFixed(1)}B`;
        }

        if (num >= 1_000_000) {
            return `${(num / 1_000_000).toFixed(1)}M`;
        }

        if (num >= 1_000) {
            return `${(num / 1_000).toFixed(1)}K`;
        }

        return num.toString();
    }

    return { formatNumber };
}

export default useNumber;