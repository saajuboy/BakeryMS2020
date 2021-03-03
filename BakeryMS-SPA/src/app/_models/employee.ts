export interface Employee {
    id: number;
    employeeNumber: number;
    name: string;
    contactNumber: string;
    nic: string;
    address: string;
    type: number;
    salary: number;
    role: number;
    isNotActive: boolean;

    typeName?: string;
    roleName?: string;
}
