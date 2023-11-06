<template>
  <div id="categoryPage" class="main" @keydown.up="onKeyUp">
    <div class="main-controler">
      <div class="main-controler-left">
        <div class="form-input">
          <div class="icon">
            <div class="icon-search-black"></div>
          </div>
          <input
            class="input input-icon"
            ref="formSearch"
            placeholder="Tìm kiếm loại tài sản"
            v-model="keyword"
            @keypress.enter="getPagingProperties"
            @focus="onFocusFormSearch"
          />
        </div>
      </div>
      <div class="main-controler-right">
        <button @click="onClickAdd" class="button main-button">
          Thêm loại tài sản
        </button>
      </div>
    </div>

    <div id="table">
      <div class="table">
        <table
          ref="tableFocus"
          tabindex="0"
          @keyup.up="onKeyUpTable"
          @keyup.down="onKeyDownTable"
          @keypress.enter.exact="onKeyEnterTable"
          @blur="onBlurTable"
        >
          <thead>
            <tr>
              <!-- <th class="text-center width-fit">
                <input
                  type="checkbox"
                  @click="selectedAllItem"
                  :checked="selectedProperties.length == properties.length"
                />
              </th> -->
              <th class="text-center" data-tip="Số thứ tự">STT</th>
              <th>Mã loại tài sản</th>
              <th>Tên loại tài sản</th>
              <th>Tỷ lệ hao mòn</th>
              <th>Số năm sử dụng</th>
              <th>Ghi chú</th>
              <th class="text-center">Chức năng</th>
            </tr>
          </thead>

          <tbody>
            <tr
              v-for="(property, index) in properties"
              :key="property"
              :class="
                selectedProperties.includes(property) || index === indexFocus
                  ? 'active'
                  : ''
              "
              @contextmenu.prevent="onClickContextMenu(property)"
              @mousedown.prevent.ctrl="mousedown(property)"
              @mouseup.prevent.ctrl="mouseup(property)"
              @click.prevent.exact="selectItem(property)"
              @click.prevent.shift="
                selectMultipleItem(currentProperty, property)
              "
              @dblclick="onClickEdit(property)"
            >
              <!-- <td class="text-center">
                <input
                  type="checkbox"
                  :checked="
                    selectedProperties.includes(property) ? true : false
                  "
                  @click.stop="selectItemToList(property)"
                />
              </td> -->
              <td class="text-center">{{ index + 1 }}</td>
              <td>{{ property.propertyTypeCode }}</td>
              <td>{{ property.propertyTypeName }}</td>
              <td>{{ property.attritionRateDefault }}</td>
              <td>{{ property.usedYearDefault }}</td>
              <td>{{ property.createdBy }}</td>
              <td class="text-center">
                <div class="trow-control">
                  <ms-tool-tip content="Sửa" position="left">
                    <div class="icon" @click.stop="onClickEdit(property)">
                      <div class="icon-Edit"></div>
                    </div>
                  </ms-tool-tip>
                  <ms-tool-tip content="Xóa" position="left">
                    <div class="icon">
                      <div
                        class="icon-Delete"
                        @click="onClickDelete1(property)"
                      ></div>
                    </div>
                  </ms-tool-tip>
                </div>
              </td>
            </tr>
            <tr style="height: 0; border: none"></tr>
          </tbody>
        </table>
      </div>
    </div>
    <!-- contextmenu  -->
    <ms-context-menu
      :posTop="posTop"
      :posLeft="posLeft"
      v-if="isShowContextMenu"
      :initItem="contextMenuItems"
      @close="closeContextMenu"
      @clickAdd="onClickAddContextMenu"
      @clickEdit="onClickEditContextMenu"
      @clickDelete="onClickDeleteContextMenu"
      @clickDuplicate="onClickDuplicateContextMenu"
    />
    <!-- FormDetail -->
    <category-detail
      v-if="isShowDetail"
      :item="commandName != 'add' ? currentProperty : {}"
      :commandName="commandName"
      @success="formDetailSuccess"
      @closeForm="toggleFormDetail"
    />

    <!-- formConfirm -->
    <ms-form-confirm
      v-if="isShowFormConfirm"
      :commandName="commandName"
      @returnTrue="formConfirmReturnTrue"
      @returnFalse="formConfirmReturnFalse"
    >
      <!-- cảnh báo khi chưa chọn item -->
      <div v-if="isShowDeleteWarning">
        {{ msgShowDeleteWarning }}
      </div>
      <!-- delete 1 item -->
      <div v-if="isShowDelete1Item">
        Bạn có muốn xóa tài sản
        <strong
          >{{ currentProperty.propertyTypeCode }} -
          {{ currentProperty.propertyTypeName }}</strong
        >?
      </div>
      <!-- delete nhiều item -->
      <div v-if="isShowDeleteManyItem">
        <strong>{{ selectedProperties.length }} </strong> tài sản đã được chọn.
        Bạn có muốn xóa?
      </div>

      <!-- thông báo -->
      <div
        v-if="commandName === 'notice' && titleFormConfirm != '' ? true : false"
      >
        {{ titleFormConfirm }}
      </div>

      <!-- Thông báo phát sinh chứng từ khi xóa 1 -->
      <div
        v-if="
          commandName === 'notice' && noticeName == 'increase1' ? true : false
        "
      >
        Tài sản <strong>{{ currentProperty.propertyCode }}</strong> đã phát sinh
        chứng từ ghi tăng. Không thể thực hiện xóa!
      </div>

      <!-- Thông báo phát sinh chứng từ khi xóa nhiều -->
      <div
        v-if="
          commandName === 'notice' && noticeName == 'increase' ? true : false
        "
      >
        Có <strong>{{ countIncrement() }}</strong> tài sản đã phát sinh chứng từ
        ghi tăng. Không thể thực hiện xóa!
      </div>

      <!-- xác nhận import -->
      <div v-if="commandName === 'import' ? true : false">
        Bạn có muốn nhập khẩu tệp <strong>{{ file.name }}</strong
        >?
      </div>
    </ms-form-confirm>

    <!-- formnotice  -->
    <ms-form-notice v-if="isShowFormNotice" />
  </div>
  <div class="loader" v-if="isShowLoading"></div>
</template>

<script>
import MsCombobox from "../base/MsCombobox.vue";
import CategoryDetail from "../base/CategoryDetail.vue";
import MsPaging from "../base/MsPaging.vue";
import MsContextMenu from "../base/MsContextMenu.vue";
import MsFormConfirm from "../base/MsFormConfirm.vue";
import MsFormNotice from "../base/MsFormNotice.vue";
import axios from "axios";
import { PropertyStatus } from "../../js/common/enumeration";
import {
  CommandName,
  Methods,
  TypeOfExcel,
  NoticeName,
  ErrorMsg,
} from "../../js/common/resource";
import {
  BASE_URL_Department,
  BASE_URL_Property,
  BASE_URL_Property_Type,
} from "../../js/common/base-url";
import MsToolTip from "../base/MsToolTip.vue";

export default {
  components: {
    MsCombobox,
    CategoryDetail,
    MsPaging,
    MsContextMenu,
    MsFormConfirm,
    MsFormNotice,
    MsToolTip,
  },
  data() {
    return {    
      posTop: 100000, //vị trí contextMenu
      posLeft: 100000, //vị trí contextMenu
      isShowContextMenu: false, // show contextMenu
      isShowDetail: false, // show form FormDetail
      isShowLoading: false, // hiển thị loading
      pageNumber: 1, // số trang
      pageSize: 20, // số Tài sản trên 1 trang
      maxPage: 15, // tổng số trang
      propertyTypeID: "", // ID loại tài sản được lọc
      departmentID: "", // ID Phòng ban được lọc
      keyword: "", // từ khóa được lọc
      totalProperty: 0, // Tổng số tài sản được lọc theo keyword
      selectedProperties: [], // các Tài sản đã chọn được chọn
      currentProperty: {}, // tài sản vừa được chọn
      commandName: "", // tên lệnh Vd: get, delete, edit ,...
      isShowFormConfirm: false, //show formConfirm
      isShowFormNotice: false, // show formnotice
      isShowDelete1Item: false, // show Delete1Item
      isShowDeleteManyItem: false, // hiển thị thông báo khi xóa nhiều item cùng lúc
      isShowDeleteWarning: false, // cảnh báo khi chưa chọn item nào mà nhấn nút xóa
      msgShowDeleteWarning: "Chưa có tài sản nào được chọn. Vui lòng chọn tài ản trước khi xóa!",
      titleFormConfirm: "", // đơn thông báo trong form confirm
      file: null, // lưu lại file muốn import
      properties: [], // Danh sách tài sản được hiển thị
      arrPage: [
        { code: 1, name: 20 },
        { code: 2, name: 30 },
        { code: 3, name: 50 },
        { code: 4, name: 100 },
      ], // danh sách số tài sản được hiển thị trong 1 trang
      propertyTypes: [], // danh sách loại tài sản
      departments: [], // danh sách Tên phòng ban
      propertyOnMouseDown: {},
      propertyOnMouseUp: {}, // danh s
      indexFocus: 0, // vị trí đang focus trong table
      noticeName: "",
      contextMenuItems: [
        { icon: "icon-add", text: "Thêm tài sản", method: "clickAdd" },
        { icon: "icon-Edit", text: "Sửa tài sản", method: "clickEdit" },
        {
          icon: "icon-delete-black",
          text: "Xóa tài sản",
          method: "clickDelete",
        },
        {
          icon: "icon-Duplicate",
          text: "Nhân bản tài sản",
          method: "clickDuplicate",
        },
      ],
    };
  },
  created() {
    // lấy danh sách property, department, propertyType
    this.getPagingProperties();
    this.getDepartments();
    this.getPropertyTypes();
  },
  methods: {
    /**
     * hàm thực hiện khi nhấn nút lên table
     * Author:TTDuc(11/09/2022)
     */
    onKeyUpTable() {
      if (this.indexFocus > 0) {
        this.indexFocus--;
      }
    },
    /**
     * hàm thực hiện khi nhấn nút xuống table
     * Author:TTDuc(11/09/2022)
     */
    onKeyDownTable() {
      if (this.indexFocus < this.properties.length - 1) {
        this.indexFocus++;
      }
    },
    /**
     * hàm thực hiện khi nhấn nút enter table
     * Author:TTDuc(11/09/2022)
     */
    onKeyEnterTable() {
      if (this.properties[this.indexFocus]) {
        let property = this.properties[this.indexFocus];

        this.selectItem(property);
      }
    },
    /**
     * hàm lấy ttn file import
     * Author : TTDuc(08/09/2022)
     */
    getFileImport(event) {
      try {
        // kiểm tra file rỗng thì gán bằng null
        console.log(event);
        this.file = event.target.files ? event.target.files[0] : null;
        event.target.value = null;
        // kiểm tra file excel hay không
        if (this.file.type === TypeOfExcel) {
          // thực set thông báo cho lệnh import
          this.commandName = CommandName.Import;
        } else {
          // thông báo sai định dạng file
          this.titleFormConfirm = ErrorMsg.TypeFileErr;
          this.commandName = CommandName.Notice;
        }
        // hiển thị form confirm
        this.isShowFormConfirm = true;
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * sự kiện mouse down trên 1 dòng dữ liệu
     * Author: TTDuc(08/09/2022)
     */
    mousedown(property) {
      try {
        // lưu tài sản khi mousedown
        this.propertyOnMouseDown = property;
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * sự kiện mouse up trên 1 dòng dữ liệu
     * Author: TTDuc(08/09/2022)
     */
    mouseup(property) {
      try {
        // lưu tài sản khi mouseup
        this.propertyOnMouseUp = property;
        this.indexFocus = this.properties.indexOf(property);
        this.currentProperty = this.properties;
        this.selectMultipleItem(
          this.propertyOnMouseDown,
          this.propertyOnMouseUp
        );
        this.propertyOnMouseDown = {};
        this.propertyOnMouseUp = {};
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * hàm thực hiện chọn nhiều item cùng lúc
     * Author : TTDuc(08/09/2022)
     */
    selectMultipleItem(item1, item2) {
      if (this.properties.includes(item1) && this.properties.includes(item2)) {
        // lấy vị trí của 2 item trong mảng properties
        let startIndex = this.properties.indexOf(item2);
        let endIndex = this.properties.indexOf(item1);

        // kiểm tra vị trí bắt đầu và kết thúc nếu bắt đầu lớn hơn kết thúc thì đổi lại
        if (startIndex > endIndex) {
          let tmp = startIndex;
          startIndex = endIndex;
          endIndex = tmp;
        }

        // thêm các item chưa có trong mảng selectedProperty vào mảng selectedProperty
        for (let i = startIndex; i <= endIndex; i++) {
          if (!this.selectedProperties.includes(this.properties[i])) {
            this.selectedProperties.push(this.properties[i]);
          }
        }

        this.indexFocus = this.properties.indexOf(item2);
      }
    },
    /**
     * hàm chọn pageNumber
     * Author: TTDuc (24/08/2022)
     */
    selectedPageSize(item) {
      this.pageSize = parseInt(item.name);
      this.getPagingProperties();
    },
    /**
     * hàm chọn department
     * Author: TTDuc (24/08/2022)
     */
    selectedDepartment(item) {
      if (item.departmentID) {
        this.departmentID = item.departmentID;
      } else {
        this.departmentID = "";
      }
      this.getPagingProperties();
    },
    selectedPropertyType(item) {
      if (item.propertyTypeID) {
        this.propertyTypeID = item.propertyTypeID;
      } else {
        this.propertyTypeID = "";
      }
      this.getPagingProperties();
    },
    /**
     * show MFormNotice
     * Author: TTDuc(10/08/2022)
     */
    showFormNotice() {
      setTimeout(() => {
        this.isShowFormNotice = false;
      }, 2000);
      this.isShowFormNotice = true;
    },
    /**
     * hàm xóa 1 item
     * Author: TTDuc (10/08/2022)
     */
    onClickDelete1(property) {
      this.currentProperty = property;
        this.isShowDelete1Item = true;
        this.commandName = CommandName.Delete;
      this.isShowFormConfirm = true;
    },

    onClickDeleteMultiple() {
      try {
        this.commandName = CommandName.Delete;
        // kiểm tra danh sách được chọn có bao nhiêu bản ghi và hiển thị thông báo
        if (this.selectedProperties.length == 0) {
          this.commandName = CommandName.Notice;
          this.titleFormConfirm = ErrorMsg.NotChooseProperty;
        } else if (this.selectedProperties.length == 1) {
          this.onClickDelete1(this.selectedProperties[0]);
        } else {
          if (this.countIncrement() == 0) {
            this.isShowDeleteManyItem = true;
          } else {
            this.noticeName = NoticeName.Increase;
            this.commandName = CommandName.Notice;
          }
        }
        this.isShowFormConfirm = true;
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * thực hiện khi form confrim trả về false
     * Author : TTDuc (24/08/2022)
     */
    formConfirmReturnFalse() {
      this.isShowFormConfirm = false;
      this.isShowDeleteManyItem = false;
      this.isShowDelete1Item = false;
      this.noticeName = NoticeName.NoAction;
      this.commandName = CommandName.NoAction;
    },
    /**
     * thực hiện khi formConfirm trả về True
     * Author: TTDuc (10/08/2022)
     */
    formConfirmReturnTrue() {
      this.isShowFormConfirm = false;
      this.isShowDeleteWarning = false;

      if (this.commandName == CommandName.Delete) {
        if (this.selectedProperties.length == 1) {
          //thực hiện xóa trên server
          let propertyTypeID = this.currentProperty.propertyTypeID;
          this.deleteProperty(propertyTypeID);
        } else {
          //thực hiện xóa trên server
          this.deleteMultipleProperty();
        }
        this.currentProperty = {};
        this.selectedProperties = [];
        this.isShowDeleteManyItem = false;
        this.isShowDelete1Item = false;
      } else if (this.commandName == CommandName.Import) {
        this.importExcel(this.file);
      } else if (
        this.commandName == CommandName.Notice &&
        (this.noticeName == NoticeName.Increase1 ||
          this.noticeName == NoticeName.Increase)
      ) {
        this.deletePropertyIncreased();
        this.noticeName = NoticeName.NoAction;
      }
      // xét lại giá trị ban đầu cho commandName và currentProperty
      this.commandName = CommandName.NoAction;
      this.titleFormConfirm = "";
    },
    deletePropertyIncreased() {
      this.selectedProperties = this.selectedProperties.filter((property) => {
        if (property.status == PropertyStatus.Normal) return property;
        return;
      });
    },
    /**
     * hàm thêm mới TS
     * Author: TTDuc (07/08/2022)
     */
    onClickAdd() {
      try {
        this.commandName = CommandName.Add;
        //mở FormDetail
        this.toggleFormDetail();
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * sự kiện click sửa property
     * Author: TTDuc(15/08/2022)
     */
    onClickEdit(property) {
      try {
        if (!this.selectedProperties.includes(property)) {
          //thực hiện chọn
          this.selectedProperties.push(property);
        }
        this.currentProperty = property;
        this.commandName = CommandName.Edit;
        this.toggleFormDetail();
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * đếm số tài sản đã được ghi tăng
     * Author: TTDuc(17/10/2022)
     */
    countIncrement() {
      let count = 0;
      for (const property of this.selectedProperties) {
        if (property.status == 1) count++;
      }
      return count;
    },
    /**
     * sự kiện nhân bản
     * Author: TTDuc (28/08/2022)
     */
    onClickDuplicate(property) {
      try {
        this.currentProperty = property;
        this.toggleFormDetail();
        this.commandName = CommandName.Duplicate;
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * close contextMenu
     * Author : TTDuc (08/08/2022)
     */
    closeContextMenu() {
      this.toggleContextMenu();
    },
    /**
     * khởi tại sự kiện ẩn hiện contextmenu
     * Author: TTDuc (08/08/2022)
     */
    toggleContextMenu() {
      this.isShowContextMenu = !this.isShowContextMenu;
    },
    /**
     * sự kiện contextmenu
     * Author: TTDuc (08/08/2022)
     */
    onClickContextMenu(property) {
      try {
        this.selectItem(property);
        this.toggleContextMenu();
        this.posTop = event.clientY;
        this.posLeft = event.clientX;
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * format monney
     * Author: TTDuc (08/08/2022)
     */
    formatMoney(money) {
      if (!isNaN(money)) {
        var moneyInt = parseInt(money);
        return moneyInt
          .toString()
          .replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
      } else {
        return parseInt(money);
      }
    },
    /**
     * hàm chọn 1 item
     * Author: TTDuc (06/08/2022)
     */
    selectItemToList(property) {
      try {
        this.currentProperty = property;
        this.indexFocus = this.properties.indexOf(property);
        if (!this.selectedProperties.includes(property)) {
          //thực hiện chọn
          this.selectedProperties.push(property);
        } else {
          //thực hiện bỏ chọn
          this.selectedProperties = this.selectedProperties.filter((a) => {
            return a !== property;
          });

          this.currentProperty =
            this.selectedProperties[this.selectedProperties.length - 1];
        }
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm chọn 1 item
     * Author: TTDuc ( 04/10/2022)
     */
    selectItem(property) {
      this.selectedProperties = [];
      this.selectedProperties.push(property);
      this.indexFocus = this.properties.indexOf(property);
      this.currentProperty = property;
    },
    /**
     * hàm chọn tất cả item
     * Author: TTDuc (06/08/2022)
     */
    selectedAllItem() {
      try {
        if (this.selectedProperties.length < this.properties.length) {
          this.selectedProperties = this.properties;
        } else {
          this.selectedProperties = [];
        }
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm ẩn hiện Form FormDetail
     * Author: TTDuc (07/08/2022)
     */
    toggleFormDetail() {
      this.isShowDetail = !this.isShowDetail;
      if (!this.isShowDetail) {
        // focus vào table
        this.$refs.tableFocus.focus();
      }
    },

    /**
     * sự kiện click add trên contextMenu
     * Author: TTDuc (28/08/2022)
     */
    onClickAddContextMenu() {
      try {
        this.onClickAdd();
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * sự kiện click edit trên contextMenu
     * Author: TTDuc (28/08/2022)
     */
    onClickEditContextMenu() {
      try {
        this.onClickEdit(this.currentProperty);
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * sự kiện click delete trên contextMenu
     * Author: TTDuc (28/08/2022)
     */
    onClickDeleteContextMenu() {
      try {
        this.onClickDelete1(this.currentProperty);
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * sự kiện click duplicate trên contextMenu
     * Author: TTDuc (28/08/2022)
     */
    onClickDuplicateContextMenu() {
      try {
        this.commandName = CommandName.Duplicate;
        this.toggleFormDetail();
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm thực hiện select khi chọn form-search
     * Author: TTDuc (07/09/2022)
     */
    onFocusFormSearch() {
      try {
        this.$refs.formSearch.select();
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm click nextPage
     * Author: TTDuc (7/08/2022)
     */
    nextPage() {
      if (this.pageNumber < this.maxPage) {
        this.pageNumber++;
        this.getPagingProperties();
      }
    },
    /**
     * hàm click vào prevPage
     * Author: TTDuc (7/08/2022)
     */
    prevPage() {
      if (this.pageNumber > 1) {
        this.pageNumber--;
        this.getPagingProperties();
      }
    },
    /**
     * hàm click vào 1 page number
     * Author: TTDuc (7/08/2022)
     */
    changePageNumber(selectedPage) {
      this.pageNumber = selectedPage;
      this.getPagingProperties();
    },
    /**
     * hàm tính tổng của 1 prop trong mảng object
     * Author: TTDuc(08/08/2022)
     */
    total(array, propName) {
      let total = array.reduce((acc, cur) => {
        if (cur[propName]) {
          return acc + cur[propName];
        } else {
          return acc;
        }
      }, 0);
      return total;
    },
    totalPresentValue() {
      let totalPresentValue = this.properties.reduce((acc, cur) => {
        if (cur.attritionValue && cur.trackingYear && cur.markedPrice) {
          return (
            acc +
            (cur.markedPrice -
              (this.getYear() - cur.trackingYear) * cur.attritionValue)
          );
        } else {
          return acc;
        }
      }, 0);

      return totalPresentValue;
    },
    /**
     * hàm xử lý sự kiện kho form detail trả về thành công
     * Author: TTDuc (07/09/2022)
     */
    formDetailSuccess() {
      this.toggleFormDetail();
      this.showFormNotice();
      this.getPagingProperties();
      this.currentProperty = {};
      this.selectedProperties = [];
    },
    /**
     * hàm lấy giá trị năm hiện tại
     * Author: TTDuc (07/09/2022)
     */
    getYear() {
      return new Date().getFullYear();
    },

    /**
     * hàm focus vào tr trên table
     * Author: TTDuc (15/09/2022)
     */
    focusToTrTag(tabindex) {
      try {
        if (this.$el.querySelector(`table tr[tabindex="${tabindex}"]`)) {
          this.$el.querySelector(`table tr[tabindex="${tabindex}"]`).focus();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /************************* làm việc với API **************************************/

    /**
     * hàm lọc, lấy danh sách , phân trang tài sản từ DB
     * Author: TTDuc 24/08/2022
     */
    getPagingProperties() {
      this.isShowLoading = true;
      this.selectedProperties = [];
      axios({
        url: `${BASE_URL_Property_Type}/filter?keyword=${this.keyword}`,
        method: "get",
        data: [],
      })
        .then((response) => {
          this.properties = response.data;
          this.isShowLoading = false;
        })
        .catch((e) => {
          console.log(e);
          this.isShowLoading = false;
        });
    },

    /**
     * xóa 1 tài sản trên DB
     * Author: TTDuc (24/07/2022)
     */
    deleteProperty(propertyTypeID) {
      this.isShowLoading = true;
      axios
        .get(`${BASE_URL_Property_Type}/validate-delete-property-type?propertyTypeId=${propertyTypeID}`)
        .then((response) => {
          if(response.data){
            this.executeDelete(propertyTypeID);
          }else{
            this.commandName = CommandName.Notice;
            this.isShowFormConfirm = true;
            this.isShowDeleteWarning = true;
            this.msgShowDeleteWarning = "Đã có tài sản thuộc loại tài sản này. Anh/chị cần xóa tài sản và các chứng từ phát sinh liên quan đến tài sản đó trước khi xóa loại tài sản.";
          }
          this.isShowLoading = false;
        })
        .catch((e) => {
          console.log(e);
          this.isShowLoading = false;
        });

      
    },

    /**
     * hàm thực hiện delete loại tài sản
     * @param {*} propertyTypeID 
     */
    executeDelete(propertyTypeID){
      this.isShowLoading = true;
      axios
        .delete(`${BASE_URL_Property_Type}/${propertyTypeID}`)
        .then((response) => {
          console.log(response.data);
          this.getPagingProperties();
          this.showFormNotice();
          this.isShowLoading = false;
        })
        .catch((e) => {
          console.log(e);
          this.isShowLoading = false;
        });
    },
    /**
     * lấy danh sách Phòng ban
     * Author: TTDuc(24/08/2022)
     */
    getDepartments() {
      axios
        .get(`${BASE_URL_Department}`)
        .then((response) => {
          this.departments = response.data;
        })
        .catch((e) => {
          console.log(e);
        });
    },
    /**
     * lấy danh sách Phòng ban
     * Author: TTDuc(24/08/2022)
     */
    getPropertyTypes() {
      axios
        .get(`${BASE_URL_Property_Type}`)
        .then((response) => {
          this.propertyTypes = response.data;
        })
        .catch((e) => {
          console.log(e);
        });
    },
    /**
     * delete nhiều tài sản cùng lúc
     * Author: TTDuc (30/08/2022)
     */
    deleteMultipleProperty() {
      this.isShowLoading = true;
      // lấy danh sách ID
      let listPropertyID = [];

      this.selectedProperties.filter((property) => {
        listPropertyID.push(property.propertyID);
      });

      // thực hiện gọi Axios
      axios({
        url: `${BASE_URL_Property}/delete-multiple`,
        method: Methods.Delete,
        data: listPropertyID,
      })
        .then(() => {
          this.getPagingProperties(); // lấy lại danh sách
          this.showFormNotice(); // thông báo thành công
        })
        .catch((err) => {
          console.log(err);
          this.isShowLoading = false;
        });
    },
    /**
     * hàm gọi API import excel
     */
    importExcel(file) {
      // loading
      this.isShowLoading = true;
      // tạo form data
      let formData = new FormData();
      formData.append("file", file);

      // gọi axios
      axios({
        url: `${BASE_URL_Property}/import-excel`,
        method: Methods.Post,
        data: formData,
      })
        .then((res) => {
          // show thành ông và lấy lại dữ liệu

          // nếu nhập khẩu được 0
          if (res.data.listId.length == 0) {
            this.titleFormConfirm = ErrorMsg.ImportErr;
          }
          // nhập khẩu được ít hơn tổng số bản ghi
          else if (res.data.totalCountError > 0) {
            let totalCountSuccesful = res.data.listId.length;
            let totalCountError = res.data.totalCountError;
            this.titleFormConfirm = `Thực hiện nhập khẩu thành công (${totalCountSuccesful}/${
              totalCountSuccesful + totalCountError
            }). Tài sản chưa được nhập khẩu có thể trùng mã hoặc thiếu dữ liệu!`;
          }
          // nhập khẩu được tất cả bản ghi
          else {
            this.titleFormConfirm = `Thực hiện nhập khẩu thành công ${res.data.listId.length} tài sản!`;
          }
          this.commandName = CommandName.Notice;
          this.isShowFormConfirm = true;
          this.getPagingProperties();
        })
        .catch((err) => console.log(err.data));
    },
  },

  mounted() {
    // mở trang focus vào table
    this.$nextTick(() => {
      this.$refs.tableFocus.focus();
    });
  },
  watch: {
    // theo dõi keyword
    keyword() {
      if (this.keyword == "") {
        this.getPagingProperties();
      }
    },
  },
};
</script>

<style scoped>
@import url(../../css/main.css);
@import url(../../css/layout/table.css);
@import url(../../css/page/category_page.css);
</style>
