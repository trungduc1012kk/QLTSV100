<template>
  <div id="form-detail" class="popup" @keyup.esc.exact="closePopup">
    <div class="form-detail-container">
      <div class="form-detail-header">
        <div class="form-detail-text">
          <h2>{{ title }}</h2>
        </div>
        <div @click="closePopup" class="form-detail-close">
          <ms-tool-tip content="Đóng" position="right">
            <div class="icon">
              <div class="icon-close"></div>
            </div>
          </ms-tool-tip>
        </div>
      </div>
      <div class="form-detail-body">
        <slot name="body"></slot>
      </div>
      <div class="form-detail-controler">
        <slot name="control"></slot>
      </div>
    </div>
  </div>
</template>

<script>
import MsToolTip from "./MsToolTip.vue";
export default {
  components: {
    MsToolTip,
  },
  props: {
    title: {
      Type: String,
    },
    isShow: {
      Type: Boolean,
    },
  },
  data() {
    return {};
  },
  methods: {
    /**
     * hàm đóng form
     * Author : TTDuc (10/10/2022)
     */
    closePopup() {
      //   console.log(1);
      this.$emit("closePopup", false);
    },
  },
};
</script>

<style>
.form-detail-body {
  overflow: auto;
}
</style>
