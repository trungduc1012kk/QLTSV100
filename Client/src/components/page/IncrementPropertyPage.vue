<template>
  <div class="increment-page main">
    <div class="increment-control flex">
      <div class="title">
        <h2>Ghi tăng tài sản</h2>
      </div>
      <div class="control">
        <button class="button main-button" @click="onClickAdd">Thêm</button>
        <ms-tool-tip content="Xóa" position="bottom">
          <button class="btn" @click="onClickDelete">
            <div class="icon">
              <div class="icon-Delete"></div>
            </div>
          </button>
        </ms-tool-tip>
        <div class="control-item" @click="isShowSelectMode = !isShowSelectMode">
          <div class="icon">
            <div
              :class="
                mode == 'horizontal'
                  ? 'icon-mode-horizontal'
                  : 'icon-mode-vertical'
              "
            ></div>
          </div>
          <div class="icon">
            <div class="icon-down-black"></div>
          </div>
          <div class="select-mode" v-show="isShowSelectMode">
            <div class="mode-item flex" @click="selectModeHorizontal">
              <div class="icon">
                <div class="icon-mode-horizontal"></div>
              </div>
              <div class="text">Giao diện ngang</div>
            </div>
            <div class="mode-item flex" @click="selectModeVertical">
              <div class="icon">
                <div class="icon-mode-vertical"></div>
              </div>
              <div class="text">Giao diện dọc</div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="increment-container">
      <splitpanes class="detail-master default-theme" horizontal size="100">
        <pane :size="masterSize">
          <div class="master">
            <div class="control flex">
              <div class="control-left">
                <div class="form-input">
                  <div class="icon">
                    <div class="icon-search-black"></div>
                  </div>
                  <input
                    class="input input-icon"
                    ref="formSearch"
                    placeholder="Tìm kiếm theo số chứng từ, nội dung"
                    v-model="keyword"
                    @keypress.enter="getIncrementProperty"
                    @focus="onFocusFormSearch"
                  />
                </div>
              </div>
              <div class="control-right">
                <div class="icon">
                  <div class="icon-print"></div>
                </div>
                <div class="icon">
                  <div class="icon-more"></div>
                </div>
              </div>
            </div>
            <ms-table
              ref="tableIncrementProperty"
              class="table-increment"
              :hasTotal="true"
              :initData="incrementProperties"
              :init-columns="columnsForTableIncrementProperty"
              :has-select="true"
              @select="selectItem"
              @deleteItem="onDeleteIncrementProperty"
              @editItem="onEditIncrementProperty"
              @on-dbl-click="onEditIncrementProperty"
              @on-context-Menu="onContextMenu"
              :auto-select="true"
            ></ms-table>
            <div class="table-footer">
              <div class="total-Property">
                Tổng số: <span>{{ totalIncrementProperties }} bản ghi</span>
              </div>
              <ms-combobox
                class="footer-combobox"
                hasIcon="false"
                :items="arrPage"
                :itemDefault="true"
                @comboboxSelectedItem="selectedPageSize"
              />
              <ms-paging
                :maxPage="maxPage"
                :pageNumber="pageNumber"
                @nextPage="nextPage"
                @prevPage="prevPage"
                @changePageNumber="changePageNumber"
              />
            </div>
          </div>
        </pane>
        <pane :size="detailSize" v-if="isShowDetail">
          <div class="detail">
            <div class="table-header flex">
              <div class="title">
                <h3>Thông tin chi tiết</h3>
              </div>
              <div class="control-item">
                <div class="icon-zoomout" @click="zoomOutDetail"></div>
              </div>
            </div>
            <ms-table
              class="table-increment-detail"
              :initData="properties"
              :init-columns="columnsForTableDetail"
              :hasTotal="true"
            >
            </ms-table>
          </div>
        </pane>
      </splitpanes>
    </div>

    <!-- form thêm mới chứng từ ghi tăng -->
    <ms-popup
      title="Thêm chứng từ ghi tăng"
      v-if="isShowFormPopupIncrement"
      @closePopup="closePopupIncrementProperty"
      class="form-increment"
    >
      <template #body>
        <div class="form-increment-body">
          <div class="form-top">
            <h3>Thông tin chứng từ</h3>
            <div class="form-top-body">
              <div class="row">
                <div class="col">
                  <label for="">Mã chứng từ<span>*</span></label>
                  <div class="form-input">
                    <input
                      type="text"
                      v-model="this.incrementProperty.incrementPropertyCode"
                      tabindex="1"
                      @blur="validate('incrementPropertyCode')"
                      :class="
                        error.incrementPropertyCode != '' ? 'border-red' : ''
                      "
                    />
                  </div>
                  <p class="error" v-if="error.incrementPropertyCode != ''">
                    <span>Mã chứng từ </span>{{ error.incrementPropertyCode }}
                  </p>
                </div>
                <div class="col">
                  <label for="">Ngày bắt đầu<span>*</span></label>
                  <div class="form-input">
                    <el-date-picker
                      v-model="incrementProperty.voucherDate"
                      type="date"
                      :format="dateConfig.Format"
                      value-format="YYYY-MM-DD"
                      @blur="validate('voucherDate')"
                      tabindex="2"
                      :class="error.voucherDate != '' ? 'border-red' : ''"
                    >
                    </el-date-picker>
                  </div>
                  <p class="error" v-if="error.voucherDate != ''">
                    <span>Ngày bắt đầu sử dụng </span>{{ error.voucherDate }}
                  </p>
                </div>
                <div class="col">
                  <label for="">Ngày ghi tăng<span>*</span></label>
                  <div class="form-input">
                    <el-date-picker
                      v-model="incrementProperty.incrementDate"
                      type="date"
                      :format="dateConfig.Format"
                      value-format="YYYY-MM-DD"
                      tabindex="3"
                      @blur="validate('incrementDate')"
                      :class="error.incrementDate != '' ? 'border-red' : ''"
                    >
                    </el-date-picker>
                  </div>
                  <p class="error" v-if="error.incrementDate != ''">
                    <span>Ngày ghi tăng </span>{{ error.incrementDate }}
                  </p>
                </div>
              </div>
              <div class="row">
                <div class="col">
                  <label for="">Nội dung</label>
                  <div class="form-input">
                    <input
                      type="text"
                      v-model="incrementProperty.description"
                      tabindex="4"
                    />
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="form-bottom">
            <h3>Thông tin chi tiết</h3>
            <div class="form-bottom-body">
              <div class="form-bottom-body-control flex">
                <div class="control-left">
                  <div class="form-input">
                    <div class="icon">
                      <div class="icon-search-black"></div>
                    </div>
                    <input
                      class="input input-icon"
                      placeholder="Tìm kiếm theo mã, tên tài sản"
                      v-model="keywordSearchProperty"
                      @keypress.enter="searchProperty"
                    />
                  </div>
                </div>
                <div class="control-right">
                  <button
                    class="button outline-button"
                    @click="onClickSelectProperty"
                    tabindex="5"
                  >
                    Chọn tài sản
                  </button>
                </div>
              </div>
              <ms-table
                class="table-property"
                :initData="listPropertyShow"
                :init-columns="columnsForTablePopupAdd"
                @delete-item="deleteProperty"
                @edit-item="editProperty"
                @on-dbl-click="editProperty"
                :hasTotal="true"
              >
              </ms-table>
            </div>
          </div>
        </div>
      </template>
      <template #control>
        <button
          class="button outline-button"
          tabindex="6"
          @click="closePopupIncrementProperty"
        >
          Hủy
        </button>
        <button class="button main-button" tabindex="7" @click="onClickSave">
          Lưu
        </button>
      </template>
    </ms-popup>

    <!-- form chọn tài sản -->
    <ms-popup
      class="form-select-property"
      v-if="isShowFormSelectProperty"
      title="Chọn tài sản ghi tăng"
      @closePopup="isShowFormSelectProperty = false"
    >
      <template #body>
        <div class="control flex">
          <div class="form-input">
            <div class="icon">
              <div class="icon-search-black"></div>
            </div>
            <input
              class="input input-icon"
              ref="formSearchProperty"
              placeholder="Tìm kiếm mã, tên tài sản"
              v-model="keywordFilterProperty"
              @keypress.enter="getProperties"
              @focus="onFocusFormSearch"
            />
          </div>
        </div>
        <ms-table
          :has-select="true"
          class="table-select-property"
          :initData="initPropertiesSelect"
          :init-columns="columnsForTableProperty"
          ref="tableSelectProperty"
        >
        </ms-table>
        <div class="table-footer">
          <div class="total-Property">
            Tổng số: <span>{{ totalProperty }} bản ghi</span>
          </div>
          <ms-combobox
            class="footer-combobox"
            hasIcon="false"
            :items="arrPageProperty"
            :itemDefault="true"
            @comboboxSelectedItem="selectedPropertyPageSize"
          />
          <ms-paging
            :maxPage="maxPagePropertiesSelect"
            :pageNumber="selectPropertyPageNumber"
            @nextPage="nextPropertyPage"
            @prevPage="prevPropertyPage"
            @changePageNumber="changePropertyPageNumber"
          /></div
      ></template>
      <template #control>
        <button
          class="button outline-button"
          @click="isShowFormSelectProperty = false"
        >
          Hủy bỏ
        </button>
        <button class="button main-button" @click="onClickConfirmSelect">
          Đồng ý
        </button>
      </template>
    </ms-popup>

    <!-- form sửa nguồn tài sản -->
    <ms-popup
      class="form-edit-property"
      v-if="isShowFormPopupEditProperty"
      title="Sửa tài sản"
      @closePopup="isShowFormPopupEditProperty = false"
    >
      <template #body>
        <label for="">Tên bộ phận sử dụng </label>
        <div class="row">
          <div class="form-input col1 col-department">
            <input
              class="input"
              type="text"
              readonly
              v-model="departmentInFormEdit"
            />
          </div>
        </div>
        <div class="row"><h4>Nguyên giá</h4></div>
        <div class="row">
          <div class="col1">Nguồn hình thành</div>
          <div class="col2">Giá trị</div>
          <div class="col3"></div>
        </div>
        <div class="form-add-budget">
          <div
            class="row"
            v-for="(budget, index) in budgetsOfProperty"
            :key="index"
          >
            <div class="col1">
              <ms-combobox
                :items="budgets"
                fieldName="budgetName"
                :initValue="budget.budgetName"
                @comboboxSelectedItem="
                  (item) => (budget.budgetName = item.budgetName)
                "
                :border="indexBudgetError.includes(index) ? 'border-red' : ''"
              />
            </div>
            <div class="col2">
              <ms-input-number
                :decimalPlaces="0"
                v-model="budget.budgetPrice"
                :border="indexBudgetError.includes(index) ? 'border-red' : ''"
              />
            </div>
            <div class="col3">
              <div class="form-edit-control flex">
                <div class="icon" @click="addBudget">
                  <div class="icon-add"></div>
                </div>
                <div
                  class="icon"
                  @click="removeBudget(budget)"
                  v-if="budgetsOfProperty.length > 1"
                >
                  <div class="icon-remove"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="row total-budget">
          <div class="col1">
            <input class="input" type="text" readonly value="Tổng" />
          </div>
          <div class="col2">
            <ms-input-number :decimalPlaces="0" v-model="markedPrice" />
          </div>
          <div class="col3"></div>
        </div>
      </template>
      <template #control>
        <button
          class="button outline-button"
          @click="isShowFormPopupEditProperty = false"
        >
          Hủy
        </button>
        <button class="button main-button" @click="onClickSaveProperty">
          Lưu
        </button>
      </template>
    </ms-popup>

    <!-- form xác nhận của trang chính -->
    <ms-form-confirm
      v-if="isShowFormConfirm"
      :commandName="commandName"
      @returnTrue="formConfirmReturnTrue"
      @returnFalse="formConfirmReturnFalse"
    >
      <!-- thông báo khi chưa chọn chứng từ nào  -->
      <div v-if="commandName == 'notice'">
        {{ contentFormConfirm }}
      </div>

      <!-- hiện xác nhận khi xóa 1  -->
      <div v-if="isShowDelete1">
        Bạn có muốn xóa chứng từ ghi tăng
        <strong>{{ incrementSelected[0].incrementPropertyCode }}</strong
        >?
      </div>

      <!-- hiện xác nhận khi xóa 1  -->
      <div v-if="isShowDeleteMultiple">
        Có <strong>{{ incrementSelected.length }}</strong> chứng từ ghi tăng
        được chọn. Bạn có muốn xóa?
      </div>
    </ms-form-confirm>

    <!-- form xác nhận của form thêm mới -->
    <ms-form-confirm
      v-if="isShowConfirmInForm"
      @returnTrue="formConfirmReturnTrue"
      @returnFalse="formConfirmReturnFalse"
      @returnNoSave="formConfirmReturnNoSave"
      :commandName="commandNameForm"
    >
      {{ contentConfirmInForm }}
    </ms-form-confirm>

    <!-- form thông báo -->
    <ms-form-notice v-if="isShowFormNotice" />

    <!-- loading -->
    <div class="loader" v-if="isShowLoading"></div>

    <!-- contextmenu  -->
    <ms-context-menu
      :posTop="posTop"
      :posLeft="posLeft"
      v-if="isShowContextMenu"
      :init-item="contextMenuItems"
      @close="closeContextMenu"
      @clickAdd="onClickAddConTextMenu"
      @clickEdit="onClickEditContextMenu"
      @clickDelete="onClickDeleteContextMenu"
    />
  </div>
</template>

<script>
import MsTable from "../base/MsTable.vue";
import MsPaging from "../base/MsPaging.vue";
import MsCombobox from "../base/MsCombobox.vue";
import MsInputNumber from "../base/MsInputNumber.vue";
import MsPopup from "../base/MsPopup.vue";
import MsToolTip from "../base/MsToolTip.vue";
import MsFormNotice from "../base/MsFormNotice.vue";
import MsContextMenu from "../base/MsContextMenu.vue";
import MsFormConfirm from "../base/MsFormConfirm.vue";
import {
  ErrorMsg,
  CommandNameFormConfirm,
  CommandName,
  NoticeMsg,
} from "@/js/common/resource";
import { ErrorCode } from "@/js/common/enumeration";
import { ElDatePicker } from "element-plus";
import { DateConfig } from "../../js/common/config";
import { Splitpanes, Pane } from "splitpanes";
import Axios from "axios";
import {
  BASE_URL_Budget,
  BASE_URL_Increment_Property,
  BASE_URL_Property,
} from "@/js/common/base-url";
export default {
  components: {
    MsTable,
    MsPaging,
    MsCombobox,
    Splitpanes,
    ElDatePicker,
    Pane,
    MsPopup,
    MsInputNumber,
    MsToolTip,
    MsFormNotice,
    MsFormConfirm,
    MsContextMenu,
  },
  data() {
    return {
      isShowFormPopupIncrement: false, // hiển thị popup increment
      arrPageProperty: [
        { code: 1, name: 20 },
        { code: 2, name: 30 },
        { code: 3, name: 50 },
        { code: 3, name: 100 },
      ], // mảng phân trang cho bảng tài sản
      arrPage: [
        { code: 1, name: 10 },
        { code: 2, name: 20 },
        { code: 3, name: 30 },
      ], // mảng phân trang
      detailSize: 35, // kích thước màn detail
      masterSize: 65, // kích thước màn master
      isShowDetail: true, // hiển thị form detail
      inputDate: Date.now(), // ngày hiện tại
      dateConfig: DateConfig, // style chung cho date
      mode: "horizontal", // chế độ màn hình
      isShowSelectMode: false, // hiển thị chọn chế độ
      isShowFormSelectProperty: false, // hiển hị form chọn tài sản
      incrementProperty: {
        incrementPropertyID: "00000000-0000-0000-0000-000000000000",
        incrementPropertyCode: "",
        voucherDate: this.formatDate(""),
        incrementDate: this.formatDate(""),
        description: "",
        modifiedDate: "1000-01-01",
        modifiedBy: "Trung Germany",
        createdDate: "1000-01-01",
        createdBy: "Strung Germany",
        listProperty: [],
        budget: 0,
      }, // chứng từ ghi tăng
      selectPropertyPageNumber: 1, // pagenumber hiện tại trang select
      selectPropertyPageSize: 20, // pagesize trang select
      initPropertiesSelect: [], // tài sản trong trang select
      maxPagePropertiesSelect: 0, // maxpage trang select
      keywordFilterProperty: "", // từ khóa tìm kiếm tài sản
      columnsForTableProperty: [
        {
          name: "propertyCode",
          value: "Mã tài sản",
          type: "text",
          total: false,
        },
        {
          name: "propertyName",
          value: "Tên tài sản",
          type: "text",
          total: false,
        },
        {
          name: "propertyTypeName",
          value: "Loại tài sản",
          type: "text",
          total: false,
        },
        {
          name: "departmentName",
          value: "Tên bộ phận sử dụng",
          type: "text",
          total: false,
        },
        {
          name: "quantity",
          value: "Số lượng",
          type: "number",
          total: true,
        },
        {
          name: "markedPrice",
          value: "Nguyên giá",
          type: "number",
          total: true,
        },
        {
          name: "attritionValue",
          value: "KH/HM Lũy kế",
          type: "number",
          total: true,
        },
      ], // các cột trong bảng tài sản
      columnsForTableIncrementProperty: [
        {
          name: "incrementPropertyCode",
          value: "Mã chứng từ",
          type: "text",
          total: false,
        },
        {
          name: "incrementDate",
          value: "Ngày ghi tăng",
          type: "date",
          total: false,
        },
        {
          name: "voucherDate",
          value: "Ngày sử dụng",
          type: "date",
          total: false,
        },
        {
          name: "budget",
          value: "Nguồn hình thành",
          type: "number",
          total: true,
        },
        {
          name: "description",
          value: "Nội dung",
          type: "text",
          total: false,
        },
        {
          name: "control",
          value: "",
          type: "control",
          control: ["Edit", "Delete"],
          total: false,
        },
      ], // các cột trong bản ghi tăng
      columnsForTableDetail: [
        {
          name: "propertyCode",
          value: "Mã tài sản",
          type: "text",
          total: false,
        },
        {
          name: "propertyName",
          value: "Tên tài sản",
          type: "text",
          total: false,
        },
        {
          name: "propertyTypeName",
          value: "Loại tài sản",
          type: "text",
          total: false,
        },
        {
          name: "departmentName",
          value: "Tên bộ phận sử dụng",
          type: "text",
          total: false,
        },
        {
          name: "quantity",
          value: "Số lượng",
          type: "number",
          total: true,
        },
        {
          name: "markedPrice",
          value: "Nguyên giá",
          type: "number",
          total: true,
        },
        {
          name: "attritionValue",
          value: "KH/HM Lũy kế",
          type: "number",
          total: true,
        },
      ], // cột trong bảng chi tiết
      columnsForTablePopupAdd: [
        {
          name: "propertyCode",
          value: "Mã tài sản",
          type: "text",
          total: false,
        },
        {
          name: "propertyName",
          value: "Tên tài sản",
          type: "text",
          total: false,
        },
        {
          name: "propertyTypeName",
          value: "Loại tài sản",
          type: "text",
          total: false,
        },
        {
          name: "departmentName",
          value: "Tên bộ phận sử dụng",
          type: "text",
          total: false,
        },
        {
          name: "quantity",
          value: "Số lượng",
          type: "number",
          total: true,
        },
        {
          name: "markedPrice",
          value: "Nguyên giá",
          type: "number",
          total: true,
        },
        {
          name: "attritionValue",
          value: "KH/HM Lũy kế",
          type: "number",
          total: true,
        },
        {
          name: "control",
          value: "Chức năng",
          type: "control",
          control: ["Edit", "Delete"],
          total: false,
        },
      ], // cột trong bảng thêm mới
      incrementProperties: [], // danh sách các chứng từ ghi tăng
      totalIncrementProperties: 0, // tổng số chứng từ
      pageSize: 10, // pageSize bảng ghi tăng
      pageNumber: 1, // pagenumber bảng ghi tăng
      maxPage: 0, // bảng ghi tăng
      keyword: "", // từ khóa tìm kiếm chứng từ
      properties: [], // danh sách tài sản
      commandName: "", // tên của các lệnh
      isShowFormPopupEditProperty: false, // show form thêm
      budgets: [], // danh sách nguồn hình thành
      budgetsOfProperty: [], // nguồn hình thành của tài sản
      markedPrice: 0, // giá trị của tài sản
      indexOfPropertyEditing: 0, // index của tài sản đang sửa
      isShowFormNotice: false, // show form thông báo
      isShowFormConfirm: false, // show form xác nhận
      contentFormConfirm: "", // nội dung form thông báo
      incrementSelected: [], // chứng từ được chọn
      isShowConfirmInForm: false, // show form xác nhận trong form thêm mới
      isShowLoading: false, // show loading
      error: {
        incrementPropertyCode: "",
        voucherDate: "",
        incrementDate: "",
        listProperty: "",
      }, // lưu lại lỗi
      rules: {
        incrementPropertyCode: {
          Required: true,
          MaxLength: 20,
        },
        voucherDate: { Required: true },
        incrementDate: { Required: true },
      }, // lưu lại các rule validate
      isShowContextMenu: false, // show context menu
      contextMenuItems: [
        { icon: "icon-add", text: "Thêm chứng từ", method: "clickAdd" },
        { icon: "icon-Edit", text: "Sửa chứng từ", method: "clickEdit" },
        {
          icon: "icon-delete-black",
          text: "Xóa chứng từ",
          method: "clickDelete",
        },
      ], // các item trong context menu
      departmentInFormEdit: "", // tên phòng ban
      keywordSearchProperty: "", // từ khóa tìm kiếm tài sản
      listPropertyShow: [], // danh sách tài sản được hiển thị
      indexBudgetError: [],
    };
  },
  methods: {
    /**
     * tìm kiếm tài sản ở form thêm mới chứng từ
     * Author: TTDuc(31/10/2022)
     */
    searchProperty() {
      try {
        console.log(this.incrementProperty.listProperty);
        if (!this.incrementProperty.listProperty) return;

        this.listPropertyShow = this.incrementProperty.listProperty.filter(
          (property) => {
            let propertyName = property.propertyName.toLowerCase();
            let propertyCode = property.propertyCode.toLowerCase();
            if (
              propertyName.includes(this.keywordSearchProperty.toLowerCase()) ||
              propertyCode.includes(this.keywordSearchProperty.toLowerCase())
            ) {
              return property;
            }
            return;
          }
        );
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý add ở context menu
     * Author: TTDuc(26/10/2022)
     */
    onClickAddConTextMenu() {
      this.onClickAdd();
    },
    /**
     * hàm xử lý edit context menu
     * Author: TTDuc(26/10/2022)
     */
    onClickEditContextMenu() {
      try {
        this.onEditIncrementProperty(this.incrementProperty);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý xóa context menu
     * Author: TTDuc(26/10/2022)
     */
    onClickDeleteContextMenu() {
      try {
        this.onDeleteIncrementProperty();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý đóng context menu
     * Author: TTDuc(26/10/2022)
     */
    closeContextMenu() {
      try {
        this.toggleContextMenu();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý mở context menu
     * Author: TTDuc(26/10/2022)
     */
    onContextMenu() {
      try {
        this.toggleContextMenu();
        this.posTop = event.clientY;
        this.posLeft = event.clientX;
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm xử lý đóng mở context menu
     * Author: TTDuc(26/10/2022)
     */
    toggleContextMenu() {
      this.isShowContextMenu = !this.isShowContextMenu;
    },
    /**
     * hàm xử lý validate tổng
     * Author: TTDuc(23/10/2022)
     */
    validateAll() {
      let isValidAll = true; // biến check lỗi tổng
      for (const propName in this.rules) {
        let isValid = true; // biến check lỗi khi duyệt qua 1 trường dữ liệu

        for (const key in this.rules[propName]) {
          if (isValid) {
            let functionName = `validate${key}`;

            // kiểm tra nếu còn đúng thì validate tiếp
            if (isValid == true) {
              let valueValidate = this.rules[propName][key];
              isValid = this[functionName](valueValidate, propName);
            }
          }
        }
        if (isValidAll) {
          isValidAll = isValid;
        }
      }

      // thực hiện focus vào lỗi
      if (!isValidAll) {
        this.focusToError();
      }

      if (isValidAll) {
        isValidAll = this.validateMajor();
      }

      return isValidAll;
    },
    /**
     * hàm xử lý validate nghiệp vụ
     * Author: TTDuc(23/10/2022)
     */
    validateMajor() {
      if (this.incrementProperty.listProperty.length == 0) {
        this.error.listProperty = ErrorMsg.ListPropertyNotEmpty;
        this.commandNameForm = CommandNameFormConfirm.Notice;
        this.contentConfirmInForm = this.error.listProperty;
        this.isShowConfirmInForm = true;
        return false;
      } else {
        this.error.listProperty = ErrorMsg.Empty;
        return true;
      }
    },
    /**
     * hàm xử lý validate qua các trường
     * Author: TTDuc(23/10/2022)
     */
    validate(propName) {
      // sau 0.2s thì validate để cập nhật dữ liệu trước khi validate
      setTimeout(() => {
        let isValid = true; // biến lưu giá trị validate sau mỗi vòng for
        for (const key in this.rules[propName]) {
          let functionName = `validate${key}`;
          if (isValid == true) {
            let valueValidate = this.rules[propName][key];
            isValid = this[functionName](valueValidate, propName);
          }
        }
      }, 200);
    },
    /**
     * hàm xử lý trường bắt buộc nhập
     * Author: TTDuc(23/10/2022)
     */
    validateRequired(value, propName) {
      // kiểm tra rỗng thì lưu lại lỗi và trả về false
      if (
        this.incrementProperty[propName] == null ||
        this.incrementProperty[propName] == ""
      ) {
        this.error[propName] = ErrorMsg.Required;
        return false;
      }
      this.error[propName] = ErrorMsg.Empty;
      return true;
    },
    /**
     * hàm xử lý độ dài lớn nhất
     * Author: TTDuc(23/10/2022)
     */
    validateMaxLength(value, propName) {
      if (this.incrementProperty[propName].length > value) {
        this.error[propName] = ` ${ErrorMsg.MaxLength} ${value} ký tự!`;
        return false;
      }
      this.error[propName] = ErrorMsg.Empty;
      return true;
    },
    /**
     * hàm xử lý focus vào lỗi
     */
    focusToError() {
      try {
        let i = 1;
        for (const key in this.error) {
          // kiểm tra xem trường nào có lỗi thì focus vào trường đó và break ngay
          if (this.error[key] !== "") {
            this.currentTabindex = i;
            this.setFocus(this.currentTabindex);
            break;
          }
          i++;
        }
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * show MFormNotice
     * Author: TTDuc(19/10/2022)
     */
    showFormNotice() {
      setTimeout(() => {
        this.isShowFormNotice = false;
      }, 2000);
      this.isShowFormNotice = true;
    },
    /**
     * hàm xóa chứng từ ghi tăng
     * Author: TTDuc(21/10/2022)
     */
    onClickDelete() {
      try {
        this.incrementSelected =
          this.$refs.tableIncrementProperty.selectedItems;

        if (this.incrementSelected.length == 0) {
          this.commandName = CommandName.Notice;
          this.contentFormConfirm = ErrorMsg.NotChooseIncrementProperty;
          this.isShowFormConfirm = true;
        } else if (this.incrementSelected.length == 1) {
          this.onDeleteIncrementProperty();
        } else if (this.incrementSelected.length > 1) {
          this.commandName = CommandName.Delete;
          this.isShowDeleteMultiple = true;
          this.isShowFormConfirm = true;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xóa 1 chứng từ ghi tăng
     * Author: TTDuc(21/10/2022)
     */
    onDeleteIncrementProperty() {
      try {
        this.incrementSelected =
          this.$refs.tableIncrementProperty.selectedItems;
        this.commandName = CommandName.Delete;
        this.isShowDelete1 = true;
        this.isShowFormConfirm = true;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý đóng popup thêm mới
     * Author: TTDuc(21/10/2022)
     */
    closePopupIncrementProperty() {
      try {
        if (!this.edited) {
          this.isShowFormPopupIncrement = false;
        } else {
          this.commandNameForm = CommandNameFormConfirm.Save;
          this.contentConfirmInForm = NoticeMsg.IncrementPropertyEdited;
          this.isShowConfirmInForm = true;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý form confirm trả về đúng
     * Author: TTDuc(21/10/2022)
     */
    formConfirmReturnTrue() {
      if (this.commandName == CommandName.Notice) {
        this.isShowFormConfirm = false;
      } else if (this.commandName == CommandName.Delete) {
        this.deleteMultipleIncrementProperty();
        this.isShowDelete1 = false;
        this.isShowDeleteMultiple = false;
      }

      if (this.commandNameForm == CommandNameFormConfirm.Close) {
        this.isShowFormPopupIncrement = false;
      } else if (this.commandNameForm == CommandNameFormConfirm.Save) {
        this.onClickSave();
      }

      this.isShowConfirmInForm = false;
      this.isShowFormConfirm = false;
    },
    /**
     * hàm xử lý khi formconfirm trả về không
     * Author: TTDuc(21/10/2022)
     */
    formConfirmReturnFalse() {
      this.isShowFormConfirm = false;
      this.isShowDelete1 = false;
      this.isShowConfirmInForm = false;
      this.isShowDeleteMultiple = false;
    },
    /**
     * hàm xử lý khi formconfirm trả về không lưu
     * Author: TTDuc(21/10/2022)
     */
    formConfirmReturnNoSave() {
      this.isShowConfirmInForm = false;
      this.isShowFormPopupIncrement = false;
    },
    /**
     * lưu tạm thời tài sản đã sửa trong form ghi tăng
     * Author: TTDuc(16/10/2022)
     */
    onClickSaveProperty() {
      try {
        if (this.validateProperty()) {
          this.incrementProperty.listProperty[
            this.indexOfPropertyEditing
          ].markedPrice = this.markedPrice;
          this.incrementProperty.listProperty[
            this.indexOfPropertyEditing
          ].budget = JSON.stringify(this.budgetsOfProperty);
          this.isShowFormPopupEditProperty = false;
          this.budgetsOfProperty = [];
          this.markedPrice = 0;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * validate tài sản được ghi tăng
     * Author: TTDuc(21/10/2022)
     */
    validateProperty() {
      let isValid = this.validateRequiredProperty();

      if (isValid) {
        isValid = this.validateDuplicateProperty();
      }

      return isValid;
    },
    /**
     * validate trường bắt buộc tài sản được ghi tăng
     * Author: TTDuc(21/10/2022)
     */
    validateRequiredProperty() {
      let isValid = true;
      for (let i = 0; i < this.budgetsOfProperty.length; i++) {
        let budget = this.budgetsOfProperty[i];
        if (budget.budgetName == "" || budget.budgetPrice <= 0) {
          if (isValid) isValid = false;
          this.addIndexBudgetError(i);
        } else {
          this.removeIndexBudgetError(i);
        }
      }

      if (!isValid) {
        this.contentConfirmInForm = NoticeMsg.DataRequired;
        this.commandNameForm = CommandNameFormConfirm.Notice;
        this.isShowConfirmInForm = true;
      }

      return isValid;
    },
    /**
     * validate tài sản có nguồn hình thành trùng nhau
     * Author: TTDuc(21/10/2022)
     */
    validateDuplicateProperty() {
      let isValid = true;
      for (let i = 0; i < this.budgetsOfProperty.length - 1; i++) {
        for (let j = i + 1; j < this.budgetsOfProperty.length; j++) {
          if (
            this.budgetsOfProperty[i].budgetName ==
            this.budgetsOfProperty[j].budgetName
          ) {
            isValid = false;
            this.addIndexBudgetError(i);
            this.addIndexBudgetError(j);
          } else {
            this.removeIndexBudgetError(i);
            this.removeIndexBudgetError(j);
          }
        }
      }

      if (!isValid) {
        this.contentConfirmInForm = NoticeMsg.DuplicateBudget;
        this.commandNameForm = CommandNameFormConfirm.Notice;
        this.isShowConfirmInForm = true;
      }

      return isValid;
    },
    addIndexBudgetError(index) {
      if (this.indexBudgetError.includes(index)) return;
      this.indexBudgetError.push(index);
    },
    removeIndexBudgetError(index) {
      try {
        if (this.indexBudgetError.includes(index)) {
          this.indexBudgetError.splice(this.indexBudgetError.indexOf(index), 1);
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm thêm 1 budget
     * Author: TTDuc(16/10/2022)
     */
    addBudget() {
      try {
        this.budgetsOfProperty.push({ budgetName: "", budgetPrice: 0 });
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm thêm 1 budget
     * Author: TTDuc(16/10/2022)
     */
    removeBudget(item) {
      try {
        if (this.budgetsOfProperty.length > 1) {
          this.budgetsOfProperty.splice(
            this.budgetsOfProperty.indexOf(item),
            1
          );
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm sửa tài sản
     * Author: TTDuc(15/10/2022)
     */
    editProperty(item) {
      try {
        this.indexOfPropertyEditing =
          this.incrementProperty.listProperty.indexOf(item);
        this.departmentInFormEdit = item.departmentName;
        this.isShowFormPopupEditProperty = true;
        this.budgetsOfProperty = JSON.parse(item.budget);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xóa tài sản
     * Author: TTDuc(15/10/2022)
     */
    deleteProperty(item) {
      try {
        this.incrementProperty.listProperty =
          this.incrementProperty.listProperty.filter((property) => {
            if (property == item) return;
            return property;
          });
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý lưu tài sản
     * Author: TTDuc(15/10/2022)
     */
    onClickSave() {
      try {
        if (this.validateAll()) {
          let totalBudget = 0;
          for (const property of this.incrementProperty.listProperty) {
            if (property.markedPrice) {
              totalBudget += property.markedPrice;
            }
          }
          this.incrementProperty.budget = totalBudget;

          if (this.commandName == CommandName.Add) {
            this.addIncrementProperty();
          } else if (this.commandName == CommandName.Edit) {
            this.editIncrementProperty();
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm mở form sửa tài sản
     * Author: TTDuc(15/10/2022)
     */
    onEditIncrementProperty(item) {
      try {
        this.commandName = CommandName.Edit;
        this.incrementProperty = Object.assign({}, item);
        this.isShowFormPopupIncrement = true;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm chuyển sang trang sau
     * Author: TTDuc(15/10/2022)
     */
    nextPage() {
      try {
        if (this.pageNumber < this.maxPage) {
          this.pageNumber++;
          this.getIncrementProperty();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm quay lại trang trước
     * Author: TTDuc(15/10/2022)
     */
    prevPage() {
      try {
        if (this.pageNumber > 1) {
          this.pageNumber--;
          this.getIncrementProperty();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm sử lý sự kiện thay đổi pagenumber
     * Author: TTDuc(15/10/2022)
     */
    changePageNumber(selectedPage) {
      try {
        this.pageNumber = selectedPage;
        this.getIncrementProperty();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý sự kiện chọn pagesize
     * Author: TTDuc(15/10/2022)
     */
    selectedPageSize(item) {
      try {
        this.pageSize = parseInt(item.name);
        this.getIncrementProperty();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý sự kiện chọn chứng từ ghi tăng
     * Author: TTDuc(15/10/2022)
     */
    selectItem(incrementProperty) {
      try {
        this.incrementProperty = Object.assign({}, incrementProperty);
        this.getIncrementPropertyByID(incrementProperty.incrementPropertyID);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm mở form chọn tài sản
     * Author: TTDuc(15/10/2022)
     */
    onClickSelectProperty() {
      try {
        this.isShowFormSelectProperty = true;
        this.getProperties();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý sự kiện nhấn nút thêm mới
     * Author: TTDuc(15/10/2022)
     */
    onClickAdd() {
      try {
        this.commandName = CommandName.Add;
        this.resetIncrementProperty();
        this.getNewIncrementPropertyCode();
        this.isShowFormPopupIncrement = !this.isShowFormPopupIncrement;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * thực hiện focus vào input có tabindex đã cho
     * Author: TTDuc(21/10/2022)
     */
    setFocus(tabindex) {
      if (this.$el.querySelector(`input[tabindex='${tabindex}']`)) {
        this.$el.querySelector(`input[tabindex='${tabindex}']`).focus();
      }
    },
    /**
     * Phóng to , thu nhỏ màn detail
     * Author: TTDuc (11/10/2022)
     */
    zoomOutDetail() {
      try {
        this.detailSize = this.detailSize != 100 ? 100 : 35;
        this.masterSize = 100 - this.detailSize;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * chế độ màn hình ngang
     * Author: TTDuc (11/10/2022)
     */
    selectModeHorizontal() {
      try {
        this.mode = "horizontal";
        this.isShowDetail = true;
        this.masterSize = 65;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * chế độ màn hình dọc
     * Author: TTDuc (11/10/2022)
     */
    selectModeVertical() {
      try {
        this.mode = "vertical";
        this.isShowDetail = false;
        this.masterSize = 100;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm nhận sự kiện chuyển trang
     * Author: TTDuc(15/10/2022)
     */
    nextPropertyPage() {
      try {
        if (this.selectPropertyPageNumber < this.maxPagePropertiesSelect) {
          this.selectPropertyPageNumber++;
          this.getProperties();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm nhận sự kiện quay lại trang trước
     * Author: TTDuc(15/10/2022)
     */
    prevPropertyPage() {
      try {
        if (this.selectPropertyPageNumber > 1) {
          this.selectPropertyPageNumber--;
          this.getProperties();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm nhận sự kiện thay đôi trang
     * Author: TTDuc(15/10/2022)
     */
    changePropertyPageNumber(selectedPage) {
      try {
        this.selectPropertyPageNumber = selectedPage;
        this.getProperties();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý khi chọn pagesize
     * Author: TTDuc(15/10/2022)
     */
    selectedPropertyPageSize(item) {
      try {
        this.selectPropertyPageSize = parseInt(item.name);
        this.getProperties();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm focus vào form search
     * Author: TTDuc(15/10/2022)
     */
    onFocusFormSearch() {
      this.$refs.formSearchProperty.select();
    },
    /**
     * hàm click sác nhận chọn form popup
     * Author: TTDuc(15/10/2022)
     */
    onClickConfirmSelect() {
      try {
        for (const item of this.$refs.tableSelectProperty.selectedItems) {
          let isPush = true;

          for (let j = 0; j < this.incrementProperty.listProperty.length; j++) {
            if (
              this.incrementProperty.listProperty[j].propertyID ==
              item.propertyID
            ) {
              isPush = false;
            }
          }

          if (isPush) {
            this.incrementProperty.listProperty.push(item);
          }
        }
        this.isShowFormSelectProperty = false;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý format ngày tháng
     * Author: TTDuc(15/10/2022)
     */
    formatDate(date) {
      // nếu có định dạng date + time thì chỉ lấy date
      let today;
      if (date != "") {
        // return date;
        today = new Date(date);
      } else {
        today = new Date();
      }

      // nếu date truyền vào rỗng thì tự động lấy ngày hiện tại
      let year = today.getFullYear();
      let month = today.getMonth() + 1;
      let day = today.getDate();
      if (month < 10) {
        month = `0${month}`;
      }

      if (day < 10) {
        day = `0${day}`;
      }

      return `${year}-${month}-${day}`;
    },
    /**
     * hàm tạo giá trị ban đầu cho chứng từ ghi tăng
     * Author: TTDuc(15/10/2022)
     */
    resetIncrementProperty() {
      this.incrementProperty = {
        incrementPropertyID: "00000000-0000-0000-0000-000000000000",
        incrementPropertyCode: "",
        voucherDate: this.formatDate(""),
        incrementDate: this.formatDate(""),
        description: "",
        modifiedDate: "1000-01-01",
        modifiedBy: "Trung Germany",
        createdDate: "1000-01-01",
        createdBy: "Strung Germany",
        listProperty: [],
        budget: 0,
      };
    },

    /************************************* API ************************************** */
    /**
     * xóa nhiều chứng từu ghi tăng
     * Author: TTDuc(21/10/2022)
     */
    deleteMultipleIncrementProperty() {
      this.isShowLoading = true;
      let listId = [];
      for (const incrementProperty of this.incrementSelected) {
        listId.push(incrementProperty.incrementPropertyID);
      }

      Axios({
        url: `${BASE_URL_Increment_Property}/delete-multiple`,
        method: "Delete",
        data: listId,
      })
        .then(() => {
          this.getIncrementProperty();
          this.showFormNotice();
          this.$refs.tableIncrementProperty.selectedItems = [];
        })
        .catch((err) => {
          this.isShowLoading = false;
          console.log(err.response);
        });
    },
    /**
     * gọi API xóa tài sản
     * Author: TTDuc(15/10/2022)
     */
    deleteIncrementProperty(item) {
      Axios({
        url: `${BASE_URL_Increment_Property}/${item.incrementPropertyID}`,
        method: "Delete",
      })
        .then(() => {
          this.getIncrementProperty();
          this.showFormNotice();
        })
        .catch((err) => {
          console.log(err.response);
        });
    },
    /**
     * gọi API lấy tài sản qua ID
     * author : TTDuc(15/10/2022)
     */
    getIncrementPropertyByID(id) {
      Axios({
        url: `${BASE_URL_Increment_Property}/${id}`,
        method: "Get",
      })
        .then((response) => {
          this.incrementProperty.listProperty = response.data;
          this.properties = response.data;
        })
        .catch((err) => {
          console.log(err.response);
        });
    },
    /**
     * gọi API lấy danh sách tài sản
     * Author: TTDuc(15/10/2022)
     */
    getIncrementProperty() {
      this.isShowLoading = true;
      Axios({
        url: `${BASE_URL_Increment_Property}/paging-data?keyword=${this.keyword}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`,
        method: "Get",
      })
        .then((response) => {
          this.incrementProperties = response.data.data;
          this.totalIncrementProperties = response.data.totalCount;
          // lấy maxpage
          this.maxPage = Math.ceil(
            this.totalIncrementProperties / this.pageSize
          );
          // nếu ở trang lớn hơn trang maxpage thì trở về trang lớn nhất
          if (this.pageNumber > this.maxPage) {
            this.pageNumber = this.maxPage;
            this.getIncrementProperty();
          }
          this.isShowLoading = false;
        })
        .catch((err) => {
          this.isShowLoading = false;
          console.log(err);
        });
    },
    /**
     * gọi API thêm mới tài sản
     */
    addIncrementProperty() {
      this.isShowLoading = true;
      Axios({
        url: `${BASE_URL_Increment_Property}`,
        method: "Post",
        data: this.incrementProperty,
      })
        .then((response) => {
          this.isShowLoading = false;
          console.log(response);
          this.isShowFormPopupIncrement = false;
          this.resetIncrementProperty();
          this.getIncrementProperty();
          this.showFormNotice();
        })
        .catch((err) => {
          this.isShowLoading = false;
          console.log(err.response);
          if (err.response.data.errorCode == ErrorCode.DuplicateCode) {
            this.error.incrementPropertyCode = ErrorMsg.Duplicate;
            this.setFocus(1);
          }
        });
    },
    /**
     * gọi API sửa tài sản
     * Author: TTDuc(15/10/2022)
     */
    editIncrementProperty() {
      this.isShowLoading = true;
      Axios({
        url: `${BASE_URL_Increment_Property}/${this.incrementProperty.incrementPropertyID}`,
        method: "Put",
        data: this.incrementProperty,
      })
        .then((response) => {
          this.isShowLoading = false;
          console.log(response);
          this.isShowFormPopupIncrement = false;
          this.resetIncrementProperty();
          this.getIncrementProperty();
          this.showFormNotice();
        })
        .catch((err) => {
          this.isShowLoading = false;
          console.log(err.response);
          if (err.response.data.errorCode == ErrorCode.DuplicateCode) {
            this.error.incrementPropertyCode = ErrorMsg.Duplicate;
            this.setFocus(1);
          }
        });
    },

    /**
     * lấy mã Ghi tăng mới
     * Author: TTDuc (12/10/2022)
     */
    getNewIncrementPropertyCode() {
      Axios({
        url: `${BASE_URL_Increment_Property}/new-code`,
        method: "Get",
      })
        .then((response) => {
          this.incrementProperty.incrementPropertyCode = response.data;
        })
        .catch((err) => {
          console.log(err.response);
        });
    },
    getBudgets() {
      Axios({
        url: `${BASE_URL_Budget}`,
        method: "Get",
      })
        .then((response) => {
          this.budgets = response.data;
        })
        .catch((err) => {
          console.log(err.response);
        });
    },
    /**
     * gọi API lấy danh sách tài sản
     * Author: TTDuc(15/10/2022)
     */
    getProperties() {
      //lấy danh sách ID
      let listIDSelected = [];
      for (const property of this.incrementProperty.listProperty) {
        listIDSelected.push(property.propertyID);
      }

      Axios({
        url: `${BASE_URL_Property}/paging-data?keyword=${this.keywordFilterProperty}&status=0&pageNumber=${this.selectPropertyPageNumber}&pageSize=${this.selectPropertyPageSize}`,
        method: "post",
        data: listIDSelected,
      })
        .then((response) => {
          this.initPropertiesSelect = response.data.data;
          // lấy maxPage
          this.maxPagePropertiesSelect = Math.ceil(
            response.data.totalCount / this.selectPropertyPageSize
          );
          // nếu đang ở trang lớn hơn maxpage thì gọi về trang maxpage
          if (this.selectPropertyPageNumber > this.maxPagePropertiesSelect) {
            this.selectPropertyPageNumber = this.maxPagePropertiesSelect;
            this.getProperties();
          }
          this.totalProperty = response.data.totalCount;
        })
        .catch((err) => {
          console.log(err.response);
        });
    },
  },
  created() {
    this.getIncrementProperty();
    this.getBudgets();
  },
  watch: {
    /**
     * lọc khi mở form thêm mới
     * Author: TTDuc(19/10/2022)
     */
    isShowFormPopupIncrement() {
      if (this.isShowFormPopupIncrement) {
        setTimeout(() => {
          this.setFocus(1);
          this.edited = false;
          this.searchProperty();
        }, 1000);
      } else {
        this.keywordSearchProperty = "";
      }
    },

    /**
     * lấy lại danh sách tài sản khi keyword rỗng
     * Author: TTDuc(19/10/2022)
     */
    keywordFilterProperty() {
      if (this.keywordFilterProperty == "") {
        this.getProperties();
      }
    },

    /**
     * thực hiện lấy lại danh sách chứng từ ghi tăng khi keyword rỗng
     * Author: TTDuc(19/10/2022)
     */
    keyword() {
      if (this.keyword == "") {
        this.getIncrementProperty();
      }
    },

    /**
     * thực hiện tĩnh tổng khi danh sách nguồn hình thành thay đổi
     * Author: TTDuc(19/10/2022)
     */
    budgetsOfProperty: {
      handler() {
        this.markedPrice = 0;

        for (const budget of this.budgetsOfProperty) {
          this.markedPrice += budget.budgetPrice;
        }
      },
      deep: true,
    },

    /**
     * thực hiện theo dõi khi mở form
     * Author: TTDuc(24/10/2022)
     */
    incrementProperty: {
      handler() {
        this.edited = true;
      },
      deep: true,
    },

    /**
     * thực hiện tìm kiếm khi danh sách được chọn thay đổi
     * Author: TTDuc(24/10/2022)
     */
    "incrementProperty.listProperty": {
      handler() {
        if (this.isShowFormPopupIncrement) {
          this.searchProperty();
        }
      },
      deep: true,
    },

    /**
     * thực hiện tìm kiếm khi keywwork rỗng
     * Author: TTDuc(24/10/2022)
     */
    keywordSearchProperty() {
      if (this.keywordSearchProperty == "") {
        this.searchProperty();
      }
    },

    /**
     * thực hiện reset giá trị khi đóng form select
     * Author: TTDuc(24/10/2022)
     */
    isShowFormSelectProperty() {
      if (!this.isShowFormSelectProperty) {
        this.keywordFilterProperty = "";
        this.selectPropertyPageNumber = 1;
      }
    },
  },
};
</script>

<style scoped>
@import url("@/css/page/increment-page.css");
</style>
