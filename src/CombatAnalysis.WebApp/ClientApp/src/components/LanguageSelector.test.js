import '@testing-library/jest-dom';
import { fireEvent, render } from '@testing-library/react';
import React from 'react';
import { useTranslation } from 'react-i18next';
import LanguageSelector from './LanguageSelector';

jest.mock('react-i18next', () => ({
    useTranslation: jest.fn().mockReturnValue({
        i18n: {
            language: 'en',
            changeLanguage: jest.fn(),
        },
    }),
}));

describe('LanguageSelector Component', () => {
    beforeAll(() => {
        delete window.location;
        window.location = { reload: jest.fn() };
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders language options correctly', () => {
        useTranslation.mockReturnValueOnce({ i18n: { language: 'en', changeLanguage: jest.fn() } });

        const { getByText } = render(<LanguageSelector />);

        const enLangOption = getByText('en');
        const ruLangOption = getByText('ru');

        expect(enLangOption).toBeInTheDocument();
        expect(ruLangOption).toBeInTheDocument();
    });

    test('displays selected language correctly', () => {
        useTranslation.mockReturnValueOnce({ i18n: { language: 'en', changeLanguage: jest.fn() } });

        const { getByText } = render(<LanguageSelector />);

        const selectedLang = getByText('en');

        expect(selectedLang).toHaveClass('selected-lang');
    });

    test('calls changeLanguage function on language selection', () => {
        const changeLanguageMock = jest.fn();
        useTranslation.mockReturnValueOnce({ i18n: { language: 'en', changeLanguage: changeLanguageMock } });

        const { getByText } = render(<LanguageSelector />);

        const ruLangOption = getByText('ru');
        fireEvent.click(ruLangOption);

        expect(changeLanguageMock).toHaveBeenCalledWith('ru');
    });
});
