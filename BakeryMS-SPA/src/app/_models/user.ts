

export interface User {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    // contactNumber: number;
    gender: string;
    created: Date;
    lastActive: Date;
    status: boolean;
    // photoUrl: string;

    // photos?: Photo[];
}
export interface Customer {
    id: number;
    name: string;
    address: string;
    contact: string;
    isRetail: boolean;
    status: number;
    debit: number;
    credit: number;
    typeName?: string;
}