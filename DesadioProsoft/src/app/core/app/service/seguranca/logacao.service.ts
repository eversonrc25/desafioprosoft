import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Usuario } from '@app-core/models/seguranca/usuario.model';
import { Log_Acao } from '@app-core/models/seguranca/logacao.model';
import { RetornoApi } from '@framework-core/models/RetornoApi';
import { Observable } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class LogAcaoService extends BaseService<Log_Acao> {

  constructor() {
    super( 'logacao');
    this.baseURL = `${this.baseURL}/auth`;
  }

  getListComboUsuario(model: Usuario, page: number, qtdItens: number): Observable<RetornoApi<Usuario[]>> {
    const url = `${this.baseURL}/${this.routeAPI}/combousuario?PAG_C=${page}&QTD_I=${qtdItens}${this.montaQuery(model)}`;

    return this.http.get<RetornoApi<Usuario[]>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }

}