import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer',
    badge: {
      variant: 'info',
      text: 'NEW'
    }
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
        name: 'Purchase Order',
        url: '/inventory/purchaseOrder',
        icon: 'icon-briefcase',
        children: [
          {
            name: 'Purchase Order List',
            url: '/inventory/purchaseOrder',
            icon: 'icon-list'
          },
          {
            name: 'Create Pur.Order',
            url: '/inventory/purchaseOrder/create',
            icon: 'icon-note'
          }
        ]
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
        name: 'Employees',
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
        icon: 'icon-fire '
      },
      {
        name: 'Supplier',
        url: '/master/supplier',
        icon: 'icon-fire '
      },
      {
        name: 'Unit',
        url: '/master/unit',
        icon: 'icon-fire '
      }
    ]
  },
  {
    name: 'User',
    url: '/User',
    icon: 'icon-user',
    children: [
      {
        name: 'Register',
        url: '/user/register',
        icon: 'icon-user-follow',
      },
      {
        name: 'List',
        url: '/user/list',
        icon: 'icon-list'
      },
      {
        name: 'Edit Profile',
        url: 'test',
        icon: 'icon-wrench'
      }
    ]
  },
  {
    name: 'Test Module',
    url: '/test',
    icon: 'icon-puzzle',
    roles: ['sdsd', 'dsdsd'],
    attributes: { disabled: false }
  },
  {
    title: true,
    name: 'Theme'
  },
  {
    name: 'Colors',
    url: '/theme/colors',
    icon: 'icon-drop'
  },
  {
    name: 'Typography',
    url: '/theme/typography',
    icon: 'icon-pencil'
  },
  {
    title: true,
    name: 'Components'
  },
  {
    name: 'Base',
    url: '/base',
    icon: 'icon-puzzle',
    children: [
      {
        name: 'Cards',
        url: '/base/cards',
        icon: 'icon-puzzle'
      },
      {
        name: 'Carousels',
        url: '/base/carousels',
        icon: 'icon-puzzle'
      },
      {
        name: 'Collapses',
        url: '/base/collapses',
        icon: 'icon-puzzle'
      },
      {
        name: 'Forms',
        url: '/base/forms',
        icon: 'icon-puzzle'
      },
      {
        name: 'Navbars',
        url: '/base/navbars',
        icon: 'icon-puzzle'

      },
      {
        name: 'Pagination',
        url: '/base/paginations',
        icon: 'icon-puzzle'
      },
      {
        name: 'Popovers',
        url: '/base/popovers',
        icon: 'icon-puzzle'
      },
      {
        name: 'Progress',
        url: '/base/progress',
        icon: 'icon-puzzle'
      },
      {
        name: 'Switches',
        url: '/base/switches',
        icon: 'icon-puzzle'
      },
      {
        name: 'Tables',
        url: '/base/tables',
        icon: 'icon-puzzle'
      },
      {
        name: 'Tabs',
        url: '/base/tabs',
        icon: 'icon-puzzle'
      },
      {
        name: 'Tooltips',
        url: '/base/tooltips',
        icon: 'icon-puzzle'
      }
    ]
  },
  {
    name: 'Buttons',
    url: '/buttons',
    icon: 'icon-cursor',
    children: [
      {
        name: 'Buttons',
        url: '/buttons/buttons',
        icon: 'icon-cursor'
      },
      {
        name: 'Dropdowns',
        url: '/buttons/dropdowns',
        icon: 'icon-cursor'
      },
      {
        name: 'Brand Buttons',
        url: '/buttons/brand-buttons',
        icon: 'icon-cursor'
      }
    ]
  },
  {
    name: 'Charts',
    url: '/charts',
    icon: 'icon-pie-chart'
  },
  {
    name: 'Icons',
    url: '/icons',
    icon: 'icon-star',
    children: [
      {
        name: 'CoreUI Icons',
        url: '/icons/coreui-icons',
        icon: 'icon-star',
        badge: {
          variant: 'success',
          text: 'NEW'
        }
      },
      {
        name: 'Flags',
        url: '/icons/flags',
        icon: 'icon-star'
      },
      {
        name: 'Font Awesome',
        url: '/icons/font-awesome',
        icon: 'icon-star',
        badge: {
          variant: 'secondary',
          text: '4.7'
        }
      },
      {
        name: 'Simple Line Icons',
        url: '/icons/simple-line-icons',
        icon: 'icon-star'
      }
    ]
  },
  {
    name: 'Notifications',
    url: '/notifications',
    icon: 'icon-bell',
    children: [
      {
        name: 'Alerts',
        url: '/notifications/alerts',
        icon: 'icon-bell'
      },
      {
        name: 'Badges',
        url: '/notifications/badges',
        icon: 'icon-bell'
      },
      {
        name: 'Modals',
        url: '/notifications/modals',
        icon: 'icon-bell'
      }
    ]
  },
  {
    name: 'Widgets',
    url: '/widgets',
    icon: 'icon-calculator',
    badge: {
      variant: 'info',
      text: 'NEW'
    }
  },

  {
    divider: true
  },
  {
    title: true,
    name: 'Extras',
  },
  {
    name: 'Pages',
    url: '/pages',
    icon: 'icon-star',
    children: [
      {
        name: 'Login',
        url: '/login',
        icon: 'icon-star'
      },
      {
        name: 'Register',
        url: '/register',
        icon: 'icon-star'
      },
      {
        name: 'Error 404',
        url: '/404',
        icon: 'icon-star'
      },
      {
        name: 'Error 500',
        url: '/500',
        icon: 'icon-star'
      }
    ]
  },
  {
    name: 'Disabled',
    url: '/dashboard',
    icon: 'icon-ban',
    badge: {
      variant: 'secondary',
      text: 'NEW'
    },
    attributes: { disabled: true },
  }
];
