import type { SearchItemModel } from './SearchItemModel';

export type SearchItemResponse = {
    page: number;
    pageSize: number;
    maxPageSize: number;
    pageCount: number;
    results: SearchItemModel[];
}