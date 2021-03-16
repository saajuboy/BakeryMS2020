import { Time } from '@angular/common';
import { TimeUnit } from 'chart.js';

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

    isChecked?: boolean; // Only used in production Plan
}

export interface Routine {
    id: number;
    employeeId: number;
    employee: Employee;
    startTime: TimeUnit;
    endTime: TimeUnit;
    businessPlaceId: number;
    date: Date;
    roleId: number;
}

export interface RoutineList {
    routines: Routine[];
}
