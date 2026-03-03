<template>
  <div class="page">
    <h2>Clientes</h2>

    <h3>Crear Cliente</h3>

<form @submit.prevent="crearCliente" class="form">
  <input v-model="nuevo.nombre" placeholder="Nombre" required />
  <input v-model="nuevo.email" type="email" placeholder="Email" required />
  <input v-model="nuevo.telefono" placeholder="Teléfono" required />
  <button type="submit">Crear</button>
</form>

<div v-if="editando" style="margin-top: 15px;">
  <h3>Editar Cliente</h3>

  <form @submit.prevent="actualizarCliente" class="form">
    <input v-model="clienteEditando.nombre" placeholder="Nombre" required />
    <input v-model="clienteEditando.email" type="email" placeholder="Email" required />
    <input v-model="clienteEditando.telefono" placeholder="Teléfono" required />

    <button type="submit">Actualizar</button>
    <button type="button" @click="cancelarEdicion">Cancelar</button>
  </form>
</div>

    <button @click="cargarClientes" :disabled="loading">
      {{ loading ? "Cargando..." : "Recargar" }}
    </button>

    <p v-if="error" class="error">{{ error }}</p>

    <table v-if="clientes.length" class="table">
      <thead>
        <tr>
          <th>Id</th>
          <th>Nombre</th>
          <th>Email</th>
          <th>Teléfono</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="c in clientes" :key="c.id">
          <td>{{ c.id }}</td>
          <td>{{ c.nombre }}</td>
          <td>{{ c.email }}</td>
          <td>{{ c.telefono }}</td>
          <td>
            <button @click="eliminarCliente(c.id)">Eliminar</button>
            <button @click="editarCliente(c)">Editar</button>
            </td>
        </tr>
      </tbody>
    </table>

    <p v-else-if="!loading">No hay clientes.</p>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import http from "../api/http";

const clientes = ref([]);
const loading = ref(false);
const error = ref("");
const editando = ref(false);
const clienteEditando = ref(null);

const nuevo = ref({
  nombre: "",
  email: "",
  telefono: ""
});

const cargarClientes = async () => {
  loading.value = true;
  error.value = "";
  try {
    const res = await http.get("/clientes");
    clientes.value = res.data;
  } catch (e) {
    error.value = "No se pudieron cargar los clientes";
  } finally {
    loading.value = false;
  }
};

const crearCliente = async () => {
  try {
    await http.post("/clientes", nuevo.value);

    nuevo.value = { nombre: "", email: "", telefono: "" };
    cargarClientes();
  } catch (e) {
    alert("Error al crear cliente");
  }
};

const eliminarCliente = async (id) => {
  if (!confirm("¿Seguro que quieres eliminar este cliente?")) return;

  try {
    await http.delete(`/clientes/${id}`);
    cargarClientes();
  } catch (e) {
    alert("Error al eliminar cliente");
  }
};
const editarCliente = (cliente) => {
  editando.value = true;
  clienteEditando.value = { ...cliente };
};

const actualizarCliente = async () => {
  try {
    await http.put(`/clientes/${clienteEditando.value.id}`, clienteEditando.value);

    editando.value = false;
    clienteEditando.value = null;

    cargarClientes();
  } catch (e) {
    alert("Error al actualizar cliente");
  }
};

const cancelarEdicion = () => {
  editando.value = false;
  clienteEditando.value = null;
};

onMounted(cargarClientes);
</script>

<style scoped>


.page {
  max-width: 900px;
  margin: 20px auto;
  padding: 10px;
}
.table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 15px;
}
.table th,
.table td {
  border: 1px solid #ddd;
  padding: 8px;
}
.error {
  color: red;
  margin-top: 10px;
}
button {
  margin-top: 10px;
  padding: 8px 12px;
}
</style>