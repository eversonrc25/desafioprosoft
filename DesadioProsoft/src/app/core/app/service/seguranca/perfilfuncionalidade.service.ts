import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { PerfilFuncionalidade } from '@app-core/models/seguranca/perfilfuncionalidade.model';
import { RetornoApi } from '@framework-core/models/RetornoApi';
import { Observable } from 'rxjs';
import { AcoesPerfil, PerfilSistema } from '@app-core/models/seguranca/acoesperfil';
import { retry, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class PerfilFuncionalidadeService extends BaseService<PerfilFuncionalidade> {

  constructor() {
    super( 'perfilfuncionalidade');
    this.baseURL = `${this.baseURL}/auth`;
  }


  getListaSistemas( perf_nr_sequencia: string,   routeParent: String): Observable<RetornoApi<PerfilSistema[]>> {

    const url = `${this.baseURL}/${routeParent}/${perf_nr_sequencia}/getacoesperfil`;

    return this.http.get<RetornoApi<PerfilSistema[]>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }


}