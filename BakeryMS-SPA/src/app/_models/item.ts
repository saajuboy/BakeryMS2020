export interface Item {
    id: number;
    name: string;
    code: string;
    description: string;

    itemCategory: ItemCategory;
    unit: Unit;
    type: number; // 0:production,1:Company,2:Raw,3:Misc
}
export interface ItemForDropdown {
    id: number;
    name: string;
    code: string;
}
export interface ItemCategory {
    id: number;
    code: string;
    description: string;
}

export interface Unit {
    id: number;
    description: string;
}
