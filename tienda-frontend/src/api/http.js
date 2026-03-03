import axios from "axios";

const http = axios.create({
  baseURL: "https://localhost:7188/api",
});

http.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  console.log("➡️ REQUEST:", config.method?.toUpperCase(), config.url);
  console.log("✅ TOKEN EN STORAGE:", token);

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
    console.log("✅ HEADER Authorization seteado");
  } else {
    console.log("❌ NO HAY TOKEN");
  }

  return config;
});

export default http;