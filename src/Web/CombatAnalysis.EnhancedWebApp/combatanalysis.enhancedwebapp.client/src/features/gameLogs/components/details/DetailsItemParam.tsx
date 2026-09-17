import './DetailsItemParam.scss';

interface DetailsItemParamProps {
    topName: string;
    topValue: string;
    bottomName: string;
    bottomValue: string;
    title: string;
    navigate: () => void;
    isActive: boolean;
}

const DetailsItemParam: React.FC<DetailsItemParamProps> = ({ topName, topValue, bottomName, bottomValue, title, navigate, isActive }) => {
    return (
        <div className="details-item-param">
            <div>{topName}</div>
            <div className={`details-item-param__value ${isActive ? 'active' : ''}`} title={title}
                    onClick={isActive ? navigate : () => {}}>
                <div>{topValue}</div>
                <span></span>
                <div>{bottomValue}</div>
            </div>
            <div>{bottomName}</div>
        </div>
    );
}

export default DetailsItemParam;