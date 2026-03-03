<template>
  <div class="login-container">
    <h1>Iniciar sesión</h1>

    <form @submit.prevent="login">
      <label>Usuario</label>
      <input v-model="username" type="text" required />

      <label>Contraseña</label>
      <input v-model="password" type="password" required />

      <button type="submit" :disabled="loading">
        {{ loading ? "Entrando..." : "Entrar" }}
      </button>

      <p v-if="error" class="error">{{ error }}</p>
    </form>
  </div>
</template>

<script setup>
import { ref } from "vue";
import http from "../api/http";

const emit = defineEmits(["logged"]);
const username = ref("");
const password = ref("");
const loading = ref(false);
const error = ref("");

const login = async () => {
  error.value = "";
  loading.value = true;

  try {
    const res = await http.post("/auth/login", {
    username: username.value,
    passwordHash: password.value,
    });

    localStorage.setItem("token", res.data.token);

    alert("Login correcto ✅ Token guardado");
    emit("logged");
  } catch (e) {
    error.value = "Usuario o contraseña incorrectos";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.login-container {
  max-width: 320px;
  margin: 60px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
}
label {
  display: block;
  margin-top: 10px;
}
input {
  width: 100%;
  padding: 8px;
  margin-top: 4px;
}
button {
  width: 100%;
  margin-top: 15px;
  padding: 10px;
}
.error {
  margin-top: 10px;
  color: red;
}
</style>