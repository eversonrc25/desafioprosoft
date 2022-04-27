import { BaseStore } from './utils/BaseStore';
import { BaseService } from './BaseService';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseDetailComponent } from './BaseDetailComponent';
import { Input, Output, EventEmitter, Directive } from '@angular/core';
import { RetornoApi } from './models/RetornoApi';
import { tap, retry, catchError } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';

@Directive()
export abstract class BaseDetailChildComponent<E> extends BaseDetailComponent<E> {

  @Input("nameIdParent") _nameIdParent: string;
  @Input("idParent") _idParent: string;
  @Input("idChild") _idChild: string;
  @Input("action") _action: string;
  @Output() onClickAction = new EventEmitter<any>();
  routeParent: string;

  constructor(nodeName: string, routeParent: string, protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<E>,
    protected service: BaseService<E>,
    protected formBuilder?: FormBuilder) {
    super(activatedRoute, router, store, service, formBuilder);
    this.dataPage.nodeName = nodeName;
    this.routeParent = routeParent;
  }

  getActivatedRoute() {
    this.actionScreen = this._action;
    if (this.actionScreen !== 'create') {
      this.dictID["id"] = this._idChild;
      this.getById(this.dictID["id"]);
      this.readOnly = !((this.actionScreen === 'create') || (this.actionScreen === 'update'));
      this.createForm();
    } else {
      let modelTemp = {} as E;
      modelTemp[this._nameIdParent] = this._idParent;

      this.retornoApi$ = this.store.setObserver<RetornoApi<E>>({ error: false, mensagem: '', dados: { ...modelTemp } });
      this.readOnly = !((this.actionScreen === 'create') || (this.actionScreen === 'update'));
      this.createForm();
      this.setFormDetail(modelTemp)
    }



  }

  getById(id: any) {
    this.retornoApi$ = this.service.getByIdChild(this.routeParent, this._idParent, id).pipe(
      tap(record => this.setFormDetail(record.dados)),
      retry(0),
      catchError(
        (e) => this.handleError(e)));
  }


  onSubmit(exit: boolean) {
    if (this.actionScreen == 'delete')
      return this.onDelete();

    if (this.formDetail.invalid) {
      if (!this.onPosInvalid()) return;
      this.alertMsg();
      return;
    } else {
      this.service.saveChild(this.actionScreen, this.routeParent, this._idParent, this.dictID.id, this.getFormSend())
        .subscribe(
          item => {

            if (!item.error) {
              this.toastr.success(this.onMessageRetorno(true), "Atenção", {
                closeButton: true, enableHtml: true,
              });
              this.onPosSubmit();
              if (exit)
                this.goBack();
              else {
                if (this.actionScreen == 'create') {

                  this._action = 'update';
                  this._idChild = item.dados[this.getId()];
                  this.getActivatedRoute()
                }

              }
            } else {
              this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
                closeButton: true, enableHtml: true,
              });
            }


          },
          error => this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
            closeButton: true, enableHtml: true,
          })
        );
    }

  }

  onDelete() {

    var callback = (result: any): void => {
      if (result.value) {

        this.service.deleteChild(this.routeParent, this._idParent, this.dictID.id)
          .subscribe(
            item => {
              if (!item.error) {
                this.toastr.success(this.onMessageRetorno(true), "Atenção", {
                  closeButton: true, enableHtml: true,
                });
                this.onPosSubmit();
                this.goBack();
              } else {
                this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
                  closeButton: true, enableHtml: true,
                });
              }


            },
            error => this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
              closeButton: true, enableHtml: true,
            })
          );
      }
    }
    this.swal.Confirma("Alerta", "Confirma deleção ?", 'warning', callback);

  }

  goBack() {
    this.onClickAction.emit({ action: null });
  }



}
