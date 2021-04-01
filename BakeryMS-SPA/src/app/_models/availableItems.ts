import { TimeUnit } from "chart.js";
import { BusinessPlace } from "./businessPlace";

export interface AvailableItemForList {
    id: number;
    name: string;
    code: string;
    unit: string;
    type: number; // 0:production,1:Company,2:Raw,3:Misc
    manufacturedDate: Date;
    expireDate: Date;
    batchNo: number;
    costPrice: number;
    stockedQuantity: number;
    usedQuantity: number;
    availableQuantity: number;
    businessPlaceId: number;
    businessPlaceName: string;
    sellingPrice?: number;
}
export interface SalesHeader {
    id: number;
    salesNo: number;
    date: Date;
    businessPlaceId: number;
    businessPlaceName?: string;
    userName?: string;
    businessPlace?: BusinessPlace;
    customerName: string;
    time?: TimeUnit;
    total: number;
    discount: number;
    receivedAmount: number;
    changeAmount: number;
    isHolded?: boolean;
    isCharged?: boolean;
    salesDetails: SalesDetail[];
}
export interface SalesDetail {
    id: number;
    itemId: number;
    type: number;
    quantity: number;
    price: number;
    lineTotal: number;
    itemName?: string;
}

export interface SalesHeaderForPos {
    id: number;
    salesNo: number;
    date: Date;
    businessPlaceId: number;
    businessPlaceName?: string;
    userName?: string;
    businessPlace?: BusinessPlace;
    customerName: string;
    time?: TimeUnit;
    total: number;
    discount: number;
    receivedAmount: number;
    changeAmount: number;
    isHolded?: boolean;
    isCharged?: boolean;
    salesDetails: SalesDetailForPos[];

    // name: string;
    // code: string;
    // unit: string;
    // type: number; // 0:production,1:Company,2:Raw,3:Misc
    // manufacturedDate: Date;
    // expireDate: Date;
    // batchNo: number;
    // costPrice: number;
    // stockedQuantity: number;
    // usedQuantity: number;
    // availableQuantity: number;
    // sellingPrice?: number;
}
export interface SalesDetailForPos {
    id: number;
    itemId: number;
    type: number;
    quantity: number;
    price: number;
    lineTotal: number;
    previousQuantity?: number;
    itemName?: string;
}
