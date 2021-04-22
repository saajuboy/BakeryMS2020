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

export interface Dashboard {
    expense: number[];
    income: number[];
    net: number[];

    orderReceived: number;
    maxOrders: number;

    ordersHandled: number;
    maxOrdersToHandle: number;

    reorders: number;
    maxItems: number;

    workers: number;
    workersMax: number;

}

export interface ControlProcedure {
    id: number;
    name: string;
    description: string;
    businessPlaceName: string;
    businessPlaceId: string;
}
