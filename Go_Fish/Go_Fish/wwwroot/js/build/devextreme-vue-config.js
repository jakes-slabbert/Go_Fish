import DxSelectBox from 'devextreme-vue/select-box';
import { DxDateBox } from 'devextreme-vue/date-box';
import DxDataGrid from 'devextreme-vue/data-grid';
import DxFileUploader from 'devextreme-vue/file-uploader';
import DxDropDownBox from 'devextreme-vue/drop-down-box';
import ArrayStore from 'devextreme/data/array_store';
import DataSource from 'devextreme/data/data_source';
import DxValidationSummary from 'devextreme-vue/validation-summary';
import {
    DxValidator,
    DxRequiredRule,
    DxCompareRule,
    DxEmailRule,
    DxPatternRule,
    DxStringLengthRule,
    DxRangeRule,
    DxAsyncRule
} from 'devextreme-vue/validator';

Vue.component('dx-validator', DxValidator);
Vue.component('dx-required-rule', DxRequiredRule);
Vue.component('dx-validation-summary', DxValidationSummary);

Vue.component('dx-select-box', DxSelectBox);
Vue.component('dx-date-box', DxDateBox);
Vue.component('dx-data-grid', DxDataGrid);
Vue.component('dx-file-uploader', DxFileUploader);
Vue.component('dx-dropdown-box', DxDropDownBox);
Vue.component('dx-selection', DxDataGrid);