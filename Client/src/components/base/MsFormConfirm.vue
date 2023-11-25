<template>
  <div id="form-confirm" class="popup">
    <div class="form-confirm-container">
      <div class="form-confirm-noti">
        <div class="icon">
          <i class="fa-solid fa-triangle-exclamation"></i>
        </div>
        <slot></slot>
      </div>
      <div class="form-confirm-control">
        <button
          class="button outline-button"
          @click="returnFalse"
          v-if="isShowButton1"
          tabindex="16"
        >
          {{ titleButton1 }}
        </button>
        <button
          class="button sub-button"
          @click="returnNoSave"
          v-if="isShowButton2"
          tabindex="15"
        >
          {{ titleButton2 }}
        </button>
        <button
          class="button main-button"
          @click="returnTrue"
          v-if="isShowButton3"
          tabindex="14"
          ref="buttonSave"
        >
          {{ titleButton3 }}
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { CommandNameFormConfirm } from "../../js/common/resource";

export default {
  props: {
    commandName: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      titleButton1: "Hủy",
      titleButton2: "Không lưu",
      titleButton3: "Đồng ý",
      isShowButton1: false,
      isShowButton2: false,
      isShowButton3: false,
    };
  },
  methods: {
    /**
     * trả về false
     * Author: TTDuc(10/08/2022)
     */
    returnFalse() {
      try {
        this.$emit("returnFalse");
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * trả về True
     * Author: TTDuc(10/08/2022)
     */
    returnTrue() {
      try {
        this.$emit("returnTrue");
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * trả về Nosave
     * Author: TTDuc(10/08/2022)
     */
    returnNoSave() {
      try {
        this.$emit("returnNoSave");
      } catch (err) {
        console.log(err);
      }
    },
  },

  created() {
    //form đóng formDetail
    if (this.commandName === CommandNameFormConfirm.Close) {
      this.titleButton1 = "Không";
      this.titleButton3 = "Hủy bỏ";
      this.isShowButton1 = true;
      this.isShowButton3 = true;
    }
    //form xóa tài sản
    else if (this.commandName === CommandNameFormConfirm.Delete) {
      this.isShowButton1 = true;
      this.isShowButton3 = true;
    }
    // form lưu tài sản
    else if (this.commandName === CommandNameFormConfirm.Save) {
      this.titleButton1 = "Hủy bỏ";
      this.titleButton2 = "Lưu";
      this.titleButton3 = "Không lưu";
      this.isShowButton1 = true;
      this.isShowButton2 = true;
      this.isShowButton3 = true;
    } else if (this.commandName === CommandNameFormConfirm.Export) {
      this.titleButton1 = "Đóng";
      this.titleButton2 = "Xuất khẩu";
      this.titleButton3 = "Xuất khẩu tất cả";
      this.isShowButton1 = true;
      this.isShowButton2 = true;
      this.isShowButton3 = true;
    }
    // form thông báo
    else if (this.commandName === CommandNameFormConfirm.Notice) {
      this.isShowButton3 = true;
      this.titleButton3 = "Đóng";
    } else if (this.commandName === CommandNameFormConfirm.Import) {
      this.isShowButton1 = true;
      this.isShowButton3 = true;
    }
  },
  mounted() {
    this.$refs.buttonSave.focus();
  },
};
</script>
<style>
@import url(@/css/layout/form-confirm.css);
</style>
