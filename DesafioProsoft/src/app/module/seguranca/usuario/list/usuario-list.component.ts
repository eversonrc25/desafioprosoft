import { Component } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { UsuarioService } from '@app-core/service/seguranca/usuario.service';
import { BaseListComponent } from '@framework-core/BaseListComponent';
import { Usuario } from '@app-core/models/seguranca/usuario.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

@Component({
  selector: 'app-usuario-list',
  templateUrl: './usuario-list.component.html'
})

export class UsuarioListComponent extends BaseListComponent<Usuario> {

  titulo = 'Lista de Usuário'
  Role = { Create: 'USUA_C', Update : 'USUA_U',  Delete : 'USUA_D', Read: 'USUA_R' }

  constructor(protected store: BaseStore<Usuario>, protected service: UsuarioService,
    protected formBuilder?: FormBuilder ) {
    super(store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));
    this.formFilter.addControl('usua_tx_nome', new FormControl(null, []));
    this.formFilter.addControl('usua_tx_apelido', new FormControl(null, []));
    this.formFilter.addControl('usua_tx_email', new FormControl(null, []));

  }

}