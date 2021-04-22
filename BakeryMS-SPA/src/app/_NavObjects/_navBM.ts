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
        url: '/pos',
        icon: 'icon-wallet',
        children: [
            {
                name: 'Sales',
                url: '/pos/sales/create',
                icon: 'icon-note'
            },
            {
                name: 'Sales List',
                url: '/pos/sales',
                icon: 'icon-list'
            },
            {
                name: 'Transactions',
                url: '/pos/transactions',
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
                name: 'Available Items',
                url: '/inventory/item',
                icon: 'icon-social-dropbox ',
            },
            {
                name: 'Purchase Order List',
                url: '/inventory/purchaseOrder',
                icon: 'icon-list'
            },
            {
                name: 'Create Pur.Order',
                url: '/inventory/purchaseOrder/create',
                icon: 'icon-note'
            },
            {
                name: 'Accept Prod Items',
                url: '/inventory/itemAcceptance',
                icon: 'icon-check'
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
        url: '/manufacturing',
        icon: 'icon-fire ',
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
            },
            {
                name: 'Production Plan List',
                url: '/manufacturing/productionPlan',
                icon: 'icon-list'
            },
            {
                name: 'Create Prod.Plan',
                url: '/manufacturing/productionPlan/create',
                icon: 'icon-note'
            },
            {
                name: 'Ingredient List',
                url: '/manufacturing/ingredient',
                icon: 'icon-list'
            },
            {
                name: 'Create Ingredients',
                url: '/manufacturing/ingredient/create',
                icon: 'icon-note'
            }
        ]
    },
    {
        name: 'Employee Management',
        url: '/hr',
        icon: 'icon-people ',
        children: [
            {
                name: 'Employees',
                url: '/hr/employees',
                icon: 'icon-people '
            },
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
                name: 'Control Procedure',
                url: '/quality/controlProcedure',
                icon: 'icon-heart '
            }
        ]
    },
    {
        name: 'Reports',
        url: '/reports',
        icon: 'icon-docs ',
        children: [
            {
                name: 'Master Reports',
                url: '/reports/master',
                icon: 'icon-list',
            },
            {
                name: 'Inventory Reports',
                url: '/reports/inventory',
                icon: 'icon-list'
            },
            {
                name: 'Manufacturing Reports',
                url: '/reports/manufacuring',
                icon: 'icon-list'
            },
            {
                name: 'Sales Reports',
                url: '/reports/sales',
                icon: 'icon-list'
            }
        ]
    },
    {
        name: 'Master Data',
        url: '/Master',
        icon: 'icon-globe ',
        children: [
            {
                name: 'Item List',
                url: '/master/item',
                icon: 'icon-list'
            },
            {
                name: 'Create Item',
                url: '/master/item/create',
                icon: 'icon-note'
            },
            {
                name: 'Item Category',
                url: '/master/itemCategory',
                icon: 'icon-list '
            },
            {
                name: 'Customer',
                url: '/master/customer',
                icon: 'icon-list'
            },
            {
                name: 'Supplier',
                url: '/master/supplier',
                icon: 'icon-list '
            },
            {
                name: 'Unit',
                url: '/master/unit',
                icon: 'icon-list '
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
        url: '/configuration',
        icon: 'icon-settings '
    }
    // {
    //     name: 'Pages',
    //     url: '/pages',
    //     icon: 'icon-star',
    //     children: [
    //         {
    //             name: 'Login',
    //             url: '/login',
    //             icon: 'icon-star'
    //         },
    //         {
    //             name: 'Register',
    //             url: '/register',
    //             icon: 'icon-star'
    //         },
    //         {
    //             name: 'Error 404',
    //             url: '/404',
    //             icon: 'icon-star'
    //         },
    //         {
    //             name: 'Error 500',
    //             url: '/500',
    //             icon: 'icon-star'
    //         }
    //     ]
    // },
    // {
    //     name: 'Disabled',
    //     url: '/dashboard',
    //     icon: 'icon-ban',
    //     badge: {
    //         variant: 'secondary',
    //         text: 'NEW'
    //     },
    //     attributes: { disabled: true },
    // }
];
