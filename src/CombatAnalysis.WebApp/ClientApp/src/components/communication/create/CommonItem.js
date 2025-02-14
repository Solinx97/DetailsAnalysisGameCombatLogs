import { useTranslation } from 'react-i18next';

const CommonItem = ({ connector, name, setName, description = "", setDescription = "", useDescription = false }) => {
    const { t } = useTranslation("communication/create");

    return (
        <div className="create-community__item">
            <div className="title">{t("Description")}</div>
            <>
                <>
                    <div className="form-group">
                        <label htmlFor="name">{t("Name")}</label>
                        <input type="text" className="form-control" name="name" id="name"
                            onChange={(e) => setName(e.target.value)} value={name} required />
                    </div>
                    {useDescription &&
                        <div className="form-group">
                            <label htmlFor="description">{t("Description")}</label>
                            <textarea className="form-control" name="description" id="description"
                                onChange={(e) => setDescription(e.target.value)} value={description} required />
                        </div>
                    }
                </>
                {connector}
            </>
        </div>
    );
}

export default CommonItem;