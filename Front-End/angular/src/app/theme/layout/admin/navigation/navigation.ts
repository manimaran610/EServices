export interface NavigationItem {
  id: string;
  title: string;
  type: 'item' | 'collapse' | 'group';
  translate?: string;
  icon?: string;
  hidden?: boolean;
  url?: string;
  classes?: string;
  exactMatch?: boolean;
  external?: boolean;
  target?: boolean;
  breadcrumbs?: boolean;
  badge?: {
    title?: string;
    type?: string;
  };
  children?: NavigationItem[];
}

export const NavigationItems: NavigationItem[] = [

  {
    id: 'ui-component',
    title: '',
    type: 'group',
    icon: 'icon-group',
    children: [
      {
        id: 'Home',
        title: 'Home',
        type: 'item',
        url: '/sample-page',
        icon: 'feather icon-home',


      },
      {
        id: 'ApplicationForm',
        title: 'Your Forms',
        type: 'item',
        url: '/home',
        icon: 'feather icon-file-text',
       
      },
    ]
  },
];
