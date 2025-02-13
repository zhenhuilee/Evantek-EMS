import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/', // url part
    component: () => import('layouts/FirstLayout.vue'), // which layout you use
    children: [
      { path: '', component: () => import('src/pages/LoginPage.vue') },
    ], // how you structure the page
  },
  {
    path: '/module',
    component: () => import('layouts/ModuleLayout.vue'),
    children: [
      { path: '', component: () => import('src/pages/ModulePage.vue') },
    ],
  },
  {
    path: '/user/attendance',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/User/AttendancePage.vue'),
      },
    ],
  },
  {
    path: '/user/changepassword',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/User/ChangePasswordPage.vue'),
      },
    ],
  },
  {
    path: '/admin/users',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      { path: '', component: () => import('src/pages/Admin/UserPage.vue') },
    ],
  },
  {
    path: '/admin/statuses',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      { path: '', component: () => import('src/pages/Admin/StatusPage.vue') },
    ],
  },
  {
    path: '/display',
    component: () => import('layouts/FirstLayout.vue'),
    children: [
      { path: '', component: () => import('src/pages/DisplayPage.vue') },
    ],
  },
  {
    path: '/admin/all-incidents',
    component: () => import('layouts/ThirdLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Admin/IncidentDisplayPage.vue'),
      },
    ],
  },
  {
    path: '/engineer/edit-incidents/:id',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Engineer/EditIncidentPage.vue'),
      },
    ],
  },
  {
    path: '/admin/create',
    component: () => import('layouts/ThirdLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Admin/CreateIncidentPage.vue'),
      },
    ],
  },
  {
    path: '/admin/view-signature/:id',
    component: () => import('layouts/ThirdLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Admin/SignaturePage.vue'),
      },
    ],
  },
  {
    path: '/engineer/view-incidents/:id',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Engineer/ViewIncidentPage.vue'),
      },
    ],
  },
  {
    path: '/admin/search',
    component: () => import('layouts/ThirdLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Admin/SearchIncidentPage.vue'),
      },
    ],
  },
  {
    path: '/admin/edit/:id',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Admin/EditIncidentPage.vue'),
      },
    ],
  },
  {
    path: '/engineer/all-incidents',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Engineer/IncidentDisplayPage.vue'),
      },
    ],
  },

  {
    path: '/engineer/signature/:id',
    component: () => import('layouts/SecondLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('src/pages/Engineer/SignaturePage.vue'),
      },
    ],
  },

  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];
export default routes;
