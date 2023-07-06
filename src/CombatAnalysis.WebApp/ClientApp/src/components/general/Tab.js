import React, { useState } from 'react';

const Tab = ({ tabs }) => {
    const [activeTab, setActiveTab] = useState(0);

    const handleTabClick = (index) => {
        setActiveTab(index);
    }

    const render = () => {
        return (<div className="tabs">
            <div className="tabs__nav">
                {tabs.map((tab, index) => (
                    <div
                        key={index}
                        className={`tab ${activeTab === index ? 'active' : ''}`}
                        onClick={() => handleTabClick(index)}
                    >
                        {tab.label}
                    </div>
                ))}
            </div>
            <div className="tabs__content">
                {tabs[activeTab].content}
            </div>
        </div>);
    }

    return render();
}

export default Tab;