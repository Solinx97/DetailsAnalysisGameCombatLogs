import '@testing-library/jest-dom';
import { fireEvent, render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import configureStore from 'redux-mock-store';
import CommunicationMenu from './CommunicationMenu';

jest.mock('react-i18next', () => ({
    useTranslation: () => ({
        t: (key: string) => key,
    }),
}));

jest.mock('react-router-dom', () => ({
    useNavigate: jest.fn(),
}));

const mockStore = configureStore([]);

describe('CommunicationMenu Component', () => {
    let store: any;

    beforeEach(() => {
        jest.clearAllMocks();
        store = mockStore({
            communityMenu: {
                value: 0,
            },
        });
    });

    const renderComponent = () => {
        return render(
            <Provider store={store}>
                <CommunicationMenu
                    currentMenuItem={0}
                    hasSubMenu={true}
                />
            </Provider>
        );
    };

    test('renders menu items', () => {
        renderComponent();

        expect(screen.getByText('Feed')).toBeInTheDocument();
        expect(screen.getByText('Chats')).toBeInTheDocument();
        expect(screen.getByText('Communities')).toBeInTheDocument();
        expect(screen.getByText('Events')).toBeInTheDocument();
        expect(screen.getByText('People')).toBeInTheDocument();
        expect(screen.getByText('MyEnvironment')).toBeInTheDocument();
    });

    test('navigates to correct route on menu item click', () => {
        const navigateMock = jest.fn();
        (useNavigate as jest.Mock).mockReturnValue(navigateMock);

        renderComponent();

        fireEvent.click(screen.getByText('Feed'));
        expect(navigateMock).toHaveBeenCalledWith('/feed');
    });

    test('disables menu item correctly', () => {
        renderComponent();

        const eventsMenuItem = screen.getByText('Events');
        const parentElement = eventsMenuItem.closest('li');
        expect(parentElement).toHaveClass('menu-item_disabled');
    });

    test('renders sub-menu items', () => {
        const navigateMock = jest.fn();
        (useNavigate as jest.Mock).mockReturnValue(navigateMock);

        renderComponent();

        fireEvent.click(screen.getByText('MyEnvironment'));
        expect(screen.getByText('MyPosts')).toBeInTheDocument();
        expect(screen.getByText('Friends')).toBeInTheDocument();
        expect(screen.getByText('Communities')).toBeInTheDocument();
        expect(screen.getByText('Profile')).toBeInTheDocument();
    });
});