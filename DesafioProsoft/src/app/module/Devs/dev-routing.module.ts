import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import {  DevListComponent } from './dev/list/dev-list.component';
import { DevCrudComponent } from './dev/crud/dev-crud.component';
 




const routes: Routes = [
  {
    path: '',
    children: [ 
      {
        path: 'dev',
        component: DevListComponent,
        data: { title: 'Devs' }
      },
      {
        path: 'dev/create',
        component: DevCrudComponent,
        data: { title: 'Cadastro de Dev' }
      },{
        path: 'dev/read/:id',
        component: DevCrudComponent,
        data: { title: 'Cadastro de Dev' }
      },{
        path: 'dev/update/:id',
        component: DevCrudComponent,
        data: { title: 'Cadastro de Dev' }
      },{
        path: 'dev/delete/:id',
        component: DevCrudComponent,
        data: { title: 'Cadastro de Dev' }
      }

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class DevRoutingModule {}
