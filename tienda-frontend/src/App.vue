<template>
  <div>
    <Login v-if="!token" @logged="onLogged" />

    <div v-else>
      <h1>Panel Admin</h1>

      <Productos /> 
      <Clientes />
      <Ventas />  

      <button @click="logout">Cerrar sesión</button>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import Login from "./pages/Login.vue";
import Productos from "./pages/Productos.vue";
import Clientes from "./pages/Clientes.vue";
import Ventas from "./pages/Ventas.vue";

const token = ref(localStorage.getItem("token"));

const onLogged = () => {
  token.value = localStorage.getItem("token");
};

const logout = () => {
  localStorage.removeItem("token");
  token.value = null;
};
</script>