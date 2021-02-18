

export interface PurchaseOrderHeader {
    id: number;
    poNumber: number;
    userId: number;
    supplierId: number;
    status: boolean;
    deliveryMethod: string;
    orderDate: Date;
    deliveryDate: Date;
    modifiedDate: Date;
    supplierName?: string;
    userName?: string;

    poDetail?: PurchaseOrderDetail[];
}

export interface PurchaseOrderDetail {
    id: number;
    orderQty: number;
    itemid: number;
    unitPrice: number;
    lineTotal: number;
    receivedQuantity: number;
    rejectedQuantity: number;
    stockedQuantity: number;
    item: string;
    modifiedDate: Date;
    dueDate: Date;
}

export interface PODRow {

    itemCode: string;
    itemName: string;
    items: any;
    dueDate: string;
    quantity: number;
    unitPrice: number;
    lineTotal: number;
}

