import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { UsuarioEmpresa } from '@app-core/models/seguranca/usuarioempresa.model';

@Injectable({
  providedIn: 'root'
})

export class UsuarioEmpresaService extends BaseService<UsuarioEmpresa> {

  constructor() {
    super( 'usuarioempresa');
    this.baseURL = `${this.baseURL}/auth`;
  }

}