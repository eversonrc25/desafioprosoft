import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
 
import { UserDirective } from '@framework-core/diretive/useraccess.directive';
import { LoadingComponent } from '@app-core/component/loading/loading.component';
import { NodataComponent } from '@app-core/component/nodata/nodata.component';
import { ShowerrorComponent } from '@app-core/component/showerror/showerror.component';
 

@NgModule({
    declarations: [
        NodataComponent,
        LoadingComponent,
        ShowerrorComponent,
        UserDirective 
    ],
    imports: [
        CommonModule,
        RouterModule,
        FormsModule, ReactiveFormsModule, NgSelectModule,
        NgbModule
    ],
    exports: [FormsModule, ReactiveFormsModule,
        NodataComponent,
        LoadingComponent,
        ShowerrorComponent,
        UserDirective 
    ],
 
})
export class SharedModule { }

