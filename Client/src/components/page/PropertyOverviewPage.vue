<template>
  <div>
    <ms-popup title="Thêm tài sản" @close-form="isShow = false" v-if="isShow">
      <template #body>
        <div>
          <ms-table></ms-table>
        </div>
      </template>

      <template #control>
        <button
          class="button outline-button"
          tabindex="12"
          @click="isShow = false"
        >
          Hủy
        </button>
        <button class="button main-button" tabindex="13" @click="onClickSave">
          Lưu
        </button>
      </template>
    </ms-popup>
  </div>
</template>

<script>
// import MsToolTip from "../base/MsToolTip.vue";
import MsPopup from "../base/MsPopup.vue";
import MsTable from "../base/MsTable.vue";

export default {
  components: {
    // MsToolTip,
    MsPopup,
    MsTable,
  },
  data() {
    return {
      isShow: true,
    };
  },
  watch: {
    isShow() {
      console.log(1);
    },
  },
};
</script>

<style scoped>
/* div {
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 30px;
  color: #1aa4c8;
  font-weight: 700;
} */
</style>
