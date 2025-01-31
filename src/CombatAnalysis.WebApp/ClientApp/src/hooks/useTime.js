const useTime = () => {
    const getTimeWithoutMs = (time) => {
        let ms = time.indexOf('.');
        let timeWithoutMs = time.substring(0, ms);

        return timeWithoutMs;
    }

    const getSeconds = (time) => {
        let timeElementsByTime = time.split(':');
        let hoursByTime = +timeElementsByTime[0];
        let minutesByTime = (hoursByTime * 60) + +timeElementsByTime[1];
        let secondsByTime = (minutesByTime * 60) + +timeElementsByTime[2];

        return secondsByTime;
    }

    const getDuration = (time1, time2) => {
        let secondsByTime1 = getSeconds(time1);
        let secondsByTime2 = getSeconds(time2);

        let durationToMinutes = "00";
        let durationToHours = "00";
        let durationToSeconds = secondsByTime1 - secondsByTime2;

        if (durationToSeconds > 60) {
            durationToMinutes = Math.trunc(durationToSeconds / 60);
            durationToSeconds -= durationToMinutes * 60;
        }

        if (durationToMinutes > 60) {
            durationToHours = Math.trunc(durationToMinutes / 60);
            durationToMinutes -= durationToHours * 60;
        }

        let duration = `${durationToHours}:${durationToMinutes}:${durationToSeconds > 9 ? durationToSeconds : `0${durationToSeconds}`}`;

        return duration;
    }

    return { getTimeWithoutMs, getSeconds, getDuration };
}

export default useTime;