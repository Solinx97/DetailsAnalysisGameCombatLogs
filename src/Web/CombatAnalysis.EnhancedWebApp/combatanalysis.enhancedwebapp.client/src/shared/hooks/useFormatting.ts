const useFormatting = () => {
    const dateFormatting = (stringOfDate: string): string => {
        const date = new Date(stringOfDate);
        const month = date.getMonth();
        const monthes = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        const formatted = `${date.getDate()} ${monthes[month]}, ${date.getHours()}:${date.getMinutes()}`;

        return formatted;
    }

    const getDate = (date: string) => {
        const parse = new Date(date);

        return parse.toLocaleString('ru-RU', {
            dateStyle: 'short',
            timeStyle: 'short'
        });
    }

    const getPreviousDayOfWeek = (targetDay: number) => {
        const date = new Date();

        const currentDay = date.getDay();

        let daysSince = (currentDay - targetDay + 7) % 7;

        if (daysSince === 0) {
            daysSince = 7;
        }

        date.setDate(date.getDate() - daysSince);

        return date;
    }

    return { dateFormatting, getDate, getPreviousDayOfWeek };
}

export default useFormatting;