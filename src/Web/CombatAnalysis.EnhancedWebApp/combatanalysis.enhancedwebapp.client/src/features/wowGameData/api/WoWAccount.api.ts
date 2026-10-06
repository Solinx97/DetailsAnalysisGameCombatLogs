import type { AccountDetailsModel } from '../types/account/AccountDetailsModel';
import type { CharacterModel } from '../types/account/CharacterModel';
import type { WoWAccountRespone } from '../types/account/WoWAccountRespone';
import type { WoWAccountCollectionItemModel } from '../types/collections/WoWAccountCollectionItemModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWAccountApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getAccountCharacters: builder.query<WoWAccountRespone, { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getCharacters?regionName=${regionName}`,
        }),
        getAccountCharactersList: builder.query<CharacterModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getCharactersList?regionName=${regionName}`,
        }),
        getAccountMounts: builder.query<WoWAccountCollectionItemModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getMounts?regionName=${regionName}`,
        }),
        getAccountToys: builder.query<WoWAccountCollectionItemModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getToys?regionName=${regionName}`,
        }),
        getAccountPets: builder.query<WoWAccountCollectionItemModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getPets?regionName=${regionName}`,
        }),
        getAccountSetTransmogs: builder.query<WoWAccountCollectionItemModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getSetTransmogs?regionName=${regionName}`,
        }),
        getAccountSlotTransmogs: builder.query<Map<string, WoWAccountCollectionItemModel[]>, { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getSlotTransmogs?regionName=${regionName}`,
        }),
        getDashboard: builder.query<AccountDetailsModel, { regionName: string, serverName: string, characterName: string }>({
            query: ({ regionName, serverName, characterName }) => `/WoWAccount/getDashboard?regionName=${regionName}&serverName=${serverName}&characterName=${characterName}`,
        }),
    })
})

export const {
    useGetAccountCharactersQuery,
    useGetAccountCharactersListQuery,
    useGetAccountToysQuery,
    useGetAccountMountsQuery,
    useGetAccountPetsQuery,
    useGetAccountSetTransmogsQuery,
    useGetAccountSlotTransmogsQuery,
    useGetDashboardQuery,
} = WoWAccountApi;