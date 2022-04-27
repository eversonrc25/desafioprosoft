import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

 
import { EmpresaListComponent } from './empresa/list/empresa-list.component'
import { EmpresaCrudComponent } from './empresa/crud/empresa-crud.component'
import { SistemaListComponent } from './sistema/list/sistema-list.component'
import { SistemaCrudComponent } from './sistema/crud/sistema-crud.component'

import { PerfilListComponent } from './perfil/list/perfil-list.component'
import { PerfilCrudComponent } from './perfil/crud/perfil-crud.component'
import { SistemaFuncionalidadeListComponent } from './sistemafuncionalidade/list/sistemafuncionalidade-list.component'
import { SistemaFuncionalidadeCrudComponent } from './sistemafuncionalidade/crud/sistemafuncionalidade-crud.component'

import { UsuarioListComponent } from './usuario/list/usuario-list.component'
import { UsuarioCrudComponent } from './usuario/crud/usuario-crud.component'
//import { AuthGuard } from '@framework-core/auth/AuthGuard'
import { AuthGuard } from '../../core/framework/auth/AuthGuard'
import { LogacaoListComponent } from './logacao/list/logacao-list.component'
//import { AuthGuard } from 'src/app/auth.guard'
//import { AuthGuard } from '@framework-core/auth/AuthGuard'


const routes: Routes = [
  {
    path: '',
    children: [

      

      {
        path: 'empresa',
        canActivate: [AuthGuard],
        component: EmpresaListComponent,
        data: { title: 'Empresa' }
      },
      {
        path: 'empresa/create',
        canActivate: [AuthGuard],
        component: EmpresaCrudComponent,
        data: { title: 'Cadastro de Empresa' }
      },{
        path: 'empresa/read/:id',
        canActivate: [AuthGuard],
        component: EmpresaCrudComponent,
        data: { title: 'Cadastro de Empresa' }
      },{
        path: 'empresa/update/:id',
        canActivate: [AuthGuard],
        component: EmpresaCrudComponent,
        data: { title: 'Cadastro de Empresa' }
      },{
        path: 'empresa/delete/:id',
        canActivate: [AuthGuard],
        component: EmpresaCrudComponent,
        data: { title: 'Cadastro de Empresa' }
      },

      {
        path: 'sistema',
        canActivate: [AuthGuard],
        component: SistemaListComponent,
        data: { title: 'Sistema' }
      },
      {
        path: 'sistema/create',
        canActivate: [AuthGuard],
        component: SistemaCrudComponent,
        data: { title: 'Cadastro de Sistema' }
      },{
        path: 'sistema/read/:id',
        canActivate: [AuthGuard],
        component: SistemaCrudComponent,
        data: { title: 'Cadastro de Sistema' }
      },{
        path: 'sistema/update/:id',
        canActivate: [AuthGuard],
        component: SistemaCrudComponent,
        data: { title: 'Cadastro de Sistema' }
      },{
        path: 'sistema/delete/:id',
        canActivate: [AuthGuard],
        component: SistemaCrudComponent,
        data: { title: 'Cadastro de Sistema' }
      },
       

      {
        path: 'perfil',
        canActivate: [AuthGuard],
        component: PerfilListComponent,
        data: { title: 'Perfil' }
      },
      {
        path: 'perfil/create',
        canActivate: [AuthGuard],
        component: PerfilCrudComponent,
        data: { title: 'Cadastro de Perfil' }
      },{
        path: 'perfil/read/:id',
        canActivate: [AuthGuard],
        component: PerfilCrudComponent,
        data: { title: 'Cadastro de Perfil' }
      },{
        path: 'perfil/update/:id',
        canActivate: [AuthGuard],
        component: PerfilCrudComponent,
        data: { title: 'Cadastro de Perfil' }
      },{
        path: 'perfil/delete/:id',
        canActivate: [AuthGuard],
        component: PerfilCrudComponent,
        data: { title: 'Cadastro de Perfil' }
      },
 
      {
        path: 'sistemafuncionalidade',
        canActivate: [AuthGuard],
        component: SistemaFuncionalidadeListComponent,
        data: { title: 'SistemaFuncionalidade' }
      },
      {
        path: 'sistemafuncionalidade/create',
        canActivate: [AuthGuard],
        component: SistemaFuncionalidadeCrudComponent,
        data: { title: 'Cadastro de SistemaFuncionalidade' }
      },{
        path: 'sistemafuncionalidade/read/:id',
        canActivate: [AuthGuard],
        component: SistemaFuncionalidadeCrudComponent,
        data: { title: 'Cadastro de SistemaFuncionalidade' }
      },{
        path: 'sistemafuncionalidade/update/:id',
        canActivate: [AuthGuard],
        component: SistemaFuncionalidadeCrudComponent,
        data: { title: 'Cadastro de SistemaFuncionalidade' }
      },{
        path: 'sistemafuncionalidade/delete/:id',
        canActivate: [AuthGuard],
        component: SistemaFuncionalidadeCrudComponent,
        data: { title: 'Cadastro de SistemaFuncionalidade' }
      },

      {
        path: 'usuario',
        component: UsuarioListComponent,
        canActivate: [AuthGuard],
        data: { title: 'Usuario' }
      },
      {
        path: 'usuario/create',
        component: UsuarioCrudComponent,
        canActivate: [AuthGuard],
        data: { title: 'Cadastro de Usuario' }
      },{
        path: 'usuario/read/:id',
        component: UsuarioCrudComponent,
        canActivate: [AuthGuard],
        data: { title: 'Cadastro de Usuario' }
      },{
        path: 'usuario/update/:id',
        component: UsuarioCrudComponent,
        canActivate: [AuthGuard],
        data: { title: 'Cadastro de Usuario' }
      },{
        path: 'usuario/delete/:id',
        component: UsuarioCrudComponent,
        canActivate: [AuthGuard],
        data: { title: 'Cadastro de Usuario' }
      },
      {
        path: 'logacao',
        component: LogacaoListComponent,
        data: { title: 'Log de Ações' }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class SegurancaRoutingModule {}
