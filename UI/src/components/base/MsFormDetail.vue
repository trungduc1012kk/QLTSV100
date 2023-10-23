<!-- eslint-disable no-debugger -->
<template>
  <div id="form-detail" class="popup" @keyup.esc.exact="closeForm">
    <div class="form-detail-container">
      <div class="form-detail-header">
        <div class="form-detail-text">
          <h2>{{ title }}</h2>
        </div>
        <ms-tool-tip content="Đóng">
          <div @click="closeForm" class="form-detail-close">
            <div class="icon">
              <div class="icon-close"></div>
            </div>
          </div>
        </ms-tool-tip>
      </div>
      <div class="form-detail-body">
        <div class="form-detail-content">
          <div class="row">
            <label for="">Mã tài sản <span>*</span></label>
            <div class="form-input">
              <input
                class="input"
                type="text"
                tabindex="1"
                id="inputFocus"
                v-model="property.propertyCode"
                :class="error.propertyCode != '' ? 'border-red' : ''"
                @blur="validate('propertyCode')"
                ref="propertyCode"
                :disabled="increased ? true : false"
              />
            </div>
            <p class="error" v-if="error.propertyCode != ''">
              <span>Mã tài sản </span>{{ error.propertyCode }}
            </p>
          </div>

          <div class="row col-2">
            <label for="">Tên tài sản <span>*</span></label>
            <div class="form-input">
              <input
                class="input"
                type="text"
                placeholder="Nhập tên tài sản"
                tabindex="2"
                v-model="property.propertyName"
                :class="error.propertyName != '' ? 'border-red' : ''"
                @blur="validate('propertyName')"
                ref="propertyName"
              />
            </div>
            <p class="error" v-if="error.propertyName != ''">
              <span>Tên tài sản </span>{{ error.propertyName }}
            </p>
          </div>
          <div class="row">
            <label for="">Mã bộ phận sử dụng <span>*</span></label>
            <ms-combobox-2
              label="Chọn mã bộ phận sử dụng"
              class="combobox"
              tab="3"
              @selectedItem="setDepartment"
              :items="departments"
              :code="item.departmentCode"
              fieldCode="departmentCode"
              fieldName="departmentName"
              @onBlur="validate('departmentCode')"
              :border="error.departmentCode != '' ? 'border-red' : ''"
              refName="departmentCode"
            />
            <p class="error" v-if="error.departmentCode != ''">
              <span>Mã phòng ban </span>{{ error.departmentCode }}
            </p>
          </div>
          <div class="row col-2">
            <label for="">Tên bộ phận sử dụng </label>
            <div class="form-input">
              <input
                class="input"
                type="text"
                disabled
                v-model="property.departmentName"
              />
            </div>
          </div>
          <div class="row">
            <label for="">Mã loại tài sản <span>*</span></label>
            <ms-combobox-2
              label="Chọn mã loại tài sản"
              class="combobox"
              tab="4"
              @selectedItem="setPropertyType"
              :items="propertyTypes"
              :code="item.propertyTypeCode"
              fieldCode="propertyTypeCode"
              fieldName="propertyTypeName"
              @onBlur="validate('propertyTypeCode')"
              :border="error.propertyTypeCode != '' ? 'border-red' : ''"
              refName="propertyTypeCode"
            />
            <p class="error" v-if="error.propertyTypeCode != ''">
              <span>Mã loại tài sản </span>{{ error.propertyTypeCode }}
            </p>
          </div>
          <div class="row col-2">
            <label for="">Tên loại tài sản</label>
            <div class="form-input">
              <input
                class="input"
                type="text"
                disabled
                v-model="property.propertyTypeName"
              />
            </div>
          </div>
          <div class="row">
            <label for="">Số lượng <span>*</span></label>
            <ms-input-number
              v-model="this.property.quantity"
              @onBlur="validate('quantity')"
              :decimalPlaces="0"
              tab="5"
              :border="error.quantity != '' ? 'border-red' : ''"
              :hasIcon="true"
              :max="1000"
              ref="quantity"
              :disabled="increased ? true : false"
            />
            <p class="error" v-if="error.quantity != ''">
              <span>Số lượng </span>{{ error.quantity }}
            </p>
          </div>
          <div class="row">
            <label for="">Nguyên giá <span>*</span></label>
            <ms-input-number
              v-model="this.property.markedPrice"
              @onBlur="validate('markedPrice')"
              :decimalPlaces="0"
              tab="6"
              :border="error.markedPrice != '' ? 'border-red' : ''"
              ref="markedPrice"
              :disabled="increased ? true : false"
            />
            <p class="error" v-if="error.markedPrice != ''">
              <span>Giá hiện tại </span>{{ error.markedPrice }}
            </p>
          </div>
          <div class="row">
            <label for="">Số năm sử dụng <span>*</span></label>
            <ms-input-number
              v-model="this.property.usedYear"
              @onBlur="validate('usedYear')"
              :decimalPlaces="0"
              tab="7"
              :border="error.usedYear != '' ? 'border-red' : ''"
              :max="100"
              ref="usedYear"
              :disabled="increased ? true : false"
            />

            <p class="error" v-if="error.usedYear != ''">
              <span>Số năm sử dụng </span>{{ error.usedYear }}
            </p>
          </div>

          <div class="row">
            <label for="">Tỷ lệ hao mòn(%) <span>*</span></label>
            <ms-input-number
              v-model="this.property.attritionRate"
              :max="100"
              @onBlur="validate('attritionRate')"
              :decimalPlaces="2"
              tab="8"
              :border="error.attritionRate != '' ? 'border-red' : ''"
              :hasIcon="true"
              ref="attritionRate"
              :disabled="increased ? true : false"
            />

            <p class="error" v-if="error.attritionRate != ''">
              <span>Tỷ lệ hao mòn </span>{{ error.attritionRate }}
            </p>
          </div>
          <div class="row">
            <label for="">Giá trị hao mòn <span>*</span></label>
            <ms-input-number
              v-model="this.property.attritionValue"
              @onBlur="validate('attritionValue')"
              :decimalPlaces="0"
              tab="9"
              :border="error.attritionValue != '' ? 'border-red' : ''"
              ref="attritionValue"
              :disabled="increased ? true : false"
            />
            <!-- @change="property.AttritionValue = formatMoney(property.attritionValue)" -->
            <p class="error" v-if="error.attritionValue != ''">
              <span>Giá trị hao mòn </span>{{ error.attritionValue }}
            </p>
          </div>
          <div class="row">
            <label for="">Năm theo dõi</label>
            <div class="form-input">
              <input
                class="input text-right"
                type="text"
                disabled
                v-model="property.trackingYear"
              />
            </div>
          </div>
          <div class="row">
            <label for="">Ngày mua <span>*</span></label>
            <div class="form-input">
              <el-date-picker
                v-model="property.purchasingDate"
                type="date"
                :format="dateConfig.Format"
                value-format="YYYY-MM-DD"
                tabindex="10"
                :class="error.purchasingDate != '' ? 'border-red' : ''"
                @blur="validate('purchasingDate')"
              >
              </el-date-picker>
            </div>
            <p class="error" v-if="error.purchasingDate != ''">
              <span>Ngày mua </span>{{ error.purchasingDate }}
            </p>
          </div>
          <div class="row">
            <label for="">Ngày bắt đầu sử dụng <span>*</span></label>
            <div class="form-input">
              <el-date-picker
                v-model="property.startUsingDate"
                type="date"
                :format="dateConfig.Format"
                value-format="YYYY-MM-DD"
                tabindex="11"
                @blur="validate('startUsingDate')"
                :class="error.startUsingDate != '' ? 'border-red' : ''"
              >
              </el-date-picker>
            </div>
            <p class="error" v-if="error.startUsingDate != ''">
              <span>Ngày bắt đầu sử dụng </span>{{ error.startUsingDate }}
            </p>
          </div>
        </div>
        <div></div>
      </div>
      <div class="form-detail-controler">
        <button class="button outline-button" tabindex="12" @click="closeForm">
          Hủy
        </button>
        <button class="button main-button" tabindex="13" @click="onClickSave">
          Lưu
        </button>
      </div>
    </div>
    <ms-form-confirm
      v-if="isShowFormConfirm"
      @returnTrue="formConfirmReturnTrue"
      @returnFalse="formConfirmReturnFalse"
      @returnNoSave="formConfirmReturnNoSave"
      :commandName="commandNameForm"
    >
      {{ contentFormConfirm }}
    </ms-form-confirm>
  </div>
</template>

<script>
import axios from "axios";
import MsCombobox2 from "./MsCombobox2.vue";
import MsInputNumber from "./MsInputNumber.vue";
import MsFormConfirm from "./MsFormConfirm.vue";
import { ElDatePicker } from "element-plus";
import vi from "element-plus/lib/locale/lang/vi";
import MsToolTip from "./MsToolTip.vue";
import {
  CommandName,
  CommandNameFormDetail,
  ErrorMsg,
  NoticeMsg,
  TitlePopup,
} from "../../js/common/resource";
import {
  BASE_URL_Department,
  BASE_URL_Property,
  BASE_URL_Property_Type,
} from "../../js/common/base-url";
import { DateConfig } from "../../js/common/config";
import { ErrorCode } from "../../js/common/enumeration";
import { GuidEmpty } from "../../js/common/resource";

export default {
  components: {
    MsCombobox2,
    MsInputNumber,
    MsFormConfirm,
    ElDatePicker,
    MsToolTip,
  },
  props: {
    item: Object,
    commandName: String,
  },
  data() {
    return {
      error: {
        propertyCode: "",
        propertyName: "",
        departmentCode: "",
        propertyTypeCode: "",
        quantity: "",
        markedPrice: "",
        usedYear: "",
        attritionRate: "",
        attritionValue: "",
        purchasingDate: "",
        startUsingDate: "",
      }, //  các thông báo lỗi
      property: {
        propertyCode: "",
        propertyName: "",
        propertyTypeCode: "",
        attritionRate: 0,
        attritionValue: 0,
        createdBy: "Trung Germany",
        createdDate: "1000-01-01",
        departmentCode: "",
        departmentID: "",
        departmentName: "",
        markedPrice: 0,
        modifiedBy: "Trung Germany",
        modifiedDate: "1000-01-01",
        propertyID: GuidEmpty,
        propertyTypeID: "",
        propertyTypeName: "",
        purchasingDate: "",
        quantity: 0,
        startUsingDate: "",
        trackingYear: 2022,
        usedYear: 0,
      }, // tài sản
      departments: [], // mảng phòng ban
      propertyTypes: [], // mảng loại tài sản
      rules: {
        propertyCode: {
          Required: true,
          MinLength: 7,
          MaxLength: 20,
        },
        propertyName: { Required: true, MaxLength: 255 },
        departmentCode: { Required: true },
        departmentName: { Required: true },
        propertyTypeCode: { Required: true },
        propertyTypeName: { Required: true },
        quantity: { Required: true, Min: 0 },
        markedPrice: {
          Required: true,
          Min: 0,
        },
        usedYear: { Required: true, Min: 0, Major: 100 },

        attritionRate: { Required: true, Major: 100 },
        attritionValue: {
          Required: true,
          Min: 0,
          LessThanMarkedPrice: true,
        },
        trackingYear: { Required: true },
        purchasingDate: { Required: true },
        startUsingDate: { Required: true },
      }, // object lưu điều kiện cần validate
      title: TitlePopup.AddProperty,
      isShowFormConfirm: false,
      commandNameForm: "", //
      contentFormConfirm: "", // nội dung hiển thị của formconfirm
      currentTabindex: 1, // giá trị hiện tại của tabindex
      ElDatePickerLocale: vi, // ngôn ngữ cho datepicker
      startWatch: false, // trạng thái đã mounted hay chưa
      edited: false, // trạng thái đã thay đổi thông hay chưa
      dateConfig: DateConfig,
      increased: false,
    };
  },
  methods: {
    /**
     * hàm validate giá trị hao mòn nhỏ hơn nguyên giá
     * Author: TTDuc(11/09/2022)
     */
    validateLessThanMarkedPrice() {
      if (this.property.attritionValue > this.property.markedPrice) {
        this.contentFormConfirm = ErrorMsg.AttritionValueLessThanMarkedPrice;
        this.commandNameForm = CommandNameFormDetail.Notice;
        this.isShowFormConfirm = true;
        this.error.attritionValue = ErrorMsg.AttritionValue;
        this.currentTabindex = 9;
        return false;
      }
      return true;
    },
    /**
     * validate tất cả các trường dữ liệu
     * Author: TTDuc (29/08/2022)
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
              isValid = this[functionName](this.rules[propName][key], propName);
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

      return isValidAll;
    },

    /**
     * focus vào trường nhập liệu bị lỗi
     * Author:TTDuc(28/09/2022)
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
     * hàm validate cho từng trường dữ liệu
     * Author: TTDuc (28/08/2022)
     */
    validate(propName) {
      // sau 0.2s thì validate để cập nhật dữ liệu trước khi validate
      setTimeout(() => {
        let isValid = true; //  biến lưu giá trị validate sau mỗi vòng for
        for (const key in this.rules[propName]) {
          let functionName = `validate${key}`;
          if (isValid == true) {
            isValid = this[functionName](this.rules[propName][key], propName);
          }
        }
      }, 200);
    },
    /**
     * validate tiền tố
     * Author : TTDuc(05/10/2022)
     */
    validatePrefix(value, propName) {
      if (this.property[propName].substring(0, value.length) === value) {
        return true;
      }
      this.error[propName] = `${ErrorMsg.Prefix} '${value}'`;
    },
    /**
     * validate nghiệp vụ
     * Author: TTDuc(11/09/2022)
     */
    validateMajor(value, propName) {
      // số năm sử dụng thay đổi thì kiểm tra lại tỷ lệ hao mòn
      if (propName == "usedYear") {
        if (
          this.property.attritionRate !=
          (value / this.property.usedYear).toFixed(2)
        ) {
          this.contentFormConfirm = ErrorMsg.AttritionRate;
          this.commandNameForm = CommandNameFormDetail.Notice;
          this.isShowFormConfirm = true;
          this.property.attritionRate = parseFloat(
            (value / this.property.usedYear).toFixed(2)
          );
          this.currentTabindex = 8;
          return false;
        }
        return true;
      }

      // tỷ lệ hao mòn thay đổi thì kiểm tra lại số năm sử dụng
      if (propName == "attritionRate") {
        if (
          this.property.attritionRate !=
          (value / this.property.usedYear).toFixed(2)
        ) {
          this.contentFormConfirm = ErrorMsg.UsedYear;
          this.commandNameForm = CommandNameFormDetail.Notice;
          this.isShowFormConfirm = true;
          this.property.usedYear = parseInt(
            value / this.property.attritionRate
          );
          this.property.attritionRate = parseFloat(
            (100 / this.property.usedYear).toFixed(2)
          );
          this.currentTabindex = 9;
          return false;
        }
        return true;
      }
    },
    /**
     * validate định dạng
     * Author: TTDuc(11/09/2022)
     */
    validateFormat(value, propName) {
      // kiểm tra không đúng định dạng thì lưu lại lỗi và trả về false
      if (!value.test(this.property[propName])) {
        this.error[propName] = ErrorMsg.Format;
        return false;
      }
      // ngược lại trả về true
      this.error[propName] = "";
      return true;
    },
    /**
     * validate trường bắt buộc nhập
     * Author : TTDuc (10/08/2022)
     */
    validateRequired(value, propName) {
      // kiểm tra rỗng thì lưu lại lỗi và trả về false
      if (this.property[propName] == null || this.property[propName] == "") {
        this.error[propName] = ErrorMsg.Required;
        return false;
      }
      this.error[propName] = "";
      return true;
    },

    /**
     * hàm validate số kí tự nhỏ nhất
     * Author :TTDuc(29/08/2022)
     */
    validateMinLength(value, propName) {
      if (this.property[propName].length < value) {
        this.error[propName] = ` ${ErrorMsg.MinLength} ${value} ký tự!`;
        return false;
      }
      this.error[propName] = "";
      return true;
    },

    /**
     * hàm validate số kí tự lớn nhất
     * Author :TTDuc(29/08/2022)
     */
    validateMaxLength(value, propName) {
      if (this.property[propName].length > value) {
        this.error[propName] = ` ${ErrorMsg.MaxLength} ${value} ký tự!`;
        return false;
      }
      this.error[propName] = "";
      return true;
    },

    /**
     * hàm validate min
     * Author:TTDuc(29/08/2022)
     */
    validateMin(value, propName) {
      if (this.property[propName] > value) {
        this.error[propName] = "";
        return true;
      }
      this.error[propName] = `${ErrorMsg.Min} ${value}`;
      return false;
    },

    /**
     * hàm validate max
     * Author:TTDuc(29/08/2022)
     */
    validateMax(value, propName) {
      if (this.property[propName] <= value) {
        this.error[propName] = "";
        return true;
      }
      this.error[propName] = `${ErrorMsg.Max} ${value}`;
      return false;
    },

    /**
     * hàm đóng form
     * Author: TTDuc (07/08/2022)
     */
    closeForm() {
      if (!this.edited) {
        this.$emit("closeForm");
      } else {
        this.commandNameForm = CommandNameFormDetail.Save;
        this.contentFormConfirm = NoticeMsg.PropertyEdited;
        this.isShowFormConfirm = true;
      }
    },

    /**
     * hàm gán giá trị cho các trường liên quan khi chọn department
     * Author: TTDuc (13/08/2022)
     */
    setDepartment(department) {
      if (this.disabled) return;
      console.log(department);
      this.property.departmentName = department.departmentName;
      this.property.departmentCode = department.departmentCode;
      this.property.departmentID = department.departmentID;
    },

    /**
     * hàm gán giá trị cho các trường liên quan khi chọn propertyType
     * Author: TTDuc (13/08/2022)
     */
    setPropertyType(propertyType) {
      if (this.increased) return;

      this.property.propertyTypeID = propertyType.propertyTypeID;
      this.property.propertyTypeName = propertyType.propertyTypeName;
      this.property.propertyTypeCode = propertyType.propertyTypeCode;
      this.property.attritionRate = propertyType.attritionRateDefault;
      this.property.usedYear = propertyType.usedYearDefault;
    },

    /**
     * format monney trả về định dạng nhóm 3 số cách nhau bằng dấu '.'
     * Author: TTDuc (08/08/2022)
     */
    formatMoney(money) {
      if (!isNaN(money)) {
        return money.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
      } else {
        return money;
      }
    },

    /**
     * hàm thực hiện chuyển về định dạng yyyy-mm-dd
     * Author: TTDuc (25/08/2022)
     */
    formatDate(date) {
      // nếu có định dạng date + time thì chỉ lấy date
      if (date != "") {
        return date;
      }

      // nếu date truyền vào rỗng thì tự động lấy ngày hiện tại
      var today = new Date();
      var year = today.getFullYear();
      var month = today.getMonth() + 1;
      var day = today.getDate();
      if (month < 10) {
        month = `0${month}`;
      }

      if (day < 10) {
        day = `0${day}`;
      }

      return `${year}-${month}-${day}`;
    },

    /**
     * hàm sự kiện click Lưu
     * Author:TTDuc (25/08/2022)
     */
    onClickSave() {
      try {
        this.commandNameForm = CommandNameFormDetail.Save;
        if (this.validateAll()) {
          if (this.commandName == CommandName.Edit) {
            this.editProperty();
          } else {
            this.addProperty();
          }
        }
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm xử lý khi form confirm trả về False
     * Author : TTDuc (06/09/2022)
     */
    formConfirmReturnFalse() {
      if (this.commandNameForm == CommandNameFormDetail.Close) {
        this.isShowFormConfirm = false;
      } else if (this.commandNameForm == CommandNameFormDetail.Save) {
        this.isShowFormConfirm = false;
      }

      this.isShowTitleOnClose = false;
      this.isShowTitleOnSave = false;
    },
    /**
     * hàm xử lý khi form confirm trả về True
     * Author : TTDuc (06/09/2022)
     */
    formConfirmReturnTrue() {
      if (this.commandNameForm == CommandNameFormDetail.Close) {
        this.$emit("closeForm");
      }
      // xử lý khi lưu
      else if (this.commandNameForm == CommandNameFormDetail.Save) {
        this.isShowTitleOnClose = false;
        this.isShowTitleOnSave = false;
        if (this.validateAll()) {
          if (this.commandName == CommandName.Edit) {
            this.editProperty();
          } else {
            this.addProperty();
          }
        }
      }
      // xử lý khi thông báo
      else if (this.commandNameForm === CommandNameFormDetail.Notice) {
        this.setFocus(this.currentTabindex);
      }

      this.isShowFormConfirm = false;
      this.commandNameForm = "";
    },
    /**
     * hàm xử lý sự kiện khi form confirm trả về không lưu
     * Author : TTDuc (06/09/2022)
     */
    formConfirmReturnNoSave() {
      if (this.commandNameForm == CommandNameFormDetail.Save) {
        this.$emit("closeForm");
      }
    },

    /***********************  API ******************************/

    /**
     * hàm lấy mã tài sản mới
     * Author: TTDuc (25/08/2022)
     */
    getNewPropertyCode() {
      axios({
        url: `${BASE_URL_Property}/new-code`,
        method: "Get",
      })
        .then((response) => {
          this.property.propertyCode = response.data;
        })
        .catch((err) => {
          if (err.response.status == 401) {
            this.$router.push("/login");
          }
          console.log(err.response);
        });
    },

    /**
     * lấy danh sách department
     * Author: TTDuc (25/08/2022)
     */
    getDepartments() {
      axios
        .get(`${BASE_URL_Department}`)
        .then((response) => {
          this.departments = response.data;
        })
        .catch((err) => {
          console.log(err);
        });
    },

    /**
     * lấy danh sách department
     * Author: TTDuc (25/08/2022)
     */
    getProeprtyTypes() {
      axios
        .get(`${BASE_URL_Property_Type}`)
        .then((response) => {
          this.propertyTypes = response.data;
        })
        .catch((err) => {
          console.log(err);
        });
    },

    /**
     * thêm tài sản
     * Author: TTDuc (25/08/2022)
     */
    addProperty() {
      axios
        .post(`${BASE_URL_Property}`, this.property)
        .then((response) => {
          console.log(response);
          this.$emit("success");
        })
        .catch((err) => {
          console.log(err.response.data);
          if (err.response.data.errorCode == ErrorCode.DuplicateCode) {
            setTimeout(() => {
              this.error.propertyCode = ErrorMsg.Duplicate;
              this.focusToError();
            }, 500);
          }
        });
    },

    /**
     * sửa tài sản
     * Author: TTDuc (29/08/2022)
     */
    editProperty() {
      axios
        .put(`${BASE_URL_Property}/${this.property.propertyID}`, this.property)
        .then((response) => {
          console.log(response);
          this.$emit("success");
        })
        .catch((err) => {
          console.log(err.response.data);
          if (err.response.data.errorCode == ErrorCode.DuplicateCode) {
            setTimeout(() => {
              this.error.propertyCode = ErrorMsg.Duplicate;
              this.focusToError();
            }, 500);
          }
        });
    },

    setFocus(tabindex) {
      if (this.$el.querySelector(`input[tabindex='${tabindex}']`)) {
        this.$el.querySelector(`input[tabindex='${tabindex}']`).focus();
      }
    },
  },

  created() {
    // thực hiện lấy danh sách department và propertyType
    this.getDepartments();
    this.getProeprtyTypes();

    // gán các Tài sản được truyền vào cho property
    this.property = Object.assign(this.property, this.item);

    //format ngày tháng cho startUsingDate và purchasingDate
    this.property.startUsingDate = this.formatDate(
      this.property.startUsingDate
    );
    this.property.purchasingDate = this.formatDate(
      this.property.purchasingDate
    );

    // kiểm tra nếu là form sửa thì xét lại tiêu đề
    if (this.commandName != CommandName.Edit) {
      this.getNewPropertyCode();
      this.property.propertyID = GuidEmpty;
    } else {
      this.title = TitlePopup.EditProperty;
    }

    //check ghi tăng
    if (this.item.status == 1 && this.commandName == "edit") {
      this.increased = true;
    }
  },

  mounted() {
    // focus vào ô tài sản khi bắt đầu mở trang
    if (this.increased) this.currentTabindex = 2;
    this.setFocus(this.currentTabindex);

    // startWatch
    setTimeout(() => {
      this.startWatch = true;
    }, 2000);
  },

  watch: {
    // tỷ lệ hao mòn thay đổi thì giá trị hao mòn thay đổi
    "property.attritionRate"(newValue, oldValue) {
      // check điều kiện sau khi mở form xong thì bắt đầu xử lý dữ liệu
      if (oldValue != 0 && newValue != this.item.attritionValue) {
        this.property.attritionValue =
          (this.property.markedPrice * this.property.attritionRate) / 100;
      }
    },

    // nguyên giá thay đổi thì giá trj hao mòn thay đổi
    "property.markedPrice"(newValue, oldValue) {
      // check điểu kiện sau khi mở form xong thì bắt đầu xử lý dữ liệu
      if (oldValue != 0 && newValue != this.item.attritionValue) {
        this.property.attritionValue =
          (this.property.markedPrice * this.property.attritionRate) / 100;
      }
    },

    property: {
      handler() {
        if (this.startWatch) {
          this.edited = true;
        }
      },
      deep: true, // deep watch
    },
  },
};
</script>

<style scoped>
@import url(../../css/main.css);
@import url(../../css/layout/form-detail.css);
</style>
