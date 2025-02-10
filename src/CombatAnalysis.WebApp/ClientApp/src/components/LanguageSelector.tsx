import { useCallback } from 'react';
import { useTranslation } from 'react-i18next';

const supportedLanguages = [
    "en",
    "ru"
];

const LanguageSelector: React.FC = () => {
    const { i18n } = useTranslation("translate");

    const selectedLang = i18n.language;

    const changeLanguage = useCallback((lang: string) => {
        i18n.changeLanguage(lang);

        window.location.reload();
    }, [i18n]);

    return (
        <div className="language dropdown">
            <ul className="language__select">
                {supportedLanguages.map((lang) => (
                    <li key={lang} className={`${lang === selectedLang ? 'selected-lang' : ''}`}
                        onClick={() => changeLanguage(lang)}>{lang}</li>
                ))}
            </ul>
        </div>
    );
}

export default LanguageSelector;