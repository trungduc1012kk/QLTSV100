<template>
  <div id="context-menu" @click="close">
    <div
      class="context-menu-content"
      :style="{ top: posTop + 'px', left: posLeft + 'px' }"
      :class="showAbove ? 'showAbove' : ''"
    >
      <div
        class="context-menu-item"
        @click="this.$emit(item.method)"
        v-for="(item, index) in items"
        :key="index"
      >
        <div class="icon">
          <div :class="item.icon"></div>
        </div>
        <div class="text">{{ item.text }}</div>
      </div>
      <!-- <div class="context-menu-item" @click="onClickAdd">
        <div class="icon">
          <div class="icon-add"></div>
        </div>
        <div class="text">Thêm tài sản</div>
      </div>
      <div class="context-menu-item" @click="onClickEdit">
        <div class="icon">
          <div class="icon-Edit"></div>
        </div>
        <div class="text">Sửa tài sản</div>
      </div>
      <div class="context-menu-item" @click="onClickDelete">
        <div class="icon">
          <div class="icon-delete-black"></div>
        </div>
        <div class="text">Xóa tài sản</div>
      </div>
      <div class="context-menu-item" @click="onClickDuplicate">
        <div class="icon">
          <div class="icon-Duplicate"></div>
        </div>
        <div class="text">Nhân bản tài sản</div>
      </div> -->
    </div>
  </div>
</template>

<script>
export default {
  props: {
    posTop: {
      type: Number,
      default: 100000,
    },
    posLeft: {
      type: Number,
      default: 100000,
    },
    initItem: { type: Array },
  },
  data() {
    return {
      heightOfContextMenu: 150,
      showAbove: false,
      items: [
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
  methods: {
    /**
     * hàm đóng form
     * Author : TTDuc (15/08/2022)
     */
    close() {
      this.$emit("close");
    },

    /**
     * hàm thêm mới nhân viên
     * Author : TTDuc (15/08/2022)
     */
    onClickAdd() {
      this.$emit("clickAdd");
    },
    /**
     * hàm thêm mới nhân viên
     * Author : TTDuc (15/08/2022)
     */
    onClickEdit() {
      this.$emit("clickEdit");
    },
    /**
     * hàm thêm mới nhân viên
     * Author : TTDuc (15/08/2022)
     */
    onClickDelete() {
      this.$emit("clickDelete");
    },
    /**
     * hàm thêm mới nhân viên
     * Author : TTDuc (15/08/2022)
     */
    onClickDuplicate() {
      this.$emit("clickDuplicate");
    },
  },
  mounted() {
    if (this.posTop > window.innerHeight - this.heightOfContextMenu) {
      this.showAbove = true;
    }

    this.items = this.initItem;
  },
};
</script>

<style scoped>
@import url(./../../css/layout/context-menu.css);
</style>
