<template>
  <div
    class="m-combobox m-combobox-2"
    @click="toggleCombobox"
    @keyup.enter="selectItem"
    @keyup.down.stop="keyDown"
    @keyup.up.stop="keyUp"
  >
    <input
      class="input"
      v-model="keyword"
      :placeholder="this.placeholder"
      :tabindex="tab"
      :class="border"
      @focus="onFocus"
      @blur="onBlur"
      :ref="refName"
    />

    <div class="up-down">
      <div class="down">
        <div class="btn icon-down-bold"></div>
      </div>
    </div>

    <div class="drop-down" v-show="isShow">
      <div class="drop-down-header">
        <div class="drop-down-item">
          <div class="drop-down-code">Mã</div>
          <div class="drop-down-name">Tên</div>
        </div>
      </div>

      <div class="drop-down-body">
        <div
          v-for="item in dataItems"
          class="drop-down-item"
          :class="item[fieldCode] == currentItem[fieldCode] ? 'active' : ''"
          @click.exact.stop="onClickItem(item)"
          :key="item"
        >
          <div class="drop-down-code">{{ item[fieldCode] }}</div>
          <div class="drop-down-name">{{ item[fieldName] }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
    },
    items: {
      type: Array,
    },
    fieldName: {
      type: String,
    },
    fieldCode: {
      type: String,
    },
    code: {
      type: String,
    },
    tab: {
      type: String,
    },
    border: {
      type: String,
    },
    refName: {
      type: String,
    },
  },
  data() {
    return {
      i: -1, // lưu thứ tự item trong mảng items
      currentItem: {}, // item hiện tại
      isShow: false, // show drop-down
      placeholder: "Nhập giá trị ", // playholder
      heightOfItem: 35, // chiều cao của 1 item
      heightOfBody: 135, // chiều cao chủa drop-down-body
      dataItems: [],
      keyword: "",
    };
  },
  methods: {
    /**
     * hàm xử lý sự kiện khi blur
     * Author: TTDuc (07/09/2022)
     */
    onBlur() {
      setTimeout(() => {
        this.isShow = false;
      }, 200);

      this.$emit("onBlur");
    },
    /**
     * hàm chọn item khi nhấn enter
     * Author : TTDuc(07/09/2022)
     */
    selectItem() {
      this.$emit("selectedItem", this.currentItem);
      this.keyword = this.currentItem[this.fieldCode];
      this.placeholder = this.currentItem[this.fieldCode];
      this.isShow = false;

      this.i = 0;
      this.$el.querySelector(".input").focus();
      this.dataItems = this.items;
    },
    /**
     * hàm click chọn item
     * Author: TTDuc (25/08/2022)
     */
    onClickItem(item) {
      this.currentItem = item;
      console.log(item);
      this.selectItem();
    },
    /**
     * khởi tạo hàm mở combobox
     * Author: TTDuc (06/08/2022)
     */
    toggleCombobox() {
      this.isShow = !this.isShow;

      if (this.isShow == true) {
        this.scrollTrackingItem((this.i - 1) * this.heightOfItem);
      }
    },
    /**
     * hàm khởi tạo sự kiện keyDown
     * Author: TTDuc (13/08/2022)
     */
    keyDown() {
      try {
        if (!this.isShow) {
          this.isShow = true;
        } else {
          this.i++;
          if (this.i > this.dataItems.length - 1) {
            this.i = this.dataItems.length - 1;
          }
          this.currentItem = this.dataItems[this.i];
          this.scrollTrackingItem((this.i - 1) * this.heightOfItem);
        }
        console.log(this.i, this.currentItem);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xử lý sự kiện key up
     * Author : TTDuc (13/12/2022)
     */
    keyUp() {
      try {
        if (this.i > 0) {
          this.i--;
          this.currentItem = this.dataItems[this.i];
          this.scrollTrackingItem((this.i - 1) * this.heightOfItem);
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm xử lý sự kiện scrollHeight
     * Author: TTDuc (13/8/2022)
     */
    scrollTrackingItem(position) {
      if (position >= 0) {
        this.$el.querySelector(".drop-down-body").scrollTo(0, position);
      }
    },
    /**
     * hàm xử lý sự kiện focus
     * Author: TTDuc (07/09/2022)
     */
    onFocus() {
      this.$el.querySelector(".input").select(); // khi focus thì select
    },
  },
  created() {
    this.placeholder = this.label;
  },
  watch: {
    items() {
      // nhận mảng item để hiển thị
      this.dataItems = this.items;
    },

    keyword() {
      if (this.keyword != this.currentItem[this.fieldCode]) {
        this.isShow = true;
      }

      if (this.keyword) {
        this.dataItems = this.items.filter((item) => {
          let valueFilter = item[this.fieldCode].trim().toLowerCase();
          if (valueFilter.includes(this.keyword.trim().toLowerCase())) {
            return item;
          }
        });
      } else {
        this.dataItems = this.items;
      }
    },
  },
  mounted() {
    // xét giá trị ban đầu cho combobox
    this.currentItem[this.fieldCode] = this.code;
    this.keyword = this.code;

    // lấy vị trí scroll hiện tại
    this.items.filter((item, index) => {
      if (item[this.fieldCode] === this.currentItem[this.fieldCode]) {
        this.i = index;
      }
    });
  },
};
</script>

<style scoped>
@import url(../../css/main.css);
@import url(../../css/base/combobox.css);
</style>
