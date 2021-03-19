import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
    {
        name: 'Dashboard',
        url: '/dashboard',
        icon: 'icon-speedometer'
        // badge: {
        //     variant: 'info',
        //     text: 'NEW'
        // }
    },
    {
        title: true,
        name: 'Main Navigation'
    },
    // {
    //     name: 'Colors',
    //     url: '/theme/colors',
    //     icon: 'icon-drop'
    // },
    // {
    //     name: 'Typography',
    //     url: '/theme/typography',
    //     icon: 'icon-pencil'
    // },
    // {
    //     title: true,
    //     name: 'Components'
    // },
    // {
    //     name: 'Test Module',
    //     url: '/test',
    //     icon: 'icon-puzzle',
    //     roles: ['sdsd', 'dsdsd'],
    //     attributes: { disabled: false }
    // },
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
                name: 'Accept Prod Items',
                url: '/inventory/itemAcceptance',
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
            },
            {
                name: 'Production Plan',
                url: '/manufacturing/productionPlan',
                icon: 'icon-briefcase',
                children: [
                    {
                        name: 'Production Plan List',
                        url: '/manufacturing/productionPlan',
                        icon: 'icon-list'
                    },
                    {
                        name: 'Create Prod.Plan',
                        url: '/manufacturing/productionPlan/create',
                        icon: 'icon-note'
                    }
                ]
            },
            {
                name: 'Recipe',
                url: '/manufacturing/ingredient',
                icon: 'icon-briefcase',
                children: [
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
    },
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
