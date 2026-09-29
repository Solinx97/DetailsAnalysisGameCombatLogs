import WoWGameDataContext from '@/context/WoWGameDataContext';
import { useContext } from 'react';
import { useGetProfessionsQuery } from '../../api/WoWCharacter.api';

const SelectedRealmCharacterProfessions: React.FC<{ username: string, serverNameSlug: string, isLoadingProfessions: boolean }> = ({ username, serverNameSlug, isLoadingProfessions }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;

    const { data: professions, isLoading, error } = useGetProfessionsQuery({ username, serverName: serverNameSlug, regionName }, {
        skip: !isLoadingProfessions
    });

    if (!isLoadingProfessions) {
        return (<></>);
    }

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    if (!professions || error) {
        return (
            <div className="selected-realm-character-professions">
                <div className="special profession-type">{t("Professions")}</div>
                <div className="special no-any-professions">{t("NoAny")}</div>
            </div>
        );
    }

    return (
        <div className="selected-realm-character-professions">
            <div className="special profession-type">{t("Professions")}</div>
            <div>
                {(!professions.primaries || professions.primaries.length === 0)
                    ? <div className="special no-any-professions">{t("NoAny")}</div>
                    : <ul className="professions">
                        {professions.primaries.map(profession => (
                            <li className="special" key={profession.profession.id}>{profession.profession.name}</li>
                        ))
                        }
                    </ul>
                }
            </div>
        </div>
    );
}

export default SelectedRealmCharacterProfessions;