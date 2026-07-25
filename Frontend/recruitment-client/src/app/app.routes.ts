import { Routes } from '@angular/router';

import { Landing } from './landing/landing';

import { Login } from './auth/login/login';
import { Register } from './auth/register/register';

import { CandidateDashboard } from './candidate-dashboard/candidate-dashboard';
import { RecruiterDashboard } from './recruiter-dashboard/recruiter-dashboard';
import { HiringManagerDashboard } from './hiring-manager-dashboard/hiring-manager-dashboard';
import { AdminDashboard } from './admin-dashboard/admin-dashboard';

export const routes: Routes = [

  // Landing Page
  {
    path: '',
    component: Landing
  },

  // Authentication
  {
    path: 'login',
    component: Login
  },
  {
    path: 'register',
    component: Register
  },

  // Dashboards
  {
    path: 'candidate',
    component: CandidateDashboard
  },
  {
    path: 'recruiter',
    component: RecruiterDashboard
  },
  {
    path: 'hiring-manager',
    component: HiringManagerDashboard
  },
  {
    path: 'admin',
    component: AdminDashboard
  },

  // Invalid URL
  {
    path: '**',
    redirectTo: ''
  }

];