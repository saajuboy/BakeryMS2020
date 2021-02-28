import { TimeUnit } from 'chart.js';
import { BusinessPlace } from './businessPlace';
import { Item } from './item';
import { User } from './User';


export interface ProductionOrderHeader {
    id: number;
    productionOrderNo: number;
    sessionId: number;
    session?: ProductionSession;
    userId: number;
    user?: User;
    businessPlaceId: number;
    businessPlace?: BusinessPlace;
    requiredDate: Date;
    enteredDate: Date;
    isNotEditable?: boolean;

    productionOrderDetails?: ProductionOrderDetail[];

    sessionName?: string;
    userName?: string;
    businessPlaceName?: string;
}

export interface ProductionOrderDetail {
    id: number;
    quantity: number;
    itemId: number;
    item: string;
    description: string;
}

export interface ProductionOrderDetailRow {

    itemCode: string;
    itemName: string;
    items: any;
    quantity: number;
    description: string;
}

export interface ProductionSession {
    id: number;
    session: string;
    startTime: TimeUnit;
    endTime: TimeUnit;
}

