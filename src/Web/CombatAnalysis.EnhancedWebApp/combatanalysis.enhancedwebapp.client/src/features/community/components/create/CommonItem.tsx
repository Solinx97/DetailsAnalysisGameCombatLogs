import type { ReactElement, SetStateAction } from 'react';
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
    const { t } = useTranslation('communication/create');

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
        <div className="create-community__item">
            <div className="title">{t("CommunityInformation")}</div>
            <>
                <>
                    <div className="form-group">
                        <label htmlFor="name">{t("Name")}</label>
                        <input type="text" className="form-control" name="name" id="name"
                            onChange={(e) => setName(e.target.value)} value={name} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="description">{t("Description")}</label>
                        <textarea className="form-control" name="description" id="description"
                            onChange={(e) => setDescription(e.target.value)} value={description} required />
                    </div>
                </>
                {connector}
            </>
        </div>
    );
}

export default CommonItem;