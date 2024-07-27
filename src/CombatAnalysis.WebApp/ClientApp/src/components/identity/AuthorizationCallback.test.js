import '@testing-library/jest-dom';
import { render, waitFor } from '@testing-library/react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';
import { useLazyAuthorizationCodeExchangeQuery } from '../../store/api/Identity.api';
import { useLazyStateValidateQuery } from '../../store/api/UserApi';
import AuthorizationCallback from './AuthorizationCallback';

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


jest.mock('react-router-dom', () => ({
    useNavigate: jest.fn(),
}));

jest.mock('../../context/AuthProvider', () => ({
    useAuth: jest.fn(),
}));

jest.mock('../../store/api/Identity.api', () => ({
    useLazyAuthorizationCodeExchangeQuery: jest.fn(),
}));

jest.mock('../../store/api/UserApi', () => ({
    useLazyStateValidateQuery: jest.fn(),
}));

describe('AuthorizationCallback Component', () => {
    let mockNavigate;
    let mockCheckAuthAsync;
    let mockAuthorizationCodeExchangeQuery;
    let mockStateValidateQuery;

    beforeEach(() => {
        mockNavigate = jest.fn();
        useNavigate.mockReturnValue(mockNavigate);

        mockCheckAuthAsync = jest.fn();
        useAuth.mockReturnValue({ checkAuthAsync: mockCheckAuthAsync });

        mockAuthorizationCodeExchangeQuery = jest.fn();
        useLazyAuthorizationCodeExchangeQuery.mockReturnValue([mockAuthorizationCodeExchangeQuery]);

        mockStateValidateQuery = jest.fn();
        useLazyStateValidateQuery.mockReturnValue([mockStateValidateQuery]);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('should navigate to token exchanged when state is valid', async () => {
        const mockResponse = { data: {} };
        mockStateValidateQuery.mockResolvedValue(mockResponse);
        mockAuthorizationCodeExchangeQuery.mockResolvedValue(mockResponse);

        render(<AuthorizationCallback />);

        await waitFor(() => {
            expect(mockStateValidateQuery).toHaveBeenCalled();
            expect(mockAuthorizationCodeExchangeQuery).toHaveBeenCalled();
        });
    });

    test('should navigate to home page when authorization code is exchanged', async () => {
        const mockResponse = { data: {} };
        mockStateValidateQuery.mockResolvedValue(mockResponse);
        mockAuthorizationCodeExchangeQuery.mockResolvedValue(mockResponse);

        render(<AuthorizationCallback />);

        await waitFor(() => {
            expect(mockStateValidateQuery).toHaveBeenCalled();
            expect(mockAuthorizationCodeExchangeQuery).toHaveBeenCalled();
            expect(mockCheckAuthAsync).toHaveBeenCalled();
            expect(mockNavigate).toHaveBeenCalledWith('/');
        });
    });

    test('should navigate to home page when state is invalid', async () => {
        const mockResponse = { data: undefined };
        mockStateValidateQuery.mockResolvedValue(mockResponse);

        render(<AuthorizationCallback />);

        await waitFor(() => {
            expect(mockStateValidateQuery).toHaveBeenCalled();

            setTimeout(() => {
                expect(mockNavigate).toHaveBeenCalledWith('/');
            }, 4000);
        });
    });
});
