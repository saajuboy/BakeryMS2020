export interface Machinery {
    id: number;
    name: string;
    model: string;
    capacity: number;
    value?: number;
    businessPlaceId: number;
    businessPlaceName: string;
    purchaseDate?: Date;

    isChecked?: boolean; // Only used in production Plan
}
