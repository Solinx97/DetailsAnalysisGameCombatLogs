export interface MenuItem {
    id: number;
    label: string;
    navigateTo: string;
    icon: any;
    disabled: boolean;
    subMenu: MenuItem[] | null;
}