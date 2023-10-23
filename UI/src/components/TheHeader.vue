<template>
  <div class="header">
    <div class="header-left">
      <h3>Danh sách tài sản</h3>
    </div>
    <div class="header-right">
      <p>Sở tài chính</p>
      <div class="header-year">
        <p>Năm</p>
        <div class="text">2022</div>
        <div class="up-down">
          <div class="up" @click="TrackingYear++">
            <div class="btn icon-up-bold"></div>
          </div>
          <div class="down" @click="TrackingYear--">
            <div class="btn icon-down-bold"></div>
          </div>
        </div>
      </div>
      <div class="icon">
        <ms-tool-tip content="Thông báo" position="bottom">
          <div class="icon-notify"></div>
        </ms-tool-tip>
      </div>
      <div class="icon">
        <ms-tool-tip content="Tiện ích" position="bottom">
          <div class="icon-option"></div>
        </ms-tool-tip>
      </div>

      <div class="icon">
        <ms-tool-tip content="Hướng dẫn" position="bottom">
          <div class="icon-help"></div>
        </ms-tool-tip>
      </div>

      <div class="icon user" @click="this.isShow = !this.isShow">
        <div class="icon-user"></div>
        <div class="user-info" v-if="isShow">
          <div class="user-name">Trung Đức</div>
          <div class="user-info-item" @click="onClickLogout">
            <div class="item-icon icon">
              <div class="icon-logout"></div>
            </div>
            <div class="item-text">Đăng xuất</div>
          </div>
        </div>
      </div>
      <div class="icon-down-bold"></div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import MsToolTip from "./base/MsToolTip.vue";
// import Axios from "axios";
export default {
  components: {
    MsToolTip,
  },
  data() {
    return {
      isShow: false,
    };
  },
  methods: {
    onClickLogout() {
      localStorage.removeItem("Token");
      axios.defaults.headers.common["Authorization"] = null;
      this.$router.push("/login");
    },
  },
};
</script>

<style scoped>
@import url(../css/main.css);
@import url(../css/layout/header.css);
</style>
