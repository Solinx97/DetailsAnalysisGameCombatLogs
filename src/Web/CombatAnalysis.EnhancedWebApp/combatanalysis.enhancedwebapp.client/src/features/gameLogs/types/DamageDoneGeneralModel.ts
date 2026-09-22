export type DamageDoneGeneralModel = {
    id: string;
    spell: string;
    damagePerSecond: number;
    value: number;
    critNumber: number;
    missNumber: number;
    castNumber: number;
    minValue: number;
    maxValue: number;
    averageValue: number;
    isPet: boolean;
    unitId: string;
}