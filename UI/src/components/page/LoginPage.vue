<template>
  <div class="login">
    <div class="login-content">
      <div class="login-content-image"></div>
      <div class="login-content-form">
        <div class="logo">
          <img src="../../assets/img/Logo_MISA.svg.png" alt="" />
        </div>
        <div class="text">Đăng nhập để làm việc với <span>MISA QLTS</span></div>
        <input type="text" v-model="userName" />

        <input type="password" v-model="password" />

        <button @click="onClickLogin">Đăng nhập</button>

        <router-link to="/taisan">Quên mật khẩu?</router-link>

        <div v-if="errorLogin !== ''" class="error-login">{{ errorLogin }}</div>
      </div>
    </div>
    <div class="login-footer">footer</div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  created() {},
  data() {
    return {
      password: "",
      userName: "",
      reload: true,
      errorLogin: "",
    };
  },
  methods: {
    async onClickLogin() {
      const response = await axios({
        url: "https://localhost:44335/api/v1/Auths/sign-in",
        method: "Post",
        data: {
          userName: this.userName,
          password: this.password,
        },
      }).catch(() => {
        this.errorLogin = "Tài khoản hoặc mật khẩu không chính xác!";
      });

      console.log(response);
      localStorage.setItem("Token", response.data);
      axios.defaults.headers.common["Authorization"] =
        "Bearer " + localStorage.getItem("Token");
      this.$router.push("/taisan");
    },
  },
};
</script>
>

<style>
@import url("../../css/page/login.css");
</style>
