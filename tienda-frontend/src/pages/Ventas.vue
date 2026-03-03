<template>
  <div class="page">
    <h2>Registrar Venta</h2>

    <form @submit.prevent="crearVenta" class="form">
      <label>Cliente</label>
      <select v-model.number="venta.clienteId" required>
        <option disabled value="">Seleccione cliente</option>
        <option v-for="c in clientes" :key="c.id" :value="c.id">
          {{ c.nombre }}
        </option>
      </select>

      <label>Producto</label>
      <select v-model.number="detalle.productoId" required>
        <option disabled value="">Seleccione producto</option>
        <option v-for="p in productos" :key="p.id" :value="p.id">
          {{ p.nombre }}
        </option>
      </select>

      <label>Cantidad</label>
      <input v-model.number="detalle.cantidad" type="number" min="1" required />

      <button type="submit">Registrar Venta</button>
    </form>

    <p v-if="error" class="error">{{ error }}</p>

    <h3>Historial de Ventas</h3>

<table v-if="ventas.length" class="table">
  <thead>
    <tr>
      <th>Id</th>
      <th>Fecha</th>
      <th>Cliente</th>
      <th>Total</th>
    </tr>
  </thead>
  <tbody>
    <tr v-for="v in ventas" :key="v.id">
      <td>{{ v.id }}</td>
      <td>{{ new Date(v.fecha).toLocaleString() }}</td>
      <td>{{ v.cliente?.nombre }}</td>
      <td>{{ v.total }}</td>
    </tr>
  </tbody>
</table>

<p v-else>No hay ventas registradas.</p>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import http from "../api/http";

const clientes = ref([]);
const productos = ref([]);
const ventas = ref([]);
const error = ref("");

const venta = ref({
  clienteId: "",
  detalles: []
});

const detalle = ref({
  productoId: "",
  cantidad: 1
});

const cargarDatos = async () => {
  const resClientes = await http.get("/clientes");
  const resProductos = await http.get("/productos");
  const resVentas = await http.get("/ventas");
  clientes.value = resClientes.data;
  productos.value = resProductos.data;
  ventas.value = resVentas.data;
  
};

const crearVenta = async () => {
  try {
    venta.value.detalles = [detalle.value];
    await http.post("/ventas", venta.value);

    alert("Venta registrada correctamente");

    venta.value = { clienteId: "", detalles: [] };
    detalle.value = { productoId: "", cantidad: 1 };

  } catch (e) {
    error.value = "Error al registrar venta";
  }
};

onMounted(cargarDatos);
</script>

<style scoped>
.table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}
.table th,
.table td {
  border: 1px solid #ddd;
  padding: 8px;
}
.page {
  max-width: 600px;
  margin: 20px auto;
}
.form {
  display: grid;
  gap: 10px;
}
.error {
  color: red;
}
</style>