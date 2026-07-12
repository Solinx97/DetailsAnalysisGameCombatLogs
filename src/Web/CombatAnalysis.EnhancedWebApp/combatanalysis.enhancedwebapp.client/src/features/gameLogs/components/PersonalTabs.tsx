import { type ReactNode, useEffect, useState } from 'react';

import './PersonalTabs.scss';

type PersonalTabItemModel = {
    id: number;
    header: string;
    content: ReactNode;
}

interface PersonalTabsProps {
    tab: number;
    tabs: PersonalTabItemModel[];
    tabsClassName: string;
}

const PersonalTabs: React.FC<PersonalTabsProps> = ({ tab, tabs, tabsClassName }) => {
    const [selectedTabIndex, setSelectedTabIndex] = useState(0);

    useEffect(() => {
        setSelectedTabIndex(tab);
    }, [tab])

    const selectTab = (index: number) => {
        setSelectedTabIndex(index);
    }

    return (
        <div className={`tabs ${tabsClassName}`}>
            <div className="tabs__header">
                <ul className="title">
                    {tabs.map((item, index) => (
                        <li key={index} onClick={() => selectTab(index)} className={`${item.id === selectedTabIndex ? 'tab-active' : ''}`}>
                            <div className="tab-name">{item.header}</div>
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