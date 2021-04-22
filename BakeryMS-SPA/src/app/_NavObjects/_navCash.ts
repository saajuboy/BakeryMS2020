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
          }
        ]
      },
      {
        name: 'Reports',
        url: '/reports',
        icon: 'icon-docs ',
        children: [
          {
            name: 'Sales Reports',
            url: '/reports/sales',
            icon: 'icon-list'
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
];