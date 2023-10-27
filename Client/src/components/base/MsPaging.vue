<template>
  <div class="paging">
    <div class="paging-item paging-prev" @click="prevPage">
      <div class="icon">
        <div class="icon-prev"></div>
      </div>
    </div>

    <div
      class="paging-item"
      v-for="(Page, index) in arrShow"
      :key="index"
      :class="Page == this.pageNumber ? 'active' : ''"
      @click="changePageNumber(Page)"
    >
      {{ Page }}
    </div>

    <div class="paging-item paging-next" @click="nextPage">
      <div class="icon">
        <div class="icon-next"></div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  methods: {
    nextPage() {
      try {
        this.$emit("nextPage");
      } catch (err) {
        console.log(err);
      }
    },
    prevPage() {
      try {
        this.$emit("prevPage");
      } catch (err) {
        console.log(err);
      }
    },
    changePageNumber(Page) {
      try {
        if (Page != "...") {
          this.selectedPage = Page;
          this.$emit("changePageNumber", this.selectedPage);
        }
      } catch (err) {
        console.log(err);
      }
    },
  },
  data() {
    return {
      numberPageDisplay: 3,
      selectedPage: 0,
    };
  },
  props: {
    pageNumber: {
      type: Number,
      required: true,
    },
    maxPage: {
      type: Number,
      required: true,
    },
  },
  computed: {
    arrShow() {
      if (this.maxPage < this.numberPageDisplay) {
        //nếu maxPage < 3 thì hiện 3 trang
        let arr = [];
        for (let i = 1; i <= this.maxPage; i++) {
          arr.push(i);
        }
        return arr;
      }
      if (this.pageNumber < this.numberPageDisplay) {
        //hiển thị 2 trang đầu và trang cuối
        return [1, 2, "...", this.maxPage];
      } else if (
        //hiển thị trang đầu trang cuối và trang hiện tại
        this.pageNumber >= this.numberPageDisplay &&
        this.pageNumber <= this.maxPage - this.numberPageDisplay + 1
      ) {
        return [1, "...", this.pageNumber, "...", this.maxPage];
      } else if (this.pageNumber >= this.maxPage - this.numberPageDisplay) {
        //hiển thị 2 trang cuối và trang đầu
        return [1, "...", this.maxPage - 1, this.maxPage];
      }
      return [];
    },
  },
};
</script>

<style scoped>
@import url(../../css/main.css);
</style>
