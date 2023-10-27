import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import "./js/axios/axios";

import ElementPlus from "element-plus";
import "element-plus/dist/index.css";
import vi from "element-plus/es/locale/lang/vi";

const app = createApp(App);
// Sử dụng router
app.use(router);
app.use(ElementPlus, {
  locale: vi,
});

app.mount("#app");
