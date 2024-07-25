import '@testing-library/jest-dom';
import { fireEvent, render } from '@testing-library/react';
import React from 'react';
import { useGetCustomerByIdQuery } from '../store/api/Customer.api';
import AddFriendItem from './AddFriendItem';

jest.mock('../store/api/Customer.api', () => ({
    useGetCustomerByIdQuery: jest.fn(),
}));

jest.mock('react-i18next', () => ({
    useTranslation: () => ({
        t: (key) => key,  // Return the key as the translation
        i18n: {
            changeLanguage: jest.fn(),
        },
    }),
    initReactI18next: {
        type: '3rdParty',
        init: jest.fn(),
    },
}));

describe('AddFriendItem Component', () => {
    const friendUserId = '123';
    const addUserIdToList = jest.fn();
    const removeUserIdToList = jest.fn();
    const filterContent = 'test';
    const peopleIdToJoin = ['456'];

    beforeEach(() => {
        useGetCustomerByIdQuery.mockReturnValue({
            data: {
                id: '123',
                username: 'testUser',
            },
            isLoading: false
        });
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders user username', () => {
        const user = {
            id: '456',
            username: 'testUser',
        };

        const { getByText } = render(
            <AddFriendItem
                friendUserId={friendUserId}
                addUserIdToList={addUserIdToList}
                removeUserIdToList={removeUserIdToList}
                filterContent={filterContent}
                peopleIdToJoin={peopleIdToJoin}
            />
        );

        const usernameElement = getByText(user.username);
        expect(usernameElement).toBeInTheDocument();
    });

    test('renders add user button when user is not in peopleIdToJoin', () => {
        const user = {
            id: '123',
            username: 'testUser',
        };

        const { getByTitle } = render(
            <AddFriendItem
                friendUserId={friendUserId}
                addUserIdToList={addUserIdToList}
                removeUserIdToList={removeUserIdToList}
                filterContent={filterContent}
                peopleIdToJoin={peopleIdToJoin}
            />
        );

        const addButton = getByTitle('SendInvite');
        expect(addButton).toBeInTheDocument();

        fireEvent.click(addButton);
        expect(addUserIdToList).toHaveBeenCalledWith(user.id);
    });

    test('renders remove user button when user is in peopleIdToJoin', () => {
        useGetCustomerByIdQuery.mockReturnValue({
            data: {
                id: '456',
                username: 'testUser',
            },
            isLoading: false
        });

        const user = {
            id: '456',
            username: 'testUser',
        };

        const { getByTitle } = render(
            <AddFriendItem
                friendUserId={friendUserId}
                addUserIdToList={addUserIdToList}
                removeUserIdToList={removeUserIdToList}
                filterContent={filterContent}
                peopleIdToJoin={peopleIdToJoin}
            />
        );

        const removeButton = getByTitle('CancelRequest');
        expect(removeButton).toBeInTheDocument();

        fireEvent.click(removeButton);
        expect(removeUserIdToList).toHaveBeenCalledWith(user.id);
    });

    test('does not render user when username does not start with filterContent', () => {
        const user = {
            id: '123',
            username: 'testUser',
        };

        const { queryByText } = render(
            <AddFriendItem
                friendUserId={friendUserId}
                addUserIdToList={addUserIdToList}
                removeUserIdToList={removeUserIdToList}
                filterContent="other"
                peopleIdToJoin={peopleIdToJoin}
            />
        );

        const usernameElement = queryByText(user.username);
        expect(usernameElement).toBeNull();
    });
});
