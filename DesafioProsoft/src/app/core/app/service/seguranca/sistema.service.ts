import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Sistema } from '@app-core/models/seguranca/sistema.model';

import { RetornoApi } from '@framework-core/models/RetornoApi';
import { Observable } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class SistemaService extends BaseService<Sistema> {

  constructor() {
    super( 'sistema'); 
    this.baseURL = `${this.baseURL}/auth`;
  }

  getListCombo(model: Sistema, page: number, qtdItens: number): Observable<RetornoApi<Sistema[]>> {
    const url = `${this.baseURL}/${this.routeAPI}/combo?QTD_I=${qtdItens}${this.montaQuery(model)}`;

    return this.http.get<RetornoApi<Sistema[]>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }

}