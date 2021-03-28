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
}
