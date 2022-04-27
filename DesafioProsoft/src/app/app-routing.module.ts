import { Routes } from '@angular/router';
import { FullComponent } from './layouts/full/full.component';

export const Approutes: Routes = [
  {
    path: '',
    component: FullComponent,
   // canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: '/auth', pathMatch: 'full' },
      {
        path: 'starter',
        loadChildren: () => import('./starter/starter.module').then(m => m.StarterModule)
      },
 
      {
        path: 'seguranca',
        loadChildren: () => import('./module/seguranca/seguranca.module').then(m => m.SegurancaModule)
      } ,
      {
        path: 'dev',
        loadChildren: () => import('./module/Devs/dev.module').then(m => m.DevModule)
      }
    ]
  },
  {
    path: 'auth',
    loadChildren: () => import('./module/authentication/authentication.module').then(m => m.AuthenticationModule)
  }
];

