<template>
  <div
    class="m-combobox"
    :class="hasIcon === 'false' ? 'm-combobox-mini' : ''"
    @click="openCombobox"
    @keyup.enter="selectItem"
    @keyup.down.stop="keyDown"
    @keyup.up.stop="keyUp"
  >
    <div class="icon" v-if="hasFiltering">
      <div class="icon-filter"></div>
    </div>

    <input
      class="input"
      type="text"
      v-model="keyword"
      :placeholder="title"
      @blur="onBlur"
      :readonly="!hasFiltering"
      @focus="onFocus"
      :class="border"
    />

    <div class="up-down">
      <div class="down">
        <div class="btn icon-down-bold"></div>
      </div>
    </div>

    <div class="drop-down" v-show="isShow">
      <div
        class="drop-down-item"
        v-for="item in dataItems"
        :key="item"
        :class="item == currentItem ? 'active' : ''"
        @click="onClickItem(item)"
      >
        {{ item[fieldName] }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    hasFiltering: {
      type: Boolean,
      default: false,
    },
    hasIcon: {
      type: String,
      default: "true",
    },
    items: {
      type: Array,
      required: true,
    },
    title: {
      type: String,
    },
    fieldName: {
      type: String,
      default: "name",
    },
    itemDefault: {
      type: Boolean,
    },
    initValue: {
      type: String,
    },
    border: {
      type: String,
    },
  },
  data() {
    return {
      i: -1, // lưu thứ tự item trong mảng items
      isShow: false, // hiển thị danh sách drop-down
      currentItem: {}, // item được chọn
      heightOfItem: 35, // chiều cao của 1 item
      heightOfBody: 135, // chiều cao chủa drop-down-body
      dataItems: [], // mảng items hiển thị
      keyword: "", // từ khóa để lọc
    };
  },
  methods: {
    /**
     * hàm xử lý sự kiện khi Blur
     * Author : (06/08/2022)
     */
    onBlur() {
      setTimeout(() => {
        this.isShow = false;
      }, 200);
    },
    /**
     * khởi tạo hàm mở combobox
     * Author: TTDuc (06/08/2022)
     */
    openCombobox() {
      this.isShow = !this.isShow;
      this.scrollToCurentItem();
    },
    /**
     * hàm thực hiện chọn 1 items
     * Author: TTDuc (24/08/2022)
     */
    onClickItem(item) {
      try {
        this.currentItem = item;
        this.$emit("comboboxSelectedItem", this.currentItem);
        this.keyword = this.currentItem[this.fieldName];
      } catch (err) {
        console.log(err);
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
        this.$el.querySelector(".drop-down").scrollTo(0, position);
      }
    },
    /**
     * hàm xử lý scroll đến item đã được chọn
     * Author : TTDuc (06/08/2022)
     */
    scrollToCurentItem() {
      this.items.filter((item, index) => {
        if (item === this.currentItem) {
          this.$el
            .querySelector(".drop-down")
            .scrollTo(0, index * this.heightOfItem);
        }
      });
    },
    /**
     * hàm chọn item khi nhấn enter
     * Author : TTDuc(07/09/2022)
     */
    selectItem() {
      this.$emit("comboboxSelectedItem", this.currentItem);
      this.keyword = this.currentItem[this.fieldName];
      this.isShow = false;

      this.i = 0;
      this.$el.querySelector(".input").focus();
      this.dataItems = this.items;
    },
    /**
     * hàm thực hiện lọc
     * Author: TTDuc (07/08/2022)
     */
    filter() {
      if (this.hasFiltering && this.keyword != "") {
        this.dataItems = this.items.filter((item) => {
          let valueFilter = item[this.fieldName].trim().toLowerCase();
          if (valueFilter.includes(this.keyword.trim().toLowerCase())) {
            return item;
          }
        });
      } else {
        this.dataItems = this.items;
      }
    },
    /**
     * hàm thực hiện chọn tất cả text
     * Author: TTDuc (07/09/2022)
     */
    onFocus() {
      try {
        if (this.hasFiltering) {
          this.$el.querySelector(".input").select();
        }
      } catch (err) {
        console.log(err);
      }
    },
  },
  created() {
    // kiểm tra nếu cần có item mặc định thì lấy item 0
    if (this.items && this.itemDefault) {
      this.currentItem = this.items[0];
    }

    for (const item of this.items) {
      if (item[this.fieldName] == this.initValue) {
        this.currentItem = item;
      }
    }

    if (!this.hasFiltering) {
      this.keyword = this.currentItem[this.fieldName];
    }

    this.dataItems = this.items;
  },
  watch: {
    keyword() {
      if (this.keyword == "") {
        this.isShow = false;
        this.currentItem = {};
        this.$emit("comboboxSelectedItem", this.currentItem);
      }

      this.filter();
    },
  },
};
</script>

<style scoped>
@import url(../../css/base/combobox.css);
</style>
