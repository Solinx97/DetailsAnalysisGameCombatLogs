import { APP_CONFIG } from '@/config/appConfig';
import { useEffect, useRef, useState, type ChangeEvent, type ReactElement, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';

interface CommonItemProps {
    connector?: ReactElement;
    name: string;
    setName(value: SetStateAction<string>): void;
    description: string | "";
    setDescription(value: SetStateAction<string>): void;
    useDescription: boolean;
    allowEdit: boolean;
}

const CommonItem: React.FC<CommonItemProps> = ({ connector, name, setName, description, setDescription, useDescription = false, allowEdit }) => {
    const maxNameLength = 128;
    const maxDescriptionLength = 512;

    const { t } = useTranslation('communication/create');

    const maxNameLengthRef = useRef<number>(APP_CONFIG.communication.length.communityNameMaxLength ?? maxNameLength);
    const maxDescriptionLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDescriptionMaxLength ?? maxDescriptionLength);

    const [currentNameLength, setCurrentNameLength] = useState(0);
    const [currentDescriptionLength, setCurrentDescriptionLength] = useState(0);

    useEffect(() => {
        setCurrentNameLength(name.length);
    }, [name]);

    useEffect(() => {
        setCurrentDescriptionLength(description.length);
    }, [description]);

    const nameHandle = (e: ChangeEvent<HTMLInputElement>) => {
        setName(e.target.value);
        setCurrentNameLength(e.target.value.length);
    }

    const descriptionHandle = (e: ChangeEvent<HTMLTextAreaElement>) => {
        setDescription(e.target.value);
        setCurrentDescriptionLength(e.target.value.length);
    }

    if (!allowEdit) {
        return (
            <div className="create-community__item restricted">
                <div className="title">{t("CommunityInformation")}</div>
                <>
                    <div className="form-group">
                        <label htmlFor="name">{t("Name")}:</label>
                        <div id="name">{name}</div>
                    </div>
                    {useDescription &&
                        <div className="form-group">
                            <label htmlFor="description">{t("Description")}:</label>
                            <div id="description">{description}</div>
                        </div>
                    }
                </>
                {connector}
            </div>
        );
    }

    return (
        <div className="community-information">
            <div className="title">{t("CommunityInformation")}</div>
            <>
                <>
                    <div className="form-group">
                        <label htmlFor="name">{t("Name")}</label>
                        <div className={`content-length ${name.length === maxNameLengthRef.current ? 'limit' : ''}`}>{currentNameLength}/{maxNameLengthRef.current}</div>
                        <input type="text" className="form-control" name="name" id="name" maxLength={maxNameLengthRef.current}
                            onChange={nameHandle} value={name} required />
                    </div>
                    {name.length === 0 &&
                        <div className="community-name-required">{t("NameRequired")}</div>
                    }
                    <div className="form-group">
                        <label htmlFor="description">{t("Description")}</label>
                        <div className={`content-length ${description.length === maxDescriptionLengthRef.current ? 'limit' : ''}`}>{currentDescriptionLength}/{maxDescriptionLengthRef.current}</div>
                        <textarea className="form-control" name="description" id="description" maxLength={maxDescriptionLengthRef.current}
                            onChange={descriptionHandle} value={description} required />
                    </div>
                    {description.length === 0 &&
                        <div className="community-description-required">{t("DescriptionRequired")}</div>
                    }
                </>
                {connector}
            </>
        </div>
    );
}

export default CommonItem;