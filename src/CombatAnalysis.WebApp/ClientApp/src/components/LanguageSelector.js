import React, { useCallback } from 'react';
import { useTranslation } from 'react-i18next';

const supportedLanguages = [
    "EN",
    "RU"
];

const LanguageSelector = () => {
    const { i18n } = useTranslation("translate");

    const selectedLang = i18n.language;

    const changeLanguage = useCallback((lang) => {
        i18n.changeLanguage(lang);

        window.location.reload(true);
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