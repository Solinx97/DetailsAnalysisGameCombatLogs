export type HealDoneGeneralModel = {
    id: string;
    spell: string;
    healPerSecond: number;
    value: number;
    critNumber: number;
    castNumber: number;
    minValue: number;
    maxValue: number;
    averageValue: number;
    isPet: boolean;
    unitId: string;
}