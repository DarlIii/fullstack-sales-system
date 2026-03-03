<template>
  <div class="page">
    <h2>Productos</h2>

    <h3>Crear Producto</h3>

<form @submit.prevent="crearProducto">
  <input v-model="nuevo.nombre" placeholder="Nombre" required />
  <input v-model="nuevo.descripcion" placeholder="Descripción" required />
  <input v-model.number="nuevo.precio" type="number" step="0.01" placeholder="Precio" required />
  <input v-model.number="nuevo.stock" type="number" placeholder="Stock" required />
  <input v-model.number="nuevo.categoriaId" type="number" placeholder="CategoriaId" required />

  <button type="submit">Crear</button>
</form>

<div v-if="editando">
  <h3>Editar Producto</h3>

  <form @submit.prevent="actualizarProducto">
    <input v-model="productoEditando.nombre" required />
    <input v-model="productoEditando.descripcion" required />
    <input v-model.number="productoEditando.precio" type="number" step="0.01" required />
    <input v-model.number="productoEditando.stock" type="number" required />
    <input v-model.number="productoEditando.categoriaId" type="number" required />

    <button type="submit">Actualizar</button>
    <button type="button" @click="editando = false">Cancelar</button>
  </form>
</div>

    <button @click="cargarProductos" :disabled="loading">
      {{ loading ? "Cargando..." : "Recargar" }}
    </button>

    <p v-if="error" class="error">{{ error }}</p>

    <table v-if="productos.length" class="table">
      <thead>
        <tr>
          <th>Id</th>
          <th>Nombre</th>
          <th>Precio</th>
          <th>Stock</th>
          <th>CategoriaId</th>
          <th>Acciones</th>
          <th>Descripción</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="p in productos" :key="p.id">
          <td>{{ p.id }}</td>
          <td>{{ p.nombre }}</td>
          <td>{{ p.precio }}</td>
          <td>{{ p.stock }}</td>
          <td>{{ p.descripcion }}</td>
          <td>{{ p.categoriaId }}</td>
          <td>
            <button @click="eliminarProducto(p.id)">Eliminar</button>
            <button @click="editarProducto(p)">Editar</button>
            </td>
        </tr>
      </tbody>
    </table>

    <p v-else-if="!loading">No hay productos.</p>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import http from "../api/http";

const productos = ref([]);
const loading = ref(false);
const error = ref("");
const nuevo = ref({
  nombre: "",
  descripcion: "",
  precio: 0,
  stock: 0,
  categoriaId: 0
});
const editando = ref(false);
const productoEditando = ref(null);

const cargarProductos = async () => {
  loading.value = true;
  error.value = "";
  try {
    const res = await http.get("/productos");
    productos.value = res.data;
  } catch (e) {
    error.value = "No se pudieron cargar los productos";
  } finally {
    loading.value = false;
  }
};

const eliminarProducto = async (id) => {
  if (!confirm("¿Seguro que quieres eliminar este producto?")) return;

  try {
    await http.delete(`/productos/${id}`);
    cargarProductos();
  } catch (e) {
    alert("Error al eliminar producto");
  }
};

const crearProducto = async () => {
  try {
    await http.post("/productos", nuevo.value);

    nuevo.value = {
      nombre: "",
      descripcion: "",
      precio: 0,
      stock: 0,
      categoriaId: 0
    };

    cargarProductos();
  } catch (e) {
    alert("Error al crear producto");
  }
};

const editarProducto = (producto) => {
  editando.value = true;
  productoEditando.value = { ...producto };
};

const actualizarProducto = async () => {
  try {
    await http.put(`/productos/${productoEditando.value.id}`, productoEditando.value);

    editando.value = false;
    productoEditando.value = null;

    cargarProductos();
  } catch (e) {
    alert("Error al actualizar producto");
  }
};

onMounted(cargarProductos);
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