export interface ProductionPlanHeader {
    id: number;
    productionSessionId: number;
    businessPlaceId: number;
    userId: number;
    date: Date;
    description: string;

    productionPlanDetails?: ProductionPlanDetail[];
    productionPlanRecipes?: ProductionPlanRecipe[];
    productionPlanMachines?: ProductionPlanMachine[];
    productionPlanWorkers?: ProductionPlanWorker[];

    isNotEditable?: boolean;
    sessionName?: string;
    userName?: string;
    businessPlaceName?: string;

    prodOrdrIds?: number[];
}

export interface ProductionPlanDetail {
    id: number;
    quantity: number;
    itemId: number;
    itemName?: string;
    description: string;
}

export interface ProductionPlanWorker {
    id: number;
    employeeId: number;
    employeeName?: string;
}

export interface ProductionPlanRecipe {
    id: number;
    quantity: number;
    itemId: number;
    itemName?: string;
    description: string;
}
export interface ProductionPlanMachine {
    id: number;
    machineryId: number;
    machineryName?: string;
}

export interface ProductionPlanDetailList {
    productionPlanDetails: ProductionPlanDetail[];
}

// export interface ProductionPlanRecipeList {
//     productionPlanDetails: ProductionPlanDetail[];
// }
