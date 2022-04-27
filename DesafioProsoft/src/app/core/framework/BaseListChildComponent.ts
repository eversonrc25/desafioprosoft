import { BaseStore } from './utils/BaseStore';
import { BaseService } from './BaseService';
import { catchError, retry } from 'rxjs/operators';
import { BaseListComponent } from './BaseListComponent';
import { Input, Output, EventEmitter, Directive } from '@angular/core';
import { FormBuilder } from '@angular/forms';

@Directive()
export abstract class BaseListChildComponent<E> extends BaseListComponent<E> {

  @Input("nameIdParent") _nameIdParent : string;
  @Input("idParent") _idParent: String;
  @Output() onClickAction = new EventEmitter<any>();
  routeParent: String;

  constructor(nodeName: string, routeParent: string, protected store: BaseStore<E>, 
    protected service: BaseService<E>,  protected formBuilder?: FormBuilder) {

    super(store, service, formBuilder);
    this.dataPage.isClear = false; // Não Limpa o Application
    this.dataPage.nodeName = nodeName;
    this.routeParent = routeParent;
  }

  list(model: any, searchPage: number, pageSize: number) {

    this.dados$ = this.service.getListChild(model, this._idParent, this.routeParent, searchPage, pageSize).pipe(
      retry(0),
      catchError(
        (e) => this.handleError(e)));
  }


  clickAction(action: string, id: string) {
    this.onClickAction.emit({ action: action, idChild: id });
  }



}
