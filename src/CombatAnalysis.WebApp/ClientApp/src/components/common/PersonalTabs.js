import { useState } from "react";
import "../../styles/common/tabs.scss";

const PersonalTabs = ({ tabs }) => {
    const [selectedTabIndex, setSelectedTabIndex] = useState(0);

    const selectTab = (index) => {
        setSelectedTabIndex(index);
    }

    return (
        <div className="tabs">
            <div className="tabs__header">
                <ul>
                    {tabs.map((item, index) => (
                        <li key={index} onClick={() => selectTab(index)} className={`${item.id === selectedTabIndex ? 'tab-active' : ''}`}>
                            {item.header}
                        </li>
                    ))}
                </ul>
            </div>
            <div className="tabs__content">
                {tabs.filter(item => item.id === selectedTabIndex).map((item) => (
                    <div key={item.id} className="content">
                        {item.content}
                    </div>
                ))}
            </div>
        </div>
    );
}

export default PersonalTabs;