type Time = {
    getTimeWithoutMs: (time: string) => string;
    getTotalSeconds: (time: string) => number;
    getDuration: (time1: string, time2: string) => string;
    formatSeconds: (totalSeconds: number) => string;
    formatDate: (dateString: string) => string;
    timeToMs: (time: string) => number;
}

const useTime = (): Time => {
    const getTimeWithoutMs = (time: string): string => {
        const ms = time.indexOf('.');
        const timeWithoutMs = time.substring(0, ms);

        return timeWithoutMs;
    }

    const getTotalSeconds = (duration: string) => {
        const [hours, minutes, seconds] = duration.split(":").map(Number);

        const totalSeconds = hours * 3600 + minutes * 60 + seconds;

        return totalSeconds;
    }

    const getDuration = (time1: string, time2: string): string => {
        const secondsByTime1 = getTotalSeconds(time1);
        const secondsByTime2 = getTotalSeconds(time2);

        let durationToMinutes = 0;
        let durationToHours = 0;
        let durationToSeconds = secondsByTime1 - secondsByTime2;

        if (durationToSeconds > 60) {
            durationToMinutes = Math.trunc(durationToSeconds / 60);
            durationToSeconds -= durationToMinutes * 60;
        }

        if (durationToMinutes > 60) {
            durationToHours = Math.trunc(durationToMinutes / 60);
            durationToMinutes -= durationToHours * 60;
        }

        const duration = `${durationToHours}:${durationToMinutes}:${durationToSeconds > 9 ? durationToSeconds : `0${durationToSeconds}`}`;

        return duration;
    }

    const formatSeconds = (totalSeconds: number): string => {
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = totalSeconds % 60;

        return [
            hours,
            minutes,
            seconds
        ]
            .map(value => String(value).padStart(2, "0"))
            .join(":");
    }

    const formatDate = (dateString: string): string => {
        const date = new Date(dateString);
        const hoursMins = `${date.getUTCHours()}:${date.getUTCMinutes()}:${date.getUTCSeconds()}`;

        return hoursMins;
    }

    const timeToMs = (time: string): number => {
        const [hours, minutes, seconds] = time.split(":").map(Number);

        return (
            hours * 3600 * 1000 +
            minutes * 60 * 1000 +
            seconds * 1000
        );
    }

    return { getTimeWithoutMs, getTotalSeconds, getDuration, formatSeconds, formatDate, timeToMs };
}

export default useTime;