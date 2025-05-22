import { NgModule } from '@angular/core';
import {
    NgxMatDatepickerActions,
    NgxMatDatepickerApply,
    NgxMatDatepickerCancel,
    NgxMatDatepickerClear,
    NgxMatDatepickerInput,
    NgxMatDatetimepicker,
} from '@ngxmc/datetime-picker';

const datePickerModules = [
    NgxMatDatepickerActions,
    NgxMatDatepickerApply,
    NgxMatDatepickerCancel,
    NgxMatDatepickerClear,
    NgxMatDatepickerInput,
    NgxMatDatetimepicker
];

@NgModule({
    imports: datePickerModules,
    exports: datePickerModules
})
export default class DatePickerModule { }