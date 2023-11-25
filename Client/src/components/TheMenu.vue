<template>
  <div class="menu">
    <div class="menu-top">
      <router-link to="/" class="brand" @click="activeNumber = -1">
        <div class="icon-logo logo"></div>
        <h1 class="brand-text" v-if="isZoomOut">MISA QLTS</h1>
      </router-link>
      <ul class="navbar">
        <div v-for="item in items" :key="item" class="nav-item">
          <router-link
            class="nav-item-parent"
            @click="
              item.openChild = !item.openChild;
              activeTo = item.to;
            "
            :to="item.to"
            :class="activeTo === item.to ? 'active' : ''"
          >
            <div class="icon">
              <div :class="item.icon"></div>
            </div>
            <div class="nav-item-text" v-if="isZoomOut">{{ item.name }}</div>
            <div class="icon-down" v-if="item.subMenu && isZoomOut"></div>
          </router-link>
          <div
            class="nav-item-childs"
            v-if="item.subMenu && activeTo === item.to"
          >
            <router-link
              class="nav-item-child"
              v-for="(child, index) in item.subMenu"
              :key="index"
              v-show="item.openChild && isZoomOut"
              :to="child.subTo"
            >
              <div class="icon">
                <div class="icon-active"></div>
              </div>
              <div class="text">{{ child.subName }}</div>
            </router-link>
          </div>
        </div>
      </ul>
    </div>
    <div class="menu-bottom">
      <button class="btn zoom-in" id="zoom" @click="zoomNavbar">
        <div class="btn-icon center">
          <div :class="isZoomOut ? 'icon-left' : 'icon-right'"></div>
        </div>
      </button>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      isZoomOut: true,
      activeTo: "",
      items: [
        // {
        //   icon: "icon-tivi",
        //   name: "Tổng quan",
        //   openChild: false,
        //   to: "/tongquan",
        // },
        {
          icon: "icon-oto",
          name: "Tài sản",
          openChild: false,
          to: "/taisan",
          // subMenu: [
          //   {
          //     subName: "Ghi Tăng",
          //     subTo: "/ghitang",
          //   },
          //   {
          //     subName: "Thay đổi thông tin",
          //     subTo: "/timkiem",
          //   },
          //   {
          //     subName: "Đánh giá lại",
          //     subTo: "/baocao",
          //   },
          // ],
        },
        {
          icon: "icon-tivi",
          name: "Ghi tăng",
          openChild: false,
          to: "/ghitang",
        },
        // {
        //   icon: "icon-street",
        //   name: "Tài sản HT-DB",
        //   openChild: false,
        //   to: "/taisanhbdt",
        // },
        // {
        //   icon: "icon-tool",
        //   name: "Công cụ công dụng",
        //   openChild: false,
        //   to: "/congcu",
        // },
        {
          icon: "icon-category",
          name: "Loại tài sản",
          openChild: false,
          to: "/loaitaisan",
        },
        {
          icon: "icon-search",
          name: "Phòng ban",
          openChild: false,
          to: "/phongban",
        },
        // {
        //   icon: "icon-report",
        //   name: "Báo cáo",
        //   openChild: false,
        //   to: "/baocao",
        // },
      ],
    };
  },
  methods: {
    /**
     * hàm bắt sự kiện thu phóng navbar
     * Author: TTDuc(07/08/2022)
     */
    zoomNavbar() {
      this.isZoomOut = !this.isZoomOut;
      this.$emit("zoomNavbar");
    },
  },
};
</script>
