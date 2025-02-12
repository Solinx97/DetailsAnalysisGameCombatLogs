import '@testing-library/jest-dom';
import { fireEvent, render } from '@testing-library/react';
import LanguageSelector from './LanguageSelector';

const mockChangeLanguage = jest.fn();

jest.mock('react-i18next', () => ({
    useTranslation: () => ({
        i18n: {
            changeLanguage: mockChangeLanguage,
            language: 'en',
        },
    }),
}));

describe('LanguageSelector Component', () => {
    beforeAll(() => {
        // Mock window.location.reload
        Object.defineProperty(window, 'location', {
            value: {
                reload: jest.fn(),
            },
            writable: true,
        });
    });

    test('renders language options', () => {
        const { getByText } = render(<LanguageSelector />);

        expect(getByText('en')).toBeInTheDocument();
        expect(getByText('ru')).toBeInTheDocument();
    });

    test('changes language on click', () => {
        const { getByText } = render(<LanguageSelector />);
        const russianOption = getByText('ru');
        const englishOption = getByText('en');

        fireEvent.click(russianOption);
        expect(mockChangeLanguage).toHaveBeenCalledWith('ru');

        fireEvent.click(englishOption);
        expect(mockChangeLanguage).toHaveBeenCalledWith('en');
    });

    test('reloads the page on language change', () => {
        const { getByText } = render(<LanguageSelector />);
        const originalReload = window.location.reload;
        window.location.reload = jest.fn();

        fireEvent.click(getByText('ru'));
        expect(window.location.reload).toHaveBeenCalled();

        window.location.reload = originalReload;
    });
});