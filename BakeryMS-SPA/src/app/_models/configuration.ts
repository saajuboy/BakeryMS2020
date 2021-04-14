export interface Configuration {
    id: number;
    description: string;
    value: string;
    userId?: number;

}

export interface ConfigurationList {
    configurations: Configuration[];
}

export interface Notification {
    id: number;
    title: string;
    message: string;
    date: string;
    time: string;
    isRead: boolean;
    userId: number;
    userName: string;
    status: number;
}
