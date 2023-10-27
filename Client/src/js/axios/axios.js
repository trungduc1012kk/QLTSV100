import axios from "axios";

axios.defaults.headers.common["Authorization"] =
  "Bearer " + localStorage.getItem("Token");

const API = axios.create({
  // baseURL: "https://localhost:44335/api/v1/",
  headers: {
    Authorization: "Bearer " + localStorage.getItem("Token"),
  },
});

export default API;
