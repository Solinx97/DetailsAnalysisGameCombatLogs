import '@testing-library/jest-dom';
import { fireEvent, render } from '@testing-library/react';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { MemoryRouter, useNavigate } from 'react-router-dom';
import { useLazyIdentityQuery } from '../store/api/UserApi';
import Home from './Home';

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: jest.fn(),
}));

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn(),
    useDispatch: jest.fn(),
}));

jest.mock('../store/api/UserApi', () => ({
    useLazyIdentityQuery: jest.fn(),
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

describe('Home Component', () => {
    beforeEach(() => {
        useSelector.mockReturnValue(null);
        useDispatch.mockReturnValue(jest.fn());
        useLazyIdentityQuery.mockReturnValue([jest.fn()]);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders the component', () => {
        const { getAllByText } = render(
            <MemoryRouter>
                <Home />
            </MemoryRouter>
        );

        const communityElements = getAllByText('Communication');
        expect(communityElements.length).toBeGreaterThan(0);
        communityElements.forEach(element => {
            expect(element).toBeInTheDocument();
        });

        const analyzingElements = getAllByText('Analyzing');
        expect(analyzingElements.length).toBeGreaterThan(0);
        analyzingElements.forEach(element => {
            expect(element).toBeInTheDocument();
        });
    });

    test('renders the authorize alert when customer is null', () => {
        useSelector.mockReturnValue(null);

        const { getByText } = render(
            <MemoryRouter>
                <Home />
            </MemoryRouter>
        );

        expect(getByText('ShouldAuthorize')).toBeInTheDocument();
    });

    test('does not render the authorize alert when customer is not null', () => {
        useSelector.mockReturnValue({});

        const { queryByText } = render(
            <MemoryRouter>
                <Home />
            </MemoryRouter>
        );

        expect(queryByText('ShouldAuthorize')).toBeNull();
    });

    test('navigates to feed when "Open" button is clicked', () => {
        const navigateMock = jest.fn();
        useNavigate.mockReturnValue(navigateMock);
        useSelector.mockReturnValue({});

        const { getAllByText } = render(
            <MemoryRouter>
                <Home />
            </MemoryRouter>
        );

        const openElements = getAllByText('Open');
        expect(openElements.length).toBeGreaterThan(0);
        openElements.forEach(element => {
            fireEvent.click(element);
        });

        expect(navigateMock).toHaveBeenCalledWith('/feed');
    });

    test('navigates to main information when "Open" button is clicked', () => {
        const navigateMock = jest.fn();
        useNavigate.mockReturnValue(navigateMock);
        useSelector.mockReturnValue({});

        const { getAllByText } = render(
            <MemoryRouter>
                <Home />
            </MemoryRouter>
        );

        const openElements = getAllByText('Open');
        expect(openElements.length).toBeGreaterThan(0);
        openElements.forEach(element => {
            fireEvent.click(element);
        });

        expect(navigateMock).toHaveBeenCalledWith('/main-information');
    });
});
