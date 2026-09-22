import type { DashboardItemModel } from './DashboardItemModel';

export type DashboardModel = {
    type: number;
    items: DashboardItemModel[];
}