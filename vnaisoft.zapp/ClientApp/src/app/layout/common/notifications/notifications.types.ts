export interface Notification {
    id: string;
    icon?: string;
    image?: string;
    title?: string;
    description?: string;
    time: string;
    link?: string;
    menu: string;
    useRouter?: boolean;
    read: boolean;
}
