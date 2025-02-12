import '@testing-library/jest-dom';
import { fireEvent, render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { useGetUserByIdQuery } from '../store/api/user/Account.api';
import AddFriendItem from './AddFriendItem';

jest.mock('react-i18next', () => ({
    useTranslation: () => ({
        t: (key: string) => key,
    }),
}));

jest.mock('../store/api/user/Account.api', () => ({
    useGetUserByIdQuery: jest.fn(),
}));

const mockStore = configureStore([]);

describe('AddFriendItem Component', () => {
    let store: any;

    beforeEach(() => {
        jest.clearAllMocks();
        store = mockStore({
            user: {
                value: null,
            },
        });
    });

    const renderComponent = (props: any) => {
        return render(
            <Provider store={store} >
                <AddFriendItem { ...props } />
            </Provider>
        );
    };

    test('renders loading state', () => {
        (useGetUserByIdQuery as jest.Mock).mockReturnValue({ data: null, isLoading: true });

        renderComponent({
            friendUserId: '1',
            addUserIdToList: jest.fn(),
            removeUserIdToList: jest.fn(),
            filterContent: '',
            peopleIdToJoin: [],
        });

        expect(screen.getByText('Loading...')).toBeInTheDocument();
    });

    test('renders user and handles add user', () => {
        const addUserIdToList = jest.fn();
        const user = { id: '1', username: 'testuser' };
        (useGetUserByIdQuery as jest.Mock).mockReturnValue({ data: user, isLoading: false });

        renderComponent({
            friendUserId: '1',
            addUserIdToList,
            removeUserIdToList: jest.fn(),
            filterContent: 'test',
            peopleIdToJoin: [],
        });

        expect(screen.getByText('testuser')).toBeInTheDocument();
        fireEvent.click(screen.getByTitle('SendInvite'));
        expect(addUserIdToList).toHaveBeenCalledWith('1');
    });

    test('renders user and handles remove user', () => {
        const removeUserIdToList = jest.fn();
        const user = { id: '1', username: 'testuser' };
        (useGetUserByIdQuery as jest.Mock).mockReturnValue({ data: user, isLoading: false });

        renderComponent({
            friendUserId: '1',
            addUserIdToList: jest.fn(),
            removeUserIdToList,
            filterContent: 'test',
            peopleIdToJoin: ['1'],
        });

        expect(screen.getByText('testuser')).toBeInTheDocument();
        fireEvent.click(screen.getByTitle('CancelRequest'));
        expect(removeUserIdToList).toHaveBeenCalledWith('1');
    });

    test('does not render user if filter does not match', () => {
        const user = { id: '1', username: 'testuser' };
        (useGetUserByIdQuery as jest.Mock).mockReturnValue({ data: user, isLoading: false });

        renderComponent({
            friendUserId: '1',
            addUserIdToList: jest.fn(),
            removeUserIdToList: jest.fn(),
            filterContent: 'nomatch',
            peopleIdToJoin: [],
        });

        expect(screen.queryByText('testuser')).not.toBeInTheDocument();
    });
});