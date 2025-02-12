import '@testing-library/jest-dom';
import { fireEvent, render, screen } from '@testing-library/react';
import { useSelector } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyAuthorizationQuery } from '../store/api/core/User.api';
import Home from './Home';

// Mock dependencies
jest.mock('react-i18next', () => ({
    useTranslation: () => ({ t: (key: string) => key })
}));

jest.mock('react-redux', () => ({
    useSelector: jest.fn()
}));

jest.mock('react-router-dom', () => ({
    useNavigate: jest.fn(),
    useLocation: jest.fn()
}));

jest.mock('../store/api/core/User.api', () => ({
    useLazyAuthorizationQuery: jest.fn()
}));

describe('Home Component', () => {
    const mockNavigate = jest.fn();
    const mockUseLazyAuthorizationQuery = jest.fn();

    beforeEach(() => {
        (useNavigate as jest.Mock).mockReturnValue(mockNavigate);
        (useLocation as jest.Mock).mockReturnValue({ search: '' });
        (useLazyAuthorizationQuery as jest.Mock).mockReturnValue([mockUseLazyAuthorizationQuery]);
        (useSelector as jest.Mock).mockReturnValue(null);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders without crashing', () => {
        render(<Home />);
    });

    test('shows authorization alert when shouldBeAuthorize is true', () => {
        (useLocation as jest.Mock).mockReturnValue({ search: '?shouldBeAuthorize=true' });

        render(<Home />);

        expect(screen.getByTestId('should-be-authorize')).toBeInTheDocument();
    });

    test('login button calls loginAsync function', async () => {
        render(<Home />);

        const loginButton = screen.getByTitle('GoToLogin');
        fireEvent.click(loginButton);

        expect(mockUseLazyAuthorizationQuery).toHaveBeenCalled();
    });

    test('navigates to feed when user is not null', () => {
        (useSelector as jest.Mock).mockReturnValue({ id: 1 });

        render(<Home />);

        const navigateButton = screen.getByTestId('go-to-communication');
        fireEvent.click(navigateButton);

        expect(mockNavigate).toHaveBeenCalledWith('/feed');
    });

    test('navigates to main information', () => {
        render(<Home />);

        const navigateButton = screen.getByTestId('go-to-combat-logs');
        fireEvent.click(navigateButton);

        expect(mockNavigate).toHaveBeenCalledWith('/main-information');
    });
});