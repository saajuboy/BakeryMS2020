import { INavData } from "@coreui/angular";

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
    // ,
    // {
    //     name: 'POS',
    //     url: '/pos',
    //     icon: 'icon-wallet',
    //     children: [
    //         {
    //             name: 'Sales',
    //             url: '/pos/sales/create',
    //             icon: 'icon-note'
    //         },
    //         {
    //             name: 'Sales List',
    //             url: '/pos/sales',
    //             icon: 'icon-list'
    //         },
    //         {
    //             name: 'Transactions',
    //             url: '/pos/transactions',
    //             icon: 'icon-list'
    //         }
    //     ]
    // },
    // {
    //     name: 'Inventory',
    //     url: '/inventory',
    //     icon: 'icon-basket-loaded',
    //     children: [
    //         {
    //             name: 'Available Items',
    //             url: '/inventory/item',
    //             icon: 'icon-social-dropbox ',
    //         },
    //         {
    //             name: 'Purchase Order',
    //             url: '/inventory/purchaseOrder',
    //             icon: 'icon-list'
    //         },
    //         {
    //             name: 'Create Pur.Order',
    //             url: '/inventory/purchaseOrder/create',
    //             icon: 'icon-note'
    //         },
    //         {
    //             name: 'Accept Prod Items',
    //             url: '/inventory/itemAcceptance',
    //             icon: 'icon-check'
    //         },
    //         {
    //             name: 'Goods Rec.Note',
    //             url: '/inventory/grn',
    //             icon: 'icon-check'
    //         }
    //     ]
    // },
    // {
    //     name: 'Manufacturing',
    //     url: '/inventory',
    //     icon: 'icon-fire ',
    //     children: [
    //         {
    //             name: 'Production Order',
    //             url: '/manufacturing/productionOrder',
    //             icon: 'icon-briefcase',
    //             children: [
    //                 {
    //                     name: 'Production Order List',
    //                     url: '/manufacturing/productionOrder',
    //                     icon: 'icon-list'
    //                 },
    //                 {
    //                     name: 'Create Prod.Order',
    //                     url: '/manufacturing/productionOrder/create',
    //                     icon: 'icon-note'
    //                 }
    //             ]
    //         }
    //     ]
    // },
    // {
    //     name: 'Reports',
    //     url: '/inventory',
    //     icon: 'icon-docs ',
    //     children: [
    //         {
    //             name: 'Purchase Order',
    //             url: '/inventory/purchaseOrder',
    //             icon: 'icon-fire '
    //         }
    //     ]
    // },
    // {
    //     divider: true
    // },
    // {
    //     title: true,
    //     name: 'System',
    // },
    // {
    //     name: 'Configuration',
    //     url: '/inventory',
    //     icon: 'icon-settings ',
    //     children: [
    //         {
    //             name: 'Purchase Order',
    //             url: '/inventory/purchaseOrder',
    //             icon: 'icon-fire '
    //         }
    //     ]
    // }
];