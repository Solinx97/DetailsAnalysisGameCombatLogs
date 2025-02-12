import '@testing-library/jest-dom';
import { render, screen, waitFor } from '@testing-library/react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';
import { useLazyStateValidateQuery } from '../../store/api/core/User.api';
import { useLazyAuthorizationCodeExchangeQuery } from '../../store/api/user/Identity.api';
import AuthorizationCallback from './AuthorizationCallback';

// Mock dependencies
jest.mock('react-i18next', () => ({
    useTranslation: () => ({ t: (key: string) => key })
}));

jest.mock('react-router-dom', () => ({
    useNavigate: jest.fn(),
    useLocation: jest.fn()
}));

jest.mock('../../context/AuthProvider', () => ({
    useAuth: jest.fn()
}));

jest.mock('../../store/api/core/User.api', () => ({
    useLazyStateValidateQuery: jest.fn()
}));

jest.mock('../../store/api/user/Identity.api', () => ({
    useLazyAuthorizationCodeExchangeQuery: jest.fn()
}));

describe('AuthorizationCallback', () => {
    const mockNavigate = jest.fn();
    const checkAuth = jest.fn();
    const mockUseLazyStateValidateQuery = jest.fn();
    const mockUseLazyAuthorizationCodeExchangeQuery = jest.fn();

    beforeEach(() => {
        (useNavigate as jest.Mock).mockReturnValue(mockNavigate);
        (useLocation as jest.Mock).mockReturnValue({ search: '' });
        (useAuth as jest.Mock).mockReturnValue({ checkAuth });
        (useLazyStateValidateQuery as jest.Mock).mockReturnValue([mockUseLazyStateValidateQuery]);
        (useLazyAuthorizationCodeExchangeQuery as jest.Mock).mockReturnValue([mockUseLazyAuthorizationCodeExchangeQuery]);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders successfully with access restored', () => {
        (useLocation as jest.Mock).mockReturnValue({ search: '?accessRestored=true' });

        render(<AuthorizationCallback />);

        expect(screen.getByText('AccessRestored')).toBeInTheDocument();
    });

    it('renders successfully with verified', () => {
        (useLocation as jest.Mock).mockReturnValue({ search: '?verified=true' });

        render(<AuthorizationCallback />);

        expect(screen.getByText('Verified')).toBeInTheDocument();
    });

    it('renders successfully with valid state', async () => {
        const mockStateValidateQuery = jest.fn().mockResolvedValue({ data: true });
        const mockAuthorizationCodeExchangeQuery = jest.fn().mockResolvedValue({ data: true });

        (useLazyStateValidateQuery as jest.Mock).mockReturnValue([mockStateValidateQuery]);
        (useLazyAuthorizationCodeExchangeQuery as jest.Mock).mockReturnValue([mockAuthorizationCodeExchangeQuery]);

        (useLocation as jest.Mock).mockReturnValue({ search: '?state=validState&code=validCode' });

        render(<AuthorizationCallback />);

        await waitFor(() => {
            expect(screen.getByText('Authorization')).toBeInTheDocument();
        });
    });

    it('renders unauthorized with invalid state', async () => {
        const mockStateValidateQuery = jest.fn().mockResolvedValue({ data: undefined });

        (useLazyStateValidateQuery as jest.Mock).mockReturnValue([mockStateValidateQuery]);

        (useLocation as jest.Mock).mockReturnValue({ search: '?state=invalidState&code=invalidCode' });

        render(<AuthorizationCallback />);

        await waitFor(() => {
            expect(screen.getByText('Unauthorized')).toBeInTheDocument();
        });
    });
});
