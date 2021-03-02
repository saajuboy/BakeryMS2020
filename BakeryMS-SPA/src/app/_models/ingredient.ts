export interface IngredientHeader {
    id: number;
    itemId: number;
    servingSize: number;
    description: string;
    method: string;

    ingredientDetails?: IngredientDetail[];

    itemName?: string;
}
export interface IngredientDetail {
    id: number;
    itemId: number;
    itemName: string;
    quantity: number;

}


