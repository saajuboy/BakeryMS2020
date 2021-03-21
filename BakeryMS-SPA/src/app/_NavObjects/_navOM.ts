import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
    {
        name: 'Dashboard',
        url: '/dashboard',
        icon: 'icon-speedometer'
    },
    {
        title: true,
        name: 'Main Navigation'
    },
    {
        name: 'POS',
        url: '/inventory',
        icon: 'icon-wallet',
        children: [
            {
                name: 'Purchase Order',
                url: '/inventory/purchaseOrder',
                icon: 'icon-list'
            }
        ]
    },
    {
        name: 'Inventory',
        url: '/inventory',
        icon: 'icon-basket-loaded',
        children: [
            {
                name: 'Purchase Order',
                url: '/inventory/purchaseOrder',
                icon: 'icon-list'
            },
            {
                name: 'Create Pur.Order',
                url: '/inventory/purchaseOrder/create',
                icon: 'icon-note'
            },
            {
                name: 'Goods Rec.Note',
                url: '/inventory/grn',
                icon: 'icon-check'
            }
        ]
    },
    {
        name: 'Manufacturing',
        url: '/inventory',
        icon: 'icon-fire ',
        children: [
            {
                name: 'Production Order',
                url: '/manufacturing/productionOrder',
                icon: 'icon-briefcase',
                children: [
                    {
                        name: 'Production Order List',
                        url: '/manufacturing/productionOrder',
                        icon: 'icon-list'
                    },
                    {
                        name: 'Create Prod.Order',
                        url: '/manufacturing/productionOrder/create',
                        icon: 'icon-note'
                    }
                ]
            }
        ]
    },
    {
        name: 'Employee Management',
        url: '/hr',
        icon: 'icon-people ',
        children: [
            {
                name: 'Employee',
                url: '/hr/employees',
                icon: 'icon-people '
            }
        ]
    },
    {
        name: 'Schedule Routine',
        url: '/hr',
        icon: 'icon-refresh ',
        children: [
            {
                name: 'Schedule Routines',
                url: '/hr/routines',
                icon: 'icon-fire '
            }
        ]
    },
    {
        name: 'Quality Control',
        url: '/inventory',
        icon: 'icon-heart ',
        children: [
            {
                name: 'Purchase Order',
                url: '/inventory/purchaseOrder',
                icon: 'icon-fire '
            }
        ]
    },
    {
        name: 'Reports',
        url: '/inventory',
        icon: 'icon-docs ',
        children: [
            {
                name: 'Purchase Order',
                url: '/inventory/purchaseOrder',
                icon: 'icon-fire '
            }
        ]
    },
    {
        name: 'Master Data',
        url: '/inventory',
        icon: 'icon-globe ',
        children: [
            {
                name: 'Purchase Order',
                url: '/inventory/purchaseOrder',
                icon: 'icon-fire '
            }
        ]
    },
    {
        divider: true
    },
    {
        title: true,
        name: 'System',
    },
    {
        name: 'Configuration',
        url: '/inventory',
        icon: 'icon-settings ',
        children: [
            {
                name: 'Purchase Order',
                url: '/inventory/purchaseOrder',
                icon: 'icon-fire '
            }
        ]
    }
];
