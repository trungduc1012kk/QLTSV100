<template>
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
            <th class="text-center width-fit" v-if="hasSelect">
              <input
                type="checkbox"
                @click="selectedAllItem"
                :checked="selectedItems.length == items.length"
              />
            </th>
            <th class="text-center" data-tip="Số thứ tự">STT</th>
            <th
              v-for="column in columns"
              :key="column"
              :class="column.type ? column.type : ''"
            >
              {{ column.value }}
            </th>
          </tr>
        </thead>

        <tbody>
          <!-- || index === indexFocus -->
          <tr
            v-for="(item, index) in items"
            :key="item"
            :class="selectedItems.includes(item) ? 'active' : ''"
            @contextmenu.prevent="onClickContextMenu(item)"
            @mousedown.prevent.ctrl="mousedown(item)"
            @mouseup.prevent.ctrl="mouseup(item)"
            @click.prevent.exact="selectItem(item)"
            @click.prevent.shift="selectMultipleItem(currentItem, item)"
            @dblclick="onDblClickEdit(item)"
          >
            <td class="text-center" v-if="hasSelect">
              <input
                type="checkbox"
                :checked="selectedItems.includes(item) ? true : false"
                @click.stop="selectItemToList(item)"
              />
            </td>
            <td class="text-center">{{ index + 1 }}</td>
            <td
              v-for="(column, index) in columns"
              :key="index"
              :class="column.type ? column.type : ''"
            >
              <div v-if="column.type == 'text'">
                {{ item[column.name] }}
              </div>
              <div v-else-if="column.type == 'number'">
                {{ formatMoney(item[column.name]) }}
              </div>
              <div v-else-if="column.type == 'date'">
                {{ formatDate(item[column.name]) }}
              </div>
              <div v-else class="trow-control">
                <div
                  v-for="control in column[column.name]"
                  :key="control"
                  class="icon"
                  @click="this['onClick' + control](item)"
                >
                  <ms-tool-tip :content="commandVi[control]" position="left">
                    <div :class="'icon-' + control"></div>
                  </ms-tool-tip>
                </div>
              </div>
            </td>
          </tr>
          <tr style="height: 0; border: none"></tr>
        </tbody>
        <tfoot v-if="hasTotal && initData && initData.length > 0">
          <tr>
            <td v-if="hasSelect"></td>
            <td></td>
            <td
              v-for="(column, index) in columns"
              :key="index"
              class="bold"
              :class="column.type ? column.type : ''"
            >
              <div v-if="column.total && column.type == 'number'">
                {{ formatMoney(total(items, column.name)) }}
              </div>
              <div v-else></div>
            </td>
          </tr>
        </tfoot>
      </table>
    </div>
  </div>
</template>
<script>
import MsToolTip from "../base/MsToolTip.vue";

export default {
  components: {
    MsToolTip,
  },
  props: {
    hasTotal: {
      type: Boolean,
      default: false,
    },
    hasSelect: {
      type: Boolean,
      default: false,
    },
    initData: {
      type: Array,
    },
    initColumns: {
      type: Array,
    },
    autoSelect: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      items: [], // các item hiển thị trên grid
      selectedItems: [], // các item được chọn
      columns: [
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
          name: "presentValue",
          value: "Giá trị còn lại",
          type: "number",
          total: true,
        },
        {
          name: "control",
          value: "Chức năng",
          type: "control",
          control: ["Edit", "Duplicate"],
          total: false,
        },
      ], // column hiển thị
      currentItem: {}, // item được chọn
      itemOnMouseDown: {}, // item khi nhấp chuột
      itemOnMouseUp: {}, // item khi thả chuột
      commandVi: {
        Delete: "Xóa",
        Edit: "Sửa",
        Add: "Thêm",
        Duplicate: "Nhân bản",
      },
    };
  },
  created() {
    this.items = this.initData;

    if (this.initColumns) {
      this.columns = this.initColumns;
    }
  },
  methods: {
    /**
     * hàm edit item
     * Author: TTDuc (10/10/2022)
     */
    onClickEdit(item) {
      this.$emit("editItem", item);
    },
    /**
     * hàm nhân bản item
     * Author: TTDuc (10/10/2022)
     */
    onClickDuplicate(item) {
      this.$emit("duplicateItem", item);
    },
    /**
     * hàm nhân bản item
     * Author: TTDuc (10/10/2022)
     */
    onClickDelete(item) {
      this.selectItem(item);
      this.$emit("deleteItem", item);
    },
    /**
     * hàm chọn item
     * Author: TTDuc (10/10/2022)
     */
    selectItem(item) {
      this.selectedItems = [];
      this.selectedItems.push(item);
      this.currentItem = item;
      this.$emit("select", item);
    },
    /**
     * hàm chọn nhiều item
     * Author: TTDuc (10/10/2022)
     */
    selectMultipleItem(item1, item2) {
      try {
        if (this.items.includes(item1) && this.items.includes(item2)) {
          // lấy vị trí của 2 item trong mảng items
          let startIndex = this.items.indexOf(item2);
          let endIndex = this.items.indexOf(item1);

          // kiểm tra vị trí bắt đầu và kết thúc nếu bắt đầu lớn hơn kết thúc thì đổi lại
          if (startIndex > endIndex) {
            let tmp = startIndex;
            startIndex = endIndex;
            endIndex = tmp;
          }

          // thêm các item chưa có trong mảng selectedProperty vào mảng selectedProperty
          for (let i = startIndex; i <= endIndex; i++) {
            if (!this.selectedItems.includes(this.items[i])) {
              this.selectedItems.push(this.items[i]);
            }
          }

          // this.indexFocus = this.items.indexOf(item2);
        }
      } catch (error) {
        console.log();
      }
    },
    /**
     * hàm chọn tất cả item
     * Author: TTDuc (10/10/2022)
     */
    selectedAllItem() {
      try {
        if (this.selectedItems.length < this.items.length) {
          this.selectedItems = this.items;
        } else {
          this.selectedItems = [];
        }
      } catch (error) {
        console.log();
      }
    },
    selectItemToList(item) {
      try {
        this.currentItem = item;
        // this.indexFocus = this.items.indexOf(item);
        if (!this.selectedItems.includes(item)) {
          //thực hiện chọn
          this.selectedItems.push(item);
        } else {
          //thực hiện bỏ chọn
          this.selectedItems = this.selectedItems.filter((a) => {
            return a !== item;
          });

          this.currentItem = this.selectedItems[this.selectedItems.length - 1];
        }
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * nhấn chuột vào 1 item
     * Author: TTDuc (10/10/2022)
     */
    mousedown(item) {
      try {
        // lưu tài sản khi mousedown
        this.itemOnMouseDown = item;
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm thả chuột ở 1 item
     * Author: TTDuc (10/10/2022)
     */
    mouseup(item) {
      try {
        // lưu tài sản khi mouseup
        this.itemOnMouseUp = item;
        // this.indexFocus = this.items.indexOf(item);
        this.currentItem = item;
        this.selectMultipleItem(this.itemOnMouseDown, this.itemOnMouseUp);
        this.itemOnMouseDown = {};
        this.itemOnMouseUp = {};
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm mở context menu
     * Author: TTDuc (10/10/2022)
     */
    onClickContextMenu(item) {
      try {
        this.selectItem(item);
        this.$emit("onContextMenu", item);
      } catch (error) {
        console.log();
      }
    },
    /**
     * hàm click dúp vào item
     * Author: TTDuc (10/10/2022)
     */
    onDblClickEdit(item) {
      try {
        this.$emit("onDblClick", item);
      } catch (error) {
        console.log();
      }
    },
    /**
     * hàm tính tổng
     * Author: TTDuc (10/10/2022)
     */
    total(array, propName) {
      try {
        let total = array.reduce((acc, cur) => {
          if (cur[propName]) {
            return acc + cur[propName];
          } else {
            return acc;
          }
        }, 0);
        return total;
      } catch (error) {
        console.log();
      }
    },
    /**
     * hàm format tiền
     * Author: TTDuc (10/10/2022)
     */
    formatMoney(money) {
      try {
        if (!isNaN(money)) {
          var moneyInt = parseInt(money);
          return moneyInt
            .toString()
            .replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
        } else {
          return 0;
        }
      } catch (error) {
        console.log();
      }
    },
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

      return `${day}/${month}/${year}`;
    },
    hasData() {
      if (this.items && this.items == []) {
        return true;
      }
      return false;
    },
  },
  mounted() {
    this.$nextTick(() => {
      this.$refs.tableFocus.focus();
    });
  },
  watch: {
    initData() {
      this.items = this.initData;

      if (this.autoSelect && this.initData.length > 0) {
        this.selectItem(this.items[0]);
      }
    },
  },
};
</script>
<style scoped>
td.date > div {
  text-align: center;
}
</style>
