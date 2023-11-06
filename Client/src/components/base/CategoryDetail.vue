<!-- eslint-disable no-debugger -->
<template>
  <div id="categoryDetail" class="popup" @keyup.esc.exact="closeForm">
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
            <label for="">Mã loại tài sản <span>*</span></label>
            <div class="form-input">
              <input
                class="input"
                type="text"
                tabindex="1"
                id="inputFocus"
                v-model="property.propertyTypeCode"
                :class="error.propertyTypeCode != '' ? 'border-red' : ''"
                @blur="validate('propertyTypeCode')"
                ref="propertyCode"
                :disabled="increased ? true : false"
              />
            </div>
            <p class="error" v-if="error.propertyTypeCode != ''">
              <span>Mã loại tài sản </span>{{ error.propertyTypeCode }}
            </p>
          </div>

          <div class="row">
            <label for="">Tên loại tài sản <span>*</span></label>
            <div class="form-input">
              <input
                class="input"
                type="text"
                placeholder="Nhập tên loại tài sản"
                tabindex="2"
                v-model="property.propertyTypeName"
                :class="error.propertyTypeName != '' ? 'border-red' : ''"
                @blur="validate('propertyTypeName')"
                ref="propertyName"
              />
            </div>
            <p class="error" v-if="error.propertyTypeName != ''">
              <span>Tên loại tài sản </span>{{ error.propertyTypeName }}
            </p>
          </div>
          <div class="row">
            <label for="">Số năm sử dụng <span>*</span></label>
            <ms-input-number
              v-model="this.property.usedYearDefault"
              @onBlur="validate('usedYearDefault')"
              :decimalPlaces="0"
              tab="7"
              :border="error.usedYearDefault != '' ? 'border-red' : ''"
              :max="100"
              ref="usedYearDefault"
              :disabled="increased ? true : false"
            />

            <p class="error" v-if="error.usedYearDefault != ''">
              <span>Số năm sử dụng </span>{{ error.usedYearDefault }}
            </p>
          </div>

          <div class="row">
            <label for="">Tỷ lệ hao mòn(%) <span>*</span></label>
            <ms-input-number
              v-model="this.property.attritionRateDefault"
              :max="100"
              @onBlur="validate('attritionRateDefault')"
              :decimalPlaces="2"
              tab="8"
              :border="error.attritionRateDefault != '' ? 'border-red' : ''"
              :hasIcon="true"
              ref="attritionRateDefault"
              :disabled="increased ? true : false"
            />

            <p class="error" v-if="error.attritionRateDefault != ''">
              <span>Tỷ lệ hao mòn </span>{{ error.attritionRateDefault }}
            </p>
          </div>
          <div class="row col-2">
            <label for="">Ghi chú</label>
            <div class="form-input">
            <input
                class="input"
                type="text"
                placeholder="Nhập ghi chú"
                tabindex="9"
                v-model="property.description"
                :class="error.description != '' ? 'border-red' : ''"
                ref="description"
              />
            </div>
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
        propertyTypeID: "",
        propertyTypeCode: "",
        propertyTypeName: "",
        attritionRateDefault: "",
        description: "",
        usedYearDefault: "",
        createdBy: "",
        createdDate: "",
        modifiedBy: "",
        modifiedDate: "",
      }, //  các thông báo lỗi
      property: {
        propertyTypeID: GuidEmpty,
        propertyTypeCode: "",
        propertyTypeName: "",
        attritionRateDefault: 0,
        usedYearDefault: 0,
        description: "",
        createdBy: "Trung Germany",
        createdDate: "1000-01-01",
        modifiedBy: "Trung Germany",
        modifiedDate: "1000-01-01",
      }, // tài sản
      rules: {
        propertyTypeCode: {
          Required: true,
          MinLength: 1,
          MaxLength: 20,
        },
        propertyTypeName: { Required: true, MaxLength: 255 },
        usedYearDefault: { Required: true, Min: 0, Major: 100 },

        attritionRateDefault: { Required: true, Major: 100 },
      }, // object lưu điều kiện cần validate
      title: TitlePopup.AddPropertyType,
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
      if (propName == "usedYearDefault") {
        if (
          this.property.attritionRateDefault !=
          (value / this.property.usedYearDefault).toFixed(2)
        ) {
          this.contentFormConfirm = ErrorMsg.AttritionRate;
          this.commandNameForm = CommandNameFormDetail.Notice;
          this.isShowFormConfirm = true;
          this.property.attritionRateDefault = parseFloat(
            (value / this.property.usedYearDefault).toFixed(2)
          );
          this.currentTabindex = 8;
          return false;
        }
        return true;
      }

      // tỷ lệ hao mòn thay đổi thì kiểm tra lại số năm sử dụng
      if (propName == "attritionRateDefault") {
        if (
          this.property.attritionRateDefault !=
          (value / this.property.usedYearDefault).toFixed(2)
        ) {
          this.contentFormConfirm = ErrorMsg.UsedYear;
          this.commandNameForm = CommandNameFormDetail.Notice;
          this.isShowFormConfirm = true;
          this.property.usedYearDefault = parseInt(
            value / this.property.attritionRateDefault
          );
          this.property.attritionRateDefault = parseFloat(
            (100 / this.property.usedYearDefault).toFixed(2)
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
     * thêm tài sản
     * Author: TTDuc (25/08/2022)
     */
     addProperty() {
      axios
        .post(`${BASE_URL_Property_Type}`, this.property)
        .then((response) => {
          console.log(response);
          this.$emit("success");
        })
        .catch((err) => {
          console.log(err.response.data);
          if (err.response.data.errorCode == ErrorCode.DuplicateCode) {
            setTimeout(() => {
              this.error.propertyTypeCode = ErrorMsg.Duplicate;
              this.focusToError();
            }, 500);
          }
        });
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
        url: `${BASE_URL_Property_Type}/new-code`,
        method: "Get",
      })
        .then((response) => {
          this.property.propertyTypeCode = response.data ? response.data.toString() : "";
        })
        .catch((err) => {
          if (err.response.status == 401) {
            this.$router.push("/login");
          }
          console.log(err.response);
        });
    },

    /**
     * sửa tài sản
     * Author: TTDuc (29/08/2022)
     */
    editProperty() {
      axios
        .put(`${BASE_URL_Property_Type}/${this.property.propertyTypeID}`, this.property)
        .then((response) => {
          console.log(response);
          this.$emit("success");
        })
        .catch((err) => {
          console.log(err.response.data);
          if (err.response.data.errorCode == ErrorCode.DuplicateCode) {
            setTimeout(() => {
              this.error.propertyTypeCode = ErrorMsg.Duplicate;
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

    // gán các Tài sản được truyền vào cho property
    this.property = Object.assign(this.property, this.item);

    // kiểm tra nếu là form sửa thì xét lại tiêu đề
    if (this.commandName != CommandName.Edit) {
      this.getNewPropertyCode();
      this.property.propertyTypeID = GuidEmpty;
    } else {
      this.title = TitlePopup.EditPropertyType;
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
    "property.attritionRateDefault"(newValue, oldValue) {
      // check điều kiện sau khi mở form xong thì bắt đầu xử lý dữ liệu
      if (oldValue != 0 && newValue != this.item.attritionValue) {
        this.property.attritionValue =
          (this.property.markedPrice * this.property.attritionRateDefault) / 100;
      }
    },

    // nguyên giá thay đổi thì giá trj hao mòn thay đổi
    "property.markedPrice"(newValue, oldValue) {
      // check điểu kiện sau khi mở form xong thì bắt đầu xử lý dữ liệu
      if (oldValue != 0 && newValue != this.item.attritionValue) {
        this.property.attritionValue =
          (this.property.markedPrice * this.property.attritionRateDefault) / 100;
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
@import url(../../css/layout/category-detail.css);
</style>
