import '@testing-library/jest-dom';
import { fireEvent, render } from '@testing-library/react';
import { useNavigate } from 'react-router-dom';
import CommunicationMenu from './CommunicationMenu';

jest.mock('react-i18next', () => ({
    useTranslation: () => ({
        t: (key) => key,
    }),
}));

jest.mock('react-router-dom', () => ({
    useNavigate: jest.fn(),
}));

describe('CommunicationMenu Component', () => {
    const currentMenuItem = 0;
    const setMenuItem = jest.fn();

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders menu items correctly', () => {
        const { getByText, getAllByText } = render(
            <CommunicationMenu
                currentMenuItem={currentMenuItem}
                setMenuItem={setMenuItem}
            />
        );

        const feedMenuItem = getByText('Feed');
        expect(feedMenuItem).toBeInTheDocument();

        const chatsMenuItem = getByText('Chats');
        expect(chatsMenuItem).toBeInTheDocument();

        const communitiesMenuItems = getAllByText('Communities');
        expect(communitiesMenuItems.length).toBeGreaterThan(0);
        expect(communitiesMenuItems[0]).toBeInTheDocument();

        const eventsMenuItem = getByText('Events');
        expect(eventsMenuItem).toBeInTheDocument();

        const peopleMenuItem = getByText('People');
        expect(peopleMenuItem).toBeInTheDocument();

        const myEnvironmentMenuItem = getByText('MyEnvironment');
        expect(myEnvironmentMenuItem).toBeInTheDocument();
    });

    test('navigates to correct route when menu item is clicked', () => {
        const navigateMock = jest.fn();
        useNavigate.mockReturnValue(navigateMock);

        const { getByText, getAllByText } = render(
            <CommunicationMenu
                currentMenuItem={currentMenuItem}
                setMenuItem={setMenuItem}
            />
        );

        const feedMenuItem = getByText('Feed');
        fireEvent.click(feedMenuItem);
        expect(navigateMock).toHaveBeenCalledWith('/feed');

        const chatsMenuItem = getByText('Chats');
        fireEvent.click(chatsMenuItem);
        expect(navigateMock).toHaveBeenCalledWith('/chats');

        const communitiesMenuItems = getAllByText('Communities');
        expect(communitiesMenuItems.length).toBeGreaterThan(0);
        fireEvent.click(communitiesMenuItems[0]);
        expect(navigateMock).toHaveBeenCalledWith('/communities');

        const peopleMenuItem = getByText('People');
        fireEvent.click(peopleMenuItem);
        expect(navigateMock).toHaveBeenCalledWith('/people');
    });

    test('renders sub menu correctly when MyEnvironment menu item is clicked', () => {
        const { getByText, getAllByText } = render(
            <CommunicationMenu
                currentMenuItem={8}
                setMenuItem={setMenuItem}
            />
        );

        const myPostsMenuItem = getByText('MyPosts');
        expect(myPostsMenuItem).toBeInTheDocument();

        const friendsMenuItem = getByText('Friends');
        expect(friendsMenuItem).toBeInTheDocument();

        const communitiesMenuItems = getAllByText('Communities');
        expect(communitiesMenuItems.length).toBeGreaterThan(1);
        expect(communitiesMenuItems[1]).toBeInTheDocument();

        const profileMenuItem = getByText('Profile');
        expect(profileMenuItem).toBeInTheDocument();
    });

    test('navigates to correct route when sub menu item is clicked', () => {
        const { getByText, getAllByText } = render(
            <CommunicationMenu
                currentMenuItem={8}
                setMenuItem={setMenuItem}
            />
        );

        const myPostsMenuItem = getByText('MyPosts');
        fireEvent.click(myPostsMenuItem);
        expect(setMenuItem).toHaveBeenCalledWith(8);

        const friendsMenuItem = getByText('Friends');
        fireEvent.click(friendsMenuItem);
        expect(setMenuItem).toHaveBeenCalledWith(9);

        const communitiesMenuItems = getAllByText('Communities');
        expect(communitiesMenuItems.length).toBeGreaterThan(1);
        fireEvent.click(communitiesMenuItems[1]);
        expect(setMenuItem).toHaveBeenCalledWith(10);

        const profileMenuItem = getByText('Profile');
        fireEvent.click(profileMenuItem);
        expect(setMenuItem).toHaveBeenCalledWith(12);
    });
});
