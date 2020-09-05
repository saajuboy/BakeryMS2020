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
        name: 'Test Module',
        url: '/test',
        icon: 'icon-puzzle',
        roles: ['sdsd', 'dsdsd'],
        attributes: { disabled: false }
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
