import { Injectable } from '@angular/core';
import { BaseState } from '../models/BaseState';
import Utils from '../utils/Utils';

@Injectable()
export class BaseStore<E> {
    private statePai = new BaseState<any>();
    private state = new BaseState<E>();

    constructor() { }

    InitStore(value: string, state: BaseState<E>, clear: boolean = false): { state: BaseState<E>, stateParent: BaseState<any> } {
        let stateReturn: BaseState<E> = { ...state };
        let stateParentReturn: BaseState<any> = {};

        const statusGravado = sessionStorage.getItem(value);
        if (statusGravado !== null) {
            stateReturn = JSON.parse(statusGravado);
        }
        if (clear) {
            sessionStorage.clear();
        }
        sessionStorage.setItem(value, JSON.stringify(stateReturn));

        stateParentReturn = this.InitStatePai(value);


        return { state: stateReturn, stateParent: stateParentReturn };
    }


    InitStatePai(value: string): BaseState<any> {
        let stateReturn: BaseState<any> = {};

        const valuePai =
            value.lastIndexOf('.') === -1
                ? 'ROOT'
                : value.substring(0, value.lastIndexOf('.'));


        const statusGravado = sessionStorage.getItem(valuePai);

        if (statusGravado !== null) {
            stateReturn = JSON.parse(statusGravado);
        }
        return stateReturn;
    }

    setState(node: string, state2: BaseState<E>, value: any): BaseState<E> {
        const newState = {
            ...state2,
            ...value
        };
        sessionStorage.setItem(node, JSON.stringify(newState));
        return newState;
    }

    setObserver<T>(value: any) {
        return Utils.setObserver<T>(value);
    }

    getState(): BaseState<E> {
        return this.state;
    }
    getStatePai<T>(): BaseState<T> {
        return this.statePai;
    }
}
