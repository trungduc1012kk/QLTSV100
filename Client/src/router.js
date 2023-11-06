import { createRouter, createWebHistory } from "vue-router";
import HomePage from "../src/components/page/HomePage.vue";
import PropertyOverviewPage from "../src/components/page/PropertyOverviewPage.vue";
import PropertyManagementPage from "../src/components/page/PropertyManagementPage.vue";
import ToolPage from "../src/components/page/ToolPage.vue";
import CategoryPage from "../src/components/page/CategoryPage.vue";
import ReportPage from "../src/components/page/ReportPage.vue";
import SearchPage from "../src/components/page/SearchPage.vue";
import PropertyHBDTPage from "../src/components/page/PropertyHBDTPage.vue";
import LoginPage from "../src/components/page/LoginPage.vue";
import IncrementPropertyPage from "../src/components/page/IncrementPropertyPage.vue";
import Axios from "axios";

const routes = [
  {
    path: "/login",
    component: LoginPage,
  },
  {
    path: "/",
    component: HomePage,
    children: [
      {
        path: "/tongquan",
        component: PropertyOverviewPage,
      },
      {
        path: "/taisan",
        component: PropertyManagementPage,
      },
      {
        path: "/timkiem",
        component: SearchPage,
      },
      {
        path: "/congcu",
        component: ToolPage,
      },
      {
        path: "/loaitaisan",
        component: CategoryPage,
      },
      {
        path: "/baocao",
        component: ReportPage,
      },
      {
        path: "/taisanhbdt",
        component: PropertyHBDTPage,
      },
      {
        path: "/ghitang",
        component: IncrementPropertyPage,
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach(async (to) => {
  if (to.fullPath == "/login") return true;

  const response = await Axios({
    url: "https://localhost:44335/api/v1/Auths",
    method: "Get",
  }).catch(() => {
    return null;
  });

  if (!response) return "/login";
});

export default router;
