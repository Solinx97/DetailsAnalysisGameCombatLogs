import { useState, useEffect } from "react";

import "../../styles/common/tabs.scss";

const PersonalTabs = ({ tab, tabs }) => {
    const [selectedTabIndex, setSelectedTabIndex] = useState(0);

    useEffect(() => {
        setSelectedTabIndex(tab);
    }, [tab])

    const selectTab = (index) => {
        setSelectedTabIndex(index);
    }

    return (
        <div className="tabs">
            <div className="tabs__header">
                <ul className="title">
                    {tabs.map((item, index) => (
                        <li key={index} onClick={() => selectTab(index)} className={`${item.id === selectedTabIndex ? 'tab-active' : ''}`}>
                            <div>{item.header}</div>
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