import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module'
import { NgbModule } from '@ng-bootstrap/ng-bootstrap'
import { NgSelectModule } from '@ng-select/ng-select';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { SegurancaRoutingModule } from './seguranca-routing.module';
import { EmpresaListComponent } from './empresa/list/empresa-list.component'
import { EmpresaCrudComponent } from './empresa/crud/empresa-crud.component'
import { SistemaListComponent } from './sistema/list/sistema-list.component'
import { SistemaCrudComponent } from './sistema/crud/sistema-crud.component'

import { PerfilListComponent } from './perfil/list/perfil-list.component'
import { PerfilCrudComponent } from './perfil/crud/perfil-crud.component'
import { PerfilFuncionalidadeComponent } from './perfil/detail/perfilfuncionalidade/perfilfuncionalidade.component'
import { PerfilFuncionalidadeListComponent } from './perfil/detail/perfilfuncionalidade/list/perfilfuncionalidade-list.component'
import { PerfilFuncionalidadeCrudComponent } from './perfil/detail/perfilfuncionalidade/crud/perfilfuncionalidade-crud.component'

import { SistemaFuncionalidadeListComponent } from './sistemafuncionalidade/list/sistemafuncionalidade-list.component'
import { SistemaFuncionalidadeCrudComponent } from './sistemafuncionalidade/crud/sistemafuncionalidade-crud.component'
import { SistemaFuncionalidadeAcaoComponent } from './sistemafuncionalidade/detail/sistemafuncionalidadeacao/sistemafuncionalidadeacao.component'
import { SistemaFuncionalidadeAcaoListComponent } from './sistemafuncionalidade/detail/sistemafuncionalidadeacao/list/sistemafuncionalidadeacao-list.component'
import { SistemaFuncionalidadeAcaoCrudComponent } from './sistemafuncionalidade/detail/sistemafuncionalidadeacao/crud/sistemafuncionalidadeacao-crud.component'

import { UsuarioListComponent } from './usuario/list/usuario-list.component'
import { UsuarioCrudComponent } from './usuario/crud/usuario-crud.component'
import { UsuarioPerfilComponent } from './usuario/detail/usuarioperfil/usuarioperfil.component'
import { UsuarioPerfilListComponent } from './usuario/detail/usuarioperfil/list/usuarioperfil-list.component'
import { UsuarioPerfilCrudComponent } from './usuario/detail/usuarioperfil/crud/usuarioperfil-crud.component'
import { UsuarioEmpresaComponent } from './usuario/detail/usuarioempresa/usuarioempresa.component'
import { UsuarioEmpresaListComponent } from './usuario/detail/usuarioempresa/list/usuarioempresa-list.component'
import { UsuarioEmpresaCrudComponent } from './usuario/detail/usuarioempresa/crud/usuarioempresa-crud.component'
import { LogacaoListComponent } from './logacao/list/logacao-list.component'

@NgModule({
  declarations: [

    EmpresaListComponent, EmpresaCrudComponent,
    SistemaListComponent, SistemaCrudComponent,

    PerfilListComponent, PerfilCrudComponent,
    PerfilFuncionalidadeComponent, PerfilFuncionalidadeListComponent, PerfilFuncionalidadeCrudComponent,

    SistemaFuncionalidadeListComponent, SistemaFuncionalidadeCrudComponent,
    SistemaFuncionalidadeAcaoComponent, SistemaFuncionalidadeAcaoListComponent, SistemaFuncionalidadeAcaoCrudComponent,
    UsuarioListComponent, UsuarioCrudComponent,
    UsuarioPerfilComponent, UsuarioPerfilListComponent, UsuarioPerfilCrudComponent,
    UsuarioEmpresaComponent, UsuarioEmpresaListComponent, UsuarioEmpresaCrudComponent,
    LogacaoListComponent

  ],
  imports: [CommonModule,
    NgbModule,
    NgSelectModule, FormsModule,
    SharedModule,
    ReactiveFormsModule,
    SegurancaRoutingModule],
  providers: [BaseStore]
})
export class SegurancaModule { }
